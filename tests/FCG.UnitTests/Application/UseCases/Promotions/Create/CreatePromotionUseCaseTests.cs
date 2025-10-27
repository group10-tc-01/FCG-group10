using FCG.Application.UseCases.Promotions.Create;
using FCG.CommomTestsUtilities.Builders.Inputs.Promotions.Create;
using FCG.CommomTestsUtilities.Builders.Repositories.GameRepository;
using FCG.CommomTestsUtilities.Builders.Repositories.PromotionRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Exceptions;
using FluentAssertions;

namespace FCG.UnitTests.Application.UseCases.Promotions.Create
{
    public class CreatePromotionUseCaseTests
    {
        public CreatePromotionUseCaseTests()
        {
            // Reset builders before each test
            ReadOnlyGameRepositoryBuilder.Reset();
            ReadOnlyPromotionRepositoryBuilder.Reset();
            WriteOnlyPromotionRepositoryBuilder.Reset();
        }

        [Fact]
        public async Task Given_ValidCreatePromotionInput_When_Handle_Then_ShouldReturnOutput()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            Setup();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act
            var result = await sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().NotBeEmpty();
            result.GameId.Should().Be(input.GameId);
            result.Discount.Should().Be(input.DiscountPercentage);
            result.StartDate.Should().Be(input.StartDate);
            result.EndDate.Should().Be(input.EndDate);

            ReadOnlyGameRepositoryBuilder.VerifyExistsAsyncWasCalled();
            ReadOnlyPromotionRepositoryBuilder.VerifyExistsActivePromotionForGameAsyncWasCalled();
            WriteOnlyPromotionRepositoryBuilder.VerifyAddAsyncWasCalled();
            UnitOfWorkBuilder.VerifySaveChangesAsyncWasCalled();
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithNonExistentGame_When_Handle_Then_ShouldThrowDomainException()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            SetupWithNonExistentGame();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(input, CancellationToken.None));

            exception.Should().NotBeNull();
            exception.Message.Should().Contain("Game not found");
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithActivePromotion_When_Handle_Then_ShouldThrowDomainException()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            SetupWithActivePromotion();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<DomainException>(() => sut.Handle(input, CancellationToken.None));

            exception.Should().NotBeNull();
            exception.Message.Should().Contain("An active promotion already exists");
        }

        [Fact]
        public async Task Given_ValidCreatePromotionInput_When_Handle_Then_ShouldAddPromotionToRepository()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            Setup();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act
            await sut.Handle(input, CancellationToken.None);

            // Assert
            WriteOnlyPromotionRepositoryBuilder.VerifyAddAsyncWasCalled();
        }

        [Fact]
        public async Task Given_ValidCreatePromotionInput_When_Handle_Then_ShouldSaveChanges()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            Setup();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act
            await sut.Handle(input, CancellationToken.None);

            // Assert
            UnitOfWorkBuilder.VerifySaveChangesAsyncWasCalled();
        }

        [Fact]
        public async Task Given_ValidCreatePromotionInput_When_Handle_Then_ShouldCheckIfGameExists()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            Setup();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act
            await sut.Handle(input, CancellationToken.None);

            // Assert
            ReadOnlyGameRepositoryBuilder.VerifyExistsAsyncWasCalled();
        }

        [Fact]
        public async Task Given_ValidCreatePromotionInput_When_Handle_Then_ShouldCheckForActivePromotion()
        {
            // Arrange
            var input = CreatePromotionInputBuilder.Build();
            Setup();

            var sut = new CreatePromotionUseCase(
                ReadOnlyGameRepositoryBuilder.Build(),
                ReadOnlyPromotionRepositoryBuilder.Build(),
                WriteOnlyPromotionRepositoryBuilder.Build(),
                UnitOfWorkBuilder.Build());

            // Act
            await sut.Handle(input, CancellationToken.None);

            // Assert
            ReadOnlyPromotionRepositoryBuilder.VerifyExistsActivePromotionForGameAsyncWasCalled();
        }

        private static void Setup()
        {
            ReadOnlyGameRepositoryBuilder.SetupExistsAsync(true);
            ReadOnlyPromotionRepositoryBuilder.SetupExistsActivePromotionForGameAsync(false);
            WriteOnlyPromotionRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
        }

        private static void SetupWithNonExistentGame()
        {
            ReadOnlyGameRepositoryBuilder.SetupExistsAsync(false);
            ReadOnlyPromotionRepositoryBuilder.SetupExistsActivePromotionForGameAsync(false);
            WriteOnlyPromotionRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
        }

        private static void SetupWithActivePromotion()
        {
            ReadOnlyGameRepositoryBuilder.SetupExistsAsync(true);
            ReadOnlyPromotionRepositoryBuilder.SetupExistsActivePromotionForGameAsync(true);
            WriteOnlyPromotionRepositoryBuilder.SetupAddAsync();
            UnitOfWorkBuilder.SetupSaveChangesAsync();
        }
    }
}
