using FCG.Application.UseCases.Authentication.Login;
using FCG.Application.UseCases.Games.Register;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Inputs.Games.Register;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations;
using FCG.Messages;
using FCG.WebApi.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class GamesControllerTest : FcgFixture
    {
        private readonly CustomWebApplicationFactory _factory;
        private const string ValidUrl = "/api/v1/games";

        public GamesControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Given_ValidRegisterGameInputWithAdminUser_When_PostIsCalled_ShouldReturnCreated()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var registerGameInput = RegisterGameInputBuilder.Build();
            var jwtToken = await GetAdminAccessToken(adminUser);

            // Act
            var result = await DoAuthenticatedPost(ValidUrl, registerGameInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RegisterGameOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Created);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Id.Should().NotBeEmpty();
            apiResponse.Data.Name.Should().Be(registerGameInput.Name);

            await VerifyGameExistsInDatabase(registerGameInput.Name);
        }

        [Fact]
        public async Task Given_InvalidRegisterGameInput_When_PostIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var registerGameInput = RegisterGameInputBuilder.BuildWithEmptyName();
            var jwtToken = await GetAdminAccessToken(adminUser);

            // Act
            var result = await DoAuthenticatedPost(ValidUrl, registerGameInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RegisterGameOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(ResourceMessages.GameNameIsRequired);
        }

        [Fact]
        public async Task Given_RegisterGameInputWithRegularUser_When_PostIsCalled_ShouldReturnForbidden()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var registerGameInput = RegisterGameInputBuilder.Build();
            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedPost(ValidUrl, registerGameInput, jwtToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_RegisterGameInputWithoutAuthentication_When_PostIsCalled_ShouldReturnUnauthorized()
        {
            // Arrange
            var registerGameInput = RegisterGameInputBuilder.Build();
            ClearAuthentication();

            // Act
            var result = await DoPost(ValidUrl, registerGameInput);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_DuplicateGameName_When_PostIsCalled_ShouldReturnConflict()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var registerGameInput = RegisterGameInputBuilder.Build();
            var jwtToken = await GetAdminAccessToken(adminUser);

            await DoAuthenticatedPost(ValidUrl, registerGameInput, jwtToken);

            var duplicateGameInput = RegisterGameInputBuilder.BuildWithName(registerGameInput.Name);

            // Act
            var result = await DoAuthenticatedPost(ValidUrl, duplicateGameInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RegisterGameOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(message => message.Contains(registerGameInput.Name));
        }

        [Fact]
        public async Task Given_RegisterGameInputWithZeroPrice_When_PostIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var registerGameInput = RegisterGameInputBuilder.BuildWithZeroPrice();
            var jwtToken = await GetAdminAccessToken(adminUser);

            // Act
            var result = await DoAuthenticatedPost(ValidUrl, registerGameInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RegisterGameOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(ResourceMessages.GamePriceMustBeGreaterThanZero);
        }

        [Fact]
        public async Task Given_RegisterGameInputWithLongName_When_PostIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var registerGameInput = RegisterGameInputBuilder.BuildWithLongName();
            var jwtToken = await GetAdminAccessToken(adminUser);

            // Act
            var result = await DoAuthenticatedPost(ValidUrl, registerGameInput, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RegisterGameOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(ResourceMessages.GameNameMaxLength);
        }

        private async Task<string> GetAdminAccessToken(User adminUser)
        {
            var loginInput = LoginInputBuilder.BuildWithValues(adminUser.Email.Value, adminUser.Password);
            Setup(adminUser);

            var loginResult = await DoPost("/api/v1/auth/login", loginInput);
            var loginResponseContent = await loginResult.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return loginApiResponse?.Data.AccessToken ?? throw new InvalidOperationException("Failed to get access token for admin user");
        }

        private async Task<string> GetRegularUserAccessToken(User regularUser)
        {
            var loginInput = LoginInputBuilder.BuildWithValues(regularUser.Email.Value, regularUser.Password);
            Setup(regularUser);

            var loginResult = await DoPost("/api/v1/auth/login", loginInput);
            var loginResponseContent = await loginResult.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return loginApiResponse?.Data.AccessToken ?? throw new InvalidOperationException("Failed to get access token for regular user");
        }

        private static void Setup(User user)
        {
            PasswordEncrypterServiceBuilder.Build();
            PasswordEncrypterServiceBuilder.SetupEncrypt(user.Password);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
        }

        private async Task VerifyGameExistsInDatabase(string gameName)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var gameInDb = await dbContext.Games.FirstOrDefaultAsync(g => g.Name.Value == gameName);
            gameInDb.Should().NotBeNull("game should be saved in database");
        }
    }
}