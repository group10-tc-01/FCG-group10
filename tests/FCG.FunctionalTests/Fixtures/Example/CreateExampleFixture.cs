using AutoFixture;
using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;
using FCG.CommomTestsUtilities.Builders.Repositories;
using FCG.CommomTestsUtilities.Builders.Repositories.ExampleRepository;

namespace FCG.FunctionalTests.Fixtures.Example
{
    public class CreateExampleFixture
    {
        public CreateExampleFixture()
        {
            var writeOnlyExampleRepository = WriteOnlyExampleRepositoryBuilder.Build();
            var unitOfWork = UnitOfWorkBuilder.Build();
            UnitOfWorkBuilder.SetupSaveChangesAsync(1);
            UnitOfWorkBuilder.SetupCommitAsync(1);

            CreateExampleUseCase = new CreateExampleUseCase(writeOnlyExampleRepository, unitOfWork);
            CreateExampleInput = CreateExampleInputBuilder.Build();
        }

        public static Fixture Fixture => new();
        public CreateExampleUseCase CreateExampleUseCase { get; }
        public CreateExampleInput CreateExampleInput { get; }
    }
}
