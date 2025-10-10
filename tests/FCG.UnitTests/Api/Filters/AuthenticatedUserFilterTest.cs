using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.Domain.Entities;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Services;
using FCG.Messages;
using FCG.WebApi.Filter;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Moq;

namespace FCG.UnitTests.Api.Filters
{
    public class AuthenticatedUserFilterTest
    {
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IReadOnlyUserRepository> _readOnlyUserRepositoryMock;
        private readonly AuthenticatedUserFilter _sut;

        public AuthenticatedUserFilterTest()
        {
            _tokenServiceMock = new Mock<ITokenService>();
            _readOnlyUserRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _sut = new AuthenticatedUserFilter(_tokenServiceMock.Object, _readOnlyUserRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_ValidTokenAndExistingUser_When_OnAuthorizationAsync_Then_ShouldNotThrow()
        {
            // Arrange
            var context = CreateAuthorizationFilterContext("Bearer valid-token");
            var userId = Guid.NewGuid();
            var user = UserBuilder.Build();

            _tokenServiceMock.Setup(x => x.ValidateAccessToken("valid-token")).Returns(userId);
            _readOnlyUserRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().NotThrowAsync();
            _tokenServiceMock.Verify(x => x.ValidateAccessToken("valid-token"), Times.Once);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Given_ValidTokenButUserNotFound_When_OnAuthorizationAsync_Then_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var context = CreateAuthorizationFilterContext("Bearer valid-token");
            var userId = Guid.NewGuid();

            _tokenServiceMock.Setup(x => x.ValidateAccessToken("valid-token")).Returns(userId);
            _readOnlyUserRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User)null!);

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidToken);
            _tokenServiceMock.Verify(x => x.ValidateAccessToken("valid-token"), Times.Once);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        }

        [Fact]
        public async Task Given_InvalidToken_When_OnAuthorizationAsync_Then_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var context = CreateAuthorizationFilterContext("Bearer invalid-token");

            _tokenServiceMock.Setup(x => x.ValidateAccessToken("invalid-token"))
                .Throws(new UnauthorizedException(ResourceMessages.InvalidToken));

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidToken);
            _tokenServiceMock.Verify(x => x.ValidateAccessToken("invalid-token"), Times.Once);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task Given_EmptyOrNullAuthorizationHeader_When_OnAuthorizationAsync_Then_ShouldThrowUnauthorizedException(string? authorizationHeader)
        {
            // Arrange
            var context = CreateAuthorizationFilterContext(authorizationHeader);

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidToken);
            _tokenServiceMock.Verify(x => x.ValidateAccessToken(It.IsAny<string>()), Times.Never);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Given_AuthorizationHeaderWithoutBearer_When_OnAuthorizationAsync_Then_ShouldThrowUnauthorizedException()
        {
            // Arrange
            var context = CreateAuthorizationFilterContext("some-token-without-bearer");

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidToken);
            _tokenServiceMock.Verify(x => x.ValidateAccessToken(It.IsAny<string>()), Times.Never);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Given_BearerTokenWithExtraSpaces_When_OnAuthorizationAsync_Then_ShouldTrimAndValidate()
        {
            // Arrange
            var context = CreateAuthorizationFilterContext("Bearer   token-with-spaces   ");
            var userId = Guid.NewGuid();
            var user = UserBuilder.Build();

            _tokenServiceMock.Setup(x => x.ValidateAccessToken("token-with-spaces")).Returns(userId);
            _readOnlyUserRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().NotThrowAsync();
            _tokenServiceMock.Verify(x => x.ValidateAccessToken("token-with-spaces"), Times.Once);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        }

        [Theory]
        [InlineData("Basic token")]
        [InlineData("Token value")]
        public async Task Given_NonBearerTokenFormat_When_OnAuthorizationAsync_Then_ShouldThrowUnauthorizedException(string authorizationHeader)
        {
            // Arrange
            var context = CreateAuthorizationFilterContext(authorizationHeader);

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedException>().WithMessage(ResourceMessages.InvalidToken);
            _tokenServiceMock.Verify(x => x.ValidateAccessToken(It.IsAny<string>()), Times.Never);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task Given_BearerWithCaseInsensitive_When_OnAuthorizationAsync_Then_ShouldAcceptAndValidate()
        {
            // Arrange
            var context = CreateAuthorizationFilterContext("BEARER valid-token");
            var userId = Guid.NewGuid();
            var user = UserBuilder.Build();

            _tokenServiceMock.Setup(x => x.ValidateAccessToken("valid-token")).Returns(userId);
            _readOnlyUserRepositoryMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var act = async () => await _sut.OnAuthorizationAsync(context);

            // Assert
            await act.Should().NotThrowAsync();
            _tokenServiceMock.Verify(x => x.ValidateAccessToken("valid-token"), Times.Once);
            _readOnlyUserRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
        }

        private static AuthorizationFilterContext CreateAuthorizationFilterContext(string? authorizationHeader)
        {
            var httpContext = new DefaultHttpContext();

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                httpContext.Request.Headers.Authorization = new StringValues(authorizationHeader);
            }

            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            return new AuthorizationFilterContext(actionContext, new List<IFilterMetadata>());
        }
    }
}