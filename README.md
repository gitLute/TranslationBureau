# Бюро переводов

![Build Status](https://github.com/gitLute/TranslationBureau/actions/workflows/build.yml/badge.svg)

Web-приложение баз данных «Бюро переводов», разработанное в ходе изучения
дисциплины «Разработка приложений баз данных для информационных систем».

## Состав решения

| Проект | Назначение |
| --- | --- |
| `TranslationBureau.Domain` | доменное ядро |
| `TranslationBureau.Application` | сценарии использования |
| `TranslationBureau.Infrastructure` | инфраструктура и доступ к данным |
| `TranslationBureau.Web` | Web-приложение ASP.NET Core MVC |
| `TranslationBureau.Tests` | модульные тесты |

## Требования

Для сборки и запуска требуются пакет .NET 10.0 SDK и система управления
базами данных PostgreSQL.

Версия пакета средств разработки зафиксирована в файле `global.json`;
параметр `rollForward` допускает использование любой более поздней версии
10.0 в пределах того же канала выпуска.

## Запуск

```bash
dotnet run --project src/TranslationBureau.Web
```

Строка подключения к базе данных размещается в файле
`src/TranslationBureau.Web/appsettings.json` и не содержит пароля.
При необходимости задать её в обход файла следует использовать
`dotnet user-secrets`.

## Сборка и публикация

```bash
# сборка в конфигурации Release
dotnet build src/TranslationBureau.slnx --configuration Release

# развёртывание, зависящее от платформы
dotnet publish src/TranslationBureau.Web --configuration Release --output ./publish

# самодостаточное развёртывание для платформы Windows
dotnet publish src/TranslationBureau.Web --configuration Release \
    --runtime win-x64 --self-contained true --output ./publish-win-x64
```

Развёртывание, зависящее от платформы, занимает около 21 МБ и требует на
целевом компьютере установленной среды выполнения .NET 10. Самодостаточное
развёртывание занимает около 127 МБ и включает среду выполнения в состав
пакета.

## Выполнение модульных тестов

```bash
dotnet test
```

Тесты не обращаются к базе данных: проверяются правила доменного ядра,
поэтому успешно выполняются на виртуальной машине конвейера.

Отчёт о покрытии кода формируется командой:

```bash
dotnet test src/TranslationBureau.slnx --configuration Release \
    --collect:"XPlat Code Coverage"
```

## Непрерывная интеграция

Описание конвейера размещено в файле `.github/workflows/build.yml`. При каждом
изменении ветви `main` конвейер выполняет восстановление зависимостей,
сборку в конфигурации Release, модульные тесты с формированием отчёта о
покрытии кода и публикацию Web-приложения под управлением Linux и Windows.
Сформированные пакеты развёртывания сохраняются в качестве артефактов сборки.