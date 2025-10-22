using FCG.Application.UseCases.Games.Register;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Games.Register;
using FCG.CommomTestsUtilities.Builders.Repositories.GameRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.GamesRepository;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Games.Register
{
    public class RegisterGameUseCaseTest
    {
        private readonly IWriteOnlyGameRepository _writeOnlyGameRepository;
        private readonly IReadOnlyGameRepository _readOnlyGameRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRegisterGameUseCase _sut;

        public RegisterGameUseCaseTest()
        {
            _writeOnlyGameRepository = WriteOnlyGameRepositoryBuilder.Build();
            _readOnlyGameRepository = ReadOnlyGameRepositoryBuilder.Build();
            _unitOfWork = UnitOfWorkBuilder.Build();
            _sut = new RegisterGameUseCase(_writeOnlyGameRepository, _readOnlyGameRepository, _unitOfWork);
        }

        [Fact]
        public async Task Given_ValidRegisterGameInput_When_Handle_Then_ShouldReturnOutput()
        {
            // Arrange
            var input = RegisterGameInputBuilder.Build();
            Setup();

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.Name.Should().Be(input.Name);

            ReadOnlyGameRepositoryBuilder.VerifyGetByNameAsyncWasCalled();
            WriteOnlyGameRepositoryBuilder.VerifyAddAsyncWasCalled();
            UnitOfWorkBuilder.VerifySaveChangesAsyncWasCalled();
        }

        [Fact]
        public async Task Given_RegisterGameInputWithExistingName_When_Handle_Then_ShouldThrowConflictException()
        {
            // Arrange
            var existingGame = GameBuilder.Build();
            var input = RegisterGameInputBuilder.BuildWithName(existingGame.Name.Value);
            SetupWithExistingGame(existingGame);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ConflictException>(() => _sut.Handle(input, CancellationToken.None));

            exception.Should().NotBeNull();
            exception.Message.Should().Contain(existingGame.Name.Value);
        }

        [Fact]
        public async Task Given_ValidRegisterGameInput_When_Handle_Then_ShouldAddGameToRepository()
        {
            // Arrange
            var input = RegisterGameInputBuilder.Build();
            Setup();

            // Act
            await _sut.Handle(input, CancellationToken.None);

            // Assert
            WriteOnlyGameRepositoryBuilder.VerifyAddAsyncWasCalled();
        }

        [Fact]
        public async Task Given_ValidRegisterGameInput_When_Handle_Then_ShouldSaveChanges()
        {
            // Arrange
            var input = RegisterGameInputBuilder.Build();
            Setup();

            // Act
            await _sut.Handle(input, CancellationToken.None);

            // Assert
            UnitOfWorkBuilder.VerifySaveChangesAsyncWasCalled();
        }

        [Fact]
        public async Task Given_ValidRegisterGameInput_When_Handle_Then_ShouldCheckIfGameExists()
        {
            // Arrange
            var input = RegisterGameInputBuilder.Build();
            Setup();

            // Act
            await _sut.Handle(input, CancellationToken.None);

            // Assert
            ReadOnlyGameRepositoryBuilder.VerifyGetByNameAsyncWasCalledWith(input.Name);
        }

        private static void Setup()
        {
            ReadOnlyGameRepositoryBuilder.SetupGetByNameAsync(null);
            WriteOnlyGameRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
        }

        private static void SetupWithExistingGame(Game existingGame)
        {
            ReadOnlyGameRepositoryBuilder.SetupGetByNameAsyncWithSpecificName(existingGame.Name.Value, existingGame);
            WriteOnlyGameRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
        }
    }
}