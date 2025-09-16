using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;
using FCG.CommomTestsUtilities.Builders.Repositories;
using FCG.CommomTestsUtilities.Builders.Repositories.ExampleRepository;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.ExampleRepository;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Example.CreateExample
{
    public class CreateExampleUseCaseTest
    {
        private readonly ICreateExampleUseCase _useCase;
        private readonly IWriteOnlyExampleRepository _writeOnlyExampleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExampleUseCaseTest()
        {
            _unitOfWork = UnitOfWorkBuilder.Build();
            _writeOnlyExampleRepository = WriteOnlyExampleRepositoryBuilder.Build();
            _useCase = new CreateExampleUseCase(_writeOnlyExampleRepository, _unitOfWork);
        }

        [Fact]
        public async Task Should_CreateExample_When_Handle_Then_ShouldReturnOutput()
        {
            // Arrange
            var input = CreateExampleInputBuilder.Build();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
            UnitOfWorkBuilder.SetupCommitAsync();

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
