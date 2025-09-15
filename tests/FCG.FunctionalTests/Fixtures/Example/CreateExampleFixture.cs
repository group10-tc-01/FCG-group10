using AutoFixture;
using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;

namespace FCG.FunctionalTests.Fixtures.Example
{
    public class CreateExampleFixture
    {
        public CreateExampleFixture()
        {
            CreateExampleUseCase = new CreateExampleUseCase();
            CreateExampleInput = CreateExampleInputBuilder.Build();
        }

        public static Fixture Fixture => new();
        public CreateExampleUseCase CreateExampleUseCase { get; }
        public CreateExampleInput CreateExampleInput { get; }
    }
}
