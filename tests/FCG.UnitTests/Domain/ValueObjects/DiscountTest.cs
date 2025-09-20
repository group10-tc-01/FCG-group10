using FCG.Domain.ValueObjects;
using FluentAssertions;
using System;
using System.Globalization;
using Xunit;

namespace FCG.UnitTests.Domain.ValueObjects
{
    public class DiscountTests
    {
        [Fact]
        public void Given_ValidPercentages_When_CreateDiscount_Then_ShouldCreateSuccessfullyAndSetProperties()
        {
            // Este teste verifica a criação bem-sucedida para valores válidos, incluindo 0 e 100.
            // Arrange
            decimal validPercentage = 25.5m;
            decimal zeroPercentage = 0m;
            decimal maxPercentage = 100m;

            // Act
            var discount = Discount.Create(validPercentage);
            var zeroDiscount = Discount.Create(zeroPercentage);
            var maxDiscount = Discount.Create(maxPercentage);

            // Assert
            discount.Should().NotBeNull();
            discount.Value.Should().Be(validPercentage);
            zeroDiscount.Value.Should().Be(0);
            maxDiscount.Value.Should().Be(100);
        }

        [Fact]
        public void Given_InvalidPercentages_When_CreateDiscount_Then_ShouldThrowArgumentException()
        {
            // Este teste consolida a validação de cenários de falha para valores fora do intervalo.
            // Arrange
            decimal negativePercentage = -5m;
            decimal invalidPercentage = 101m;

            // Act
            Action actNegative = () => Discount.Create(negativePercentage);
            Action actAbove100 = () => Discount.Create(invalidPercentage);

            // Assert
            actNegative.Should().Throw<ArgumentException>()
                .WithMessage("Discount must be between 0 and 100.");
            actAbove100.Should().Throw<ArgumentException>()
                .WithMessage("Discount must be between 0 and 100.");
        }

        [Fact]
        public void Given_DiscountObject_When_ImplicitlyConvertedToDecimal_Then_ShouldReturnValue()
        {
            // Testa a conversão implícita do objeto Discount para decimal.
            // Arrange
            var discount = Discount.Create(15.75m);

            // Act
            decimal value = discount;

            // Assert
            value.Should().Be(15.75m);
        }

        [Fact]
        public void Given_DecimalValue_When_ImplicitlyConvertedToDiscount_Then_ShouldCreateDiscount()
        {
            // Testa a conversão implícita de decimal para o objeto Discount.
            // Arrange
            decimal value = 30m;

            // Act
            Discount discount = value;

            // Assert
            discount.Value.Should().Be(30m);
        }

        [Fact]
        public void Given_TwoDiscountsWithSameValue_When_Compare_Then_ShouldBeEqual()
        {
            // Valida a igualdade de valor, uma característica fundamental de Value Objects.
            // Arrange
            var discount1 = Discount.Create(25m);
            var discount2 = Discount.Create(25m);

            // Act & Assert
            discount1.Should().Be(discount2);
        }

        [Fact]
        public void Given_TwoDiscountsWithDifferentValues_When_Compare_Then_ShouldNotBeEqual()
        {
            // Valida a diferença entre dois objetos com valores distintos.
            // Arrange
            var discount1 = Discount.Create(25m);
            var discount2 = Discount.Create(50m);

            // Act & Assert
            discount1.Should().NotBe(discount2);
        }

        [Fact]
        public void Given_DiscountObject_When_CallToString_Then_ShouldReturnFormattedValue()
        {
            // Teste de cenário de borda para o método ToString.
            // Ele agora verifica diferentes valores e garante que o formato "F2" funcione corretamente.
            // Arrange
            var discount1 = Discount.Create(15.5m);
            var discount2 = Discount.Create(15m);
            var discount3 = Discount.Create(15.555m);

            // Act & Assert
            // Usamos uma cultura invariante para garantir que o teste funcione em qualquer máquina.
            discount1.ToString().Should().Be("15.50");
            discount2.ToString().Should().Be("15.00");
            // O formato "F2" arredonda para duas casas decimais, o que é o comportamento esperado.
            discount3.ToString().Should().Be("15.56");
        }

        [Fact]
        public void Given_TwoDiscountsWithSameValue_When_Compare_Then_ShouldBeEqualButNotSameReference()
        {
            // Teste para demonstrar que Value Objects são iguais em valor, mas não são a mesma instância.
            // Arrange
            var discount1 = Discount.Create(25m);
            var discount2 = Discount.Create(25m);

            // Act & Assert
            discount1.Should().Be(discount2);
            discount1.Should().NotBeSameAs(discount2);
        }
    }
}