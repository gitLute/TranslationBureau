using TranslationBureau.Domain.Entities;
using TranslationBureau.Domain.Exceptions;
using Xunit;

namespace TranslationBureau.Tests;

public class TranslatorTests
{
    [Fact]
    public void Create_ПриКорректныхДанных_СоздаётАктивногоПереводчика()
    {
        var translator = Translator.Create("Иванова Мария Петровна",
            "+375291234567", "ivanova@example.com", "высшая", 22m);

        Assert.Equal("Иванова Мария Петровна", translator.FullName);
        Assert.Equal(22m, translator.RatePerUnit);
        Assert.True(translator.IsActive);
    }

    [Fact]
    public void Create_ПриПустомЗначенииФИО_ВозбуждаетИсключение()
    {
        Assert.Throws<DomainException>(() =>
            Translator.Create("   ", null, null, "первая", 18m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_ПриНеположительнойСтавке_ВозбуждаетИсключение(decimal rate)
    {
        Assert.Throws<DomainException>(() =>
            Translator.Create("Петров Сергей Николаевич", null, null, "первая", rate));
    }

    [Fact]
    public void Deactivate_ПереводитЗаписьВАрхивноеСостояние()
    {
        var translator = Translator.Create("Сидорова Анна Игоревна",
            null, null, "вторая", 15m);

        translator.Deactivate();

        Assert.False(translator.IsActive);
    }

    [Fact]
    public void Activate_ВозвращаетЗаписьВРабочееСостояние()
    {
        var translator = Translator.Create("Сидорова Анна Игоревна",
            null, null, "вторая", 15m);

        translator.Deactivate();
        translator.Activate();

        Assert.True(translator.IsActive);
    }

    [Fact]
    public void Update_ИзменяетРеквизитыПереводчика()
    {
        var translator = Translator.Create("Сидорова Анна Игоревна",
            null, null, "вторая", 15m);

        translator.Update("  Кузнецов Олег Павлович  ", "+375291112233",
            "kuznetsov@example.com", "высшая", 30m);

        Assert.Equal("Кузнецов Олег Павлович", translator.FullName);
        Assert.Equal("+375291112233", translator.Phone);
        Assert.Equal("kuznetsov@example.com", translator.Email);
        Assert.Equal("высшая", translator.Category);
        Assert.Equal(30m, translator.RatePerUnit);
    }

    [Fact]
    public void Update_ПриНеположительнойСтавке_ВозбуждаетИсключение()
    {
        var translator = Translator.Create("Сидорова Анна Игоревна",
            null, null, "вторая", 15m);

        Assert.Throws<DomainException>(() =>
            translator.Update("Сидорова Анна Игоревна", null, null, "вторая", -5m));
    }

    [Fact]
    public void Create_ОбрезаетПробелыВНачалеИКонцеФИО()
    {
        var translator = Translator.Create("  Иванов Иван Иванович  ",
            null, null, "первая", 10m);

        Assert.Equal("Иванов Иван Иванович", translator.FullName);
    }
}