# Сборка образа контейнера с Web-приложением «Бюро переводов».
# Образ воспроизводим: зависимости восстанавливаются при сборке,
# среда выполнения берётся из официального образа .NET.

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Сначала копируются файлы проектов — это позволяет кэшировать слой
# восстановления зависимостей до копирования исходного кода.
COPY src/TranslationBureau.Domain/TranslationBureau.Domain.csproj \
     src/TranslationBureau.Domain/
COPY src/TranslationBureau.Application/TranslationBureau.Application.csproj \
     src/TranslationBureau.Application/
COPY src/TranslationBureau.Infrastructure/TranslationBureau.Infrastructure.csproj \
     src/TranslationBureau.Infrastructure/
COPY src/TranslationBureau.Web/TranslationBureau.Web.csproj \
     src/TranslationBureau.Web/
COPY src/TranslationBureau.slnx src/

RUN dotnet restore src/TranslationBureau.slnx

COPY . .

RUN dotnet publish src/TranslationBureau.Web \
        --configuration Release \
        --no-restore \
        --output /app

# Среда выполнения: образ существенно меньше образа со средствами разработки.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=build /app .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TranslationBureau.Web.dll"]