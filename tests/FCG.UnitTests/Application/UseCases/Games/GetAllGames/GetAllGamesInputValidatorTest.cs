using FCG.Application.UseCases.Games.GetAll;
using FCG.Messages;
using FluentValidation.TestHelper;

namespace FCG.UnitTests.Application.UseCases.Games.GetAllGames
{
    public class GetAllGamesInputValidatorTest
    {
        private readonly GetAllGamesInputValidator _validator;

        public GetAllGamesInputValidatorTest()
        {
            _validator = new GetAllGamesInputValidator();
        }

        #region Valid Cases

        [Fact]
        public void Given_ValidInputWithoutFilters_When_Validate_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = null
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Given_ValidInputWithEmptyFilter_When_Validate_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter()
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Given_ValidInputWithNameFilter_When_Validate_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { Name = "The Witcher" }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Given_ValidInputWithCategoryFilter_When_Validate_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter { Category = "RPG" }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Given_ValidInputWithPriceRangeFilter_When_Validate_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = 10.00m,
                    MaxPrice = 50.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Given_ValidInputWithAllFilters_When_Validate_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    Name = "Dark Souls",
                    Category = "RPG",
                    MinPrice = 20.00m,
                    MaxPrice = 60.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        #endregion

        #region Name Filter Validation

        [Fact]
        public void Given_InputWithNameFilterExceedingMaxLength_When_Validate_ShouldHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    Name = new string('A', 256) // 256 characters
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filter.Name)
                .WithErrorMessage(ResourceMessages.GameNameMaxLength);
        }

        [Fact]
        public void Given_InputWithNameFilterAt255Characters_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    Name = new string('A', 255) // Exactly 255 characters
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.Name);
        }

        #endregion

        #region Category Filter Validation

        [Fact]
        public void Given_InputWithCategoryFilterExceedingMaxLength_When_Validate_ShouldHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    Category = new string('B', 101) // 101 characters
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filter.Category)
                .WithErrorMessage(ResourceMessages.GameCategoryMaxLength);
        }

        [Fact]
        public void Given_InputWithCategoryFilterAt100Characters_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    Category = new string('B', 100) // Exactly 100 characters
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.Category);
        }

        #endregion

        #region MinPrice Filter Validation

        [Fact]
        public void Given_InputWithNegativeMinPrice_When_Validate_ShouldHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = -10.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filter.MinPrice)
                .WithErrorMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);
        }

        [Fact]
        public void Given_InputWithZeroMinPrice_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = 0.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.MinPrice);
        }

        #endregion

        #region MaxPrice Filter Validation

        [Fact]
        public void Given_InputWithNegativeMaxPrice_When_Validate_ShouldHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MaxPrice = -20.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filter.MaxPrice)
                .WithErrorMessage(ResourceMessages.GamePriceMustBeGreaterThanZero);
        }

        [Fact]
        public void Given_InputWithZeroMaxPrice_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MaxPrice = 0.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.MaxPrice);
        }

        #endregion

        #region Price Range Validation

        [Fact]
        public void Given_InputWithMaxPriceLessThanMinPrice_When_Validate_ShouldHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = 50.00m,
                    MaxPrice = 30.00m // MaxPrice < MinPrice
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filter.MaxPrice)
                .WithErrorMessage(ResourceMessages.GameMaxPriceMustBeGreaterThanMinPrice);
        }

        [Fact]
        public void Given_InputWithMaxPriceEqualToMinPrice_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = 40.00m,
                    MaxPrice = 40.00m // MaxPrice == MinPrice
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.MaxPrice);
        }

        [Fact]
        public void Given_InputWithOnlyMinPriceSet_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = 10.00m,
                    MaxPrice = null
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.MaxPrice);
        }

        [Fact]
        public void Given_InputWithOnlyMaxPriceSet_When_Validate_ShouldNotHaveValidationError()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    MinPrice = null,
                    MaxPrice = 100.00m
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Filter.MaxPrice);
        }

        #endregion

        #region Multiple Validation Errors

        [Fact]
        public void Given_InputWithMultipleInvalidFilters_When_Validate_ShouldHaveMultipleValidationErrors()
        {
            // Arrange
            var input = new GetAllGamesInput
            {
                PageNumber = 1,
                PageSize = 10,
                Filter = new GameFilter
                {
                    Name = new string('A', 300), // Exceeds max length
                    Category = new string('B', 150), // Exceeds max length
                    MinPrice = -10.00m, // Negative
                    MaxPrice = -20.00m // Negative
                }
            };

            // Act
            var result = _validator.TestValidate(input);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Filter.Name);
            result.ShouldHaveValidationErrorFor(x => x.Filter.Category);
            result.ShouldHaveValidationErrorFor(x => x.Filter.MinPrice);
            result.ShouldHaveValidationErrorFor(x => x.Filter.MaxPrice);
        }

        #endregion
    }
}