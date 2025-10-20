using FCG.Application.Shared.Params;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace FCG.UnitTests.Application.Shared.Params
{
    public class PaginationParamsTests
    {
        [Fact(DisplayName = "Construtor deve definir valores padrão corretamente")]
        public void Constructor_ShouldSetDefaultValuesCorrectly()
        {
            // Act
            var paginationParams = new PaginationParams();

            // Assert
            paginationParams.PageNumber.Should().Be(1);
            paginationParams.PageSize.Should().Be(10);
        }

        [Fact(DisplayName = "PageNumber deve permitir get e set corretamente")]
        public void PageNumber_ShouldAllowGetAndSet()
        {
            // Arrange
            var paginationParams = new PaginationParams();
            const int expectedPageNumber = 5;

            // Act
            paginationParams.PageNumber = expectedPageNumber;
            var actualPageNumber = paginationParams.PageNumber;

            // Assert
            actualPageNumber.Should().Be(expectedPageNumber);
        }

        [Fact(DisplayName = "PageSize deve permitir get e set corretamente")]
        public void PageSize_ShouldAllowGetAndSet()
        {
            // Arrange
            var paginationParams = new PaginationParams();
            const int expectedPageSize = 25;

            // Act
            paginationParams.PageSize = expectedPageSize;
            var actualPageSize = paginationParams.PageSize;

            // Assert
            actualPageSize.Should().Be(expectedPageSize);
        }

        [Theory(DisplayName = "PageNumber deve aceitar valores válidos")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void PageNumber_GivenValidValues_ShouldPassValidation(int validPageNumber)
        {
            // Arrange
            var paginationParams = new PaginationParams();
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            paginationParams.PageNumber = validPageNumber;
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            paginationParams.PageNumber.Should().Be(validPageNumber);
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Theory(DisplayName = "PageNumber deve rejeitar valores inválidos")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void PageNumber_GivenInvalidValues_ShouldFailValidation(int invalidPageNumber)
        {
            // Arrange
            var paginationParams = new PaginationParams();
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            paginationParams.PageNumber = invalidPageNumber;
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            paginationParams.PageNumber.Should().Be(invalidPageNumber);
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);
            validationResults[0].ErrorMessage.Should().Be("PageNumber deve ser maior que 0");
            validationResults[0].MemberNames.Should().Contain("PageNumber");
        }

        [Theory(DisplayName = "PageSize deve aceitar valores válidos")]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(25)]
        [InlineData(50)]
        public void PageSize_GivenValidValues_ShouldPassValidation(int validPageSize)
        {
            // Arrange
            var paginationParams = new PaginationParams();
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            paginationParams.PageSize = validPageSize;
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            paginationParams.PageSize.Should().Be(validPageSize);
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Theory(DisplayName = "PageSize deve rejeitar valores inválidos")]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(51)]
        [InlineData(100)]
        public void PageSize_GivenInvalidValues_ShouldFailValidation(int invalidPageSize)
        {
            // Arrange
            var paginationParams = new PaginationParams();
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            paginationParams.PageSize = invalidPageSize;
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            paginationParams.PageSize.Should().Be(invalidPageSize);
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);
            validationResults[0].ErrorMessage.Should().Be("PageSize deve estar entre 1 e 50");
            validationResults[0].MemberNames.Should().Contain("PageSize");
        }

        [Fact(DisplayName = "Deve validar múltiplas propriedades inválidas simultaneamente")]
        public void Validation_GivenMultipleInvalidProperties_ShouldReturnAllValidationErrors()
        {
            // Arrange
            var paginationParams = new PaginationParams();
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            paginationParams.PageNumber = 0;
            paginationParams.PageSize = 51;
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            paginationParams.PageNumber.Should().Be(0);
            paginationParams.PageSize.Should().Be(51);
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(2);

            var pageNumberError = validationResults.FirstOrDefault(r => r.MemberNames.Contains("PageNumber"));
            pageNumberError.Should().NotBeNull();
            pageNumberError!.ErrorMessage.Should().Be("PageNumber deve ser maior que 0");

            var pageSizeError = validationResults.FirstOrDefault(r => r.MemberNames.Contains("PageSize"));
            pageSizeError.Should().NotBeNull();
            pageSizeError!.ErrorMessage.Should().Be("PageSize deve estar entre 1 e 50");
        }

        [Fact(DisplayName = "Deve validar com sucesso quando todas as propriedades são válidas")]
        public void Validation_GivenAllValidProperties_ShouldPassCompleteValidation()
        {
            // Arrange
            var paginationParams = new PaginationParams();
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            paginationParams.PageNumber = 2;
            paginationParams.PageSize = 20;
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            paginationParams.PageNumber.Should().Be(2);
            paginationParams.PageSize.Should().Be(20);
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }

        [Fact(DisplayName = "Deve testar valores limite das propriedades")]
        public void Properties_ShouldHandleBoundaryValues()
        {
            // Arrange
            var paginationParams = new PaginationParams();

            // Act & Assert 
            paginationParams.PageNumber = 1;
            paginationParams.PageNumber.Should().Be(1);

            paginationParams.PageNumber = int.MaxValue;
            paginationParams.PageNumber.Should().Be(int.MaxValue);

            // Act & Assert 
            paginationParams.PageSize = 1;
            paginationParams.PageSize.Should().Be(1);

            paginationParams.PageSize = 50;
            paginationParams.PageSize.Should().Be(50);
        }
    }
}