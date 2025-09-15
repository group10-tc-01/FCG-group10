using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Example.CreateExample
{
    public class CreateExampleInputValidatorTest
    {
        private readonly CreateExampleInputValidator _validator;

        public CreateExampleInputValidatorTest()
        {
            _validator = new CreateExampleInputValidator();
        }

        [Fact]
        public void Should_ValidInput_When_Validate_Then_ShouldBeValid()
        {
            // Arrange
            var input = CreateExampleInputBuilder.Build();

            // Act
            var result = _validator.Validate(input);

            // Assert
            result.IsValid.Should().BeTrue();
        }
    }
}
