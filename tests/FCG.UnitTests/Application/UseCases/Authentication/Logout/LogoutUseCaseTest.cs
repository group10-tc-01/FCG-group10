using FCG.Application.UseCases.Authentication.Logout;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Logout;
using FCG.CommomTestsUtilities.Builders.Repositories.RefreshTokenRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Repositories.RefreshTokenRepository;
using FCG.Messages;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Authentication.Logout
{
    public class LogoutUseCaseTest
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogoutUseCase _sut;

        public LogoutUseCaseTest()
        {
            _refreshTokenRepository = RefreshTokenRepositoryBuilder.Build();
            var logger = new Mock<ILogger<LogoutUseCase>>().Object;
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _sut = new LogoutUseCase(_refreshTokenRepository, logger, correlationIdProvider);
        }

        [Fact]
        public async Task Given_ValidLogoutInput_When_Handle_Then_ShouldReturnSuccessOutput()
        {
            // Arrange
            var input = LogoutInputBuilder.Build();
            Setup();

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(ResourceMessages.LogoutSuccessFull);
        }

        [Fact]
        public async Task Given_ValidLogoutInput_When_Handle_Then_ShouldCallRevokeAllByUserIdAsync()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var input = LogoutInputBuilder.BuildWithUserId(userId);
            Setup();

            // Act
            await _sut.Handle(input, CancellationToken.None);

            // Assert
            RefreshTokenRepositoryBuilder.VerifyRevokeAllByUserIdAsyncWasCalledWith(userId);
        }

        [Fact]
        public async Task Given_EmptyUserId_When_Handle_Then_ShouldStillReturnSuccessOutput()
        {
            // Arrange
            var input = LogoutInputBuilder.BuildWithEmptyUserId();
            Setup();

            // Act
            var result = await _sut.Handle(input, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Message.Should().Be(ResourceMessages.LogoutSuccessFull);
        }

        private static void Setup()
        {
            RefreshTokenRepositoryBuilder.SetupRevokeAllByUserIdAsync();
        }
    }
}