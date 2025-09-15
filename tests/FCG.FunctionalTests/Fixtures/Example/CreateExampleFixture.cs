using AutoFixture;
using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;
using FCG.CommomTestsUtilities.Builders.Repositories.ExampleRepository;

namespace FCG.FunctionalTests.Fixtures.Example
{
    public class CreateExampleFixture
    {
        public CreateExampleFixture()
        {
            var writeOnlyExampleRepository = WriteOnlyExampleRepositoryBuilder.Build();
            CreateExampleUseCase = new CreateExampleUseCase(writeOnlyExampleRepository);
            CreateExampleInput = CreateExampleInputBuilder.Build();
        }

        public static Fixture Fixture => new();
        public CreateExampleUseCase CreateExampleUseCase { get; }
        public CreateExampleInput CreateExampleInput { get; }
    }
}
