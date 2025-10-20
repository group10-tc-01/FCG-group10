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

        [Theory(DisplayName = "PageNumber deve aceitar valores válidos")]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        public void PageNumber_GivenValidValues_ShouldPassValidation(int validPageNumber)
        {
            // Arrange
            var paginationParams = new PaginationParams { PageNumber = validPageNumber };
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
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
            var paginationParams = new PaginationParams { PageNumber = invalidPageNumber };
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
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
            var paginationParams = new PaginationParams { PageSize = validPageSize };
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
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
            var paginationParams = new PaginationParams { PageSize = invalidPageSize };
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(1);
            validationResults[0].ErrorMessage.Should().Be("PageSize deve estar entre 1 e 50");
            validationResults[0].MemberNames.Should().Contain("PageSize");
        }

        [Fact(DisplayName = "Deve validar múltiplas propriedades inválidas simultaneamente")]
        public void Validation_GivenMultipleInvalidProperties_ShouldReturnAllValidationErrors()
        {
            // Arrange
            var paginationParams = new PaginationParams
            {
                PageNumber = 0,
                PageSize = 51
            };
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
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
            var paginationParams = new PaginationParams
            {
                PageNumber = 2,
                PageSize = 20
            };
            var validationContext = new ValidationContext(paginationParams);
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(paginationParams, validationContext, validationResults, true);

            // Assert
            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
            paginationParams.PageNumber.Should().Be(2);
            paginationParams.PageSize.Should().Be(20);
        }
    }
}