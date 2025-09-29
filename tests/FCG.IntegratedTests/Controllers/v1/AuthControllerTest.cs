using FCG.Application.UseCases.Authentication.Login;
using FCG.Application.UseCases.Authentication.Logout;
using FCG.Application.UseCases.Authentication.RefreshToken;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.IntegratedTests.Configurations;
using FCG.Messages;
using FCG.WebApi.Models;
using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class AuthControllerTest : FcgFixture
    {
        public AuthControllerTest(CustomWebApplicationFactory factory) : base(factory) { }

        #region Login

        [Fact]
        public async Task Given_ValidLoginInput_When_PostIsCalled_ShouldReturnOk()
        {
            // Arrange
            var validUrl = "/api/v1/auth/login";
            var createdUser = Factory.CreatedUsers.First();
            var loginInput = LoginInputBuilder.BuildWithValues(createdUser.Email, createdUser.Password);

            // Act
            var result = await DoPost(validUrl, loginInput);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.AccessToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.RefreshToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.ExpiresInMinutes.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Given_InvalidLoginInput_When_PostIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var validUrl = "/api/v1/auth/login";
            var loginInput = LoginInputBuilder.BuildWithValues("invalid_email", "passwordtest");

            // Act
            var result = await DoPost(validUrl, loginInput);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(ResourceMessages.LoginInvalidEmailFormat);
        }

        #endregion

        #region Logout

        [Fact]
        public async Task Given_ValidLogout_When_PostIsCalled_Then_ShouldReturnOk()
        {
            // Arrange
            var validUrl = "/api/v1/auth/logout";
            var createdUser = Factory.CreatedUsers.First();
            var loginInput = LoginInputBuilder.BuildWithValues(createdUser.Email, createdUser.Password);
            var loginResult = await DoPost("/api/v1/auth/login", loginInput);
            var loginResponseContent = await loginResult.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var jwtToken = loginApiResponse?.Data.AccessToken!;
            var createdRefreshToken = Factory.CreatedRefreshTokens.First();

            // Act
            var result = await DoAuthenticatedPostWithoutContent(validUrl, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<LogoutOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Message.Should().Be(ResourceMessages.LogoutSuccessFull);
        }

        #endregion

        #region RefreshToken

        [Fact]
        public async Task Given_ValidRefreshTokenInput_When_PostIsCalled_ShouldReturnOk()
        {
            // Arrange
            var validUrl = "/api/v1/auth/refresh-token";
            var createdUser = Factory.CreatedUsers.First();
            var loginInput = LoginInputBuilder.BuildWithValues(createdUser.Email, createdUser.Password);
            var loginResult = await DoPost("/api/v1/auth/login", loginInput);
            var loginResponseContent = await loginResult.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var jwtToken = loginApiResponse?.Data.AccessToken!;
            var createdRefreshToken = Factory.CreatedRefreshTokens.First();
            var refreshTokenInput = new RefreshTokenInput { RefreshToken = createdRefreshToken.Token };

            // Act
            var result = await DoAuthenticatedPost(validUrl, refreshTokenInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RefreshTokenOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.AccessToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.RefreshToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.ExpiresInDays.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Given_InvalidRefreshTokenInput_When_PostIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var validUrl = "/api/v1/auth/refresh-token";
            var createdUser = Factory.CreatedUsers.First();
            var loginInput = LoginInputBuilder.BuildWithValues(createdUser.Email, createdUser.Password);
            var loginResult = await DoPost("/api/v1/auth/login", loginInput);
            var loginResponseContent = await loginResult.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var jwtToken = loginApiResponse?.Data.AccessToken!;
            var invalidRefreshToken = "invalid-refresh-token";
            var refreshTokenInput = new RefreshTokenInput { RefreshToken = invalidRefreshToken };

            // Act
            var result = await DoAuthenticatedPost(validUrl, refreshTokenInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RefreshTokenOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(ResourceMessages.InvalidRefreshToken);
        }

        #endregion
    }
}
