using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Example.CreateExample
{
    public class CreateExampleUseCaseTest
    {
        private readonly ICreateExampleUseCase _useCase;

        public CreateExampleUseCaseTest()
        {
            _useCase = new CreateExampleUseCase();
        }

        [Fact]
        public async Task Should_CreateExample_When_Handle_Then_ShouldReturnOutput()
        {
            // Arrange
            var input = CreateExampleInputBuilder.Build();

            // Act
            var result = await _useCase.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Example.Should().NotBeNull();
            result.Example.Name.Should().Be(input.Name);
            result.Example.Description.Should().Be(input.Description);
        }
    }
}
