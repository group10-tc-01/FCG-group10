using FCG.Application.UseCases.Authentication.Login;
using FCG.Application.UseCases.Promotions.Create;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Inputs.Games.Register;
using FCG.CommomTestsUtilities.Builders.Inputs.Promotions.Create;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations;
using FCG.WebApi.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class PromotionsControllerTest : FcgFixture
    {
        private readonly CustomWebApplicationFactory _factory;
        private const string ValidUrl = "/api/v1/promotions";

        public PromotionsControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Given_ValidCreatePromotionInputWithAdminUser_When_PostIsCalled_ShouldReturnCreated()
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var game = await CreateGame(adminUser);
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithGameId(game.Id);
            var jwtToken = await GetAdminAccessToken(adminUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreatePromotionResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.Created);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Id.Should().NotBeEmpty();
            apiResponse.Data.GameId.Should().Be(game.Id);
            apiResponse.Data.Discount.Should().Be(createPromotionRequest.DiscountPercentage);
            apiResponse.Data.StartDate.Should().Be(createPromotionRequest.StartDate);
            apiResponse.Data.EndDate.Should().Be(createPromotionRequest.EndDate);

            await VerifyPromotionExistsInDatabase(game.Id);
        }

        [Fact]
        public async Task Given_InvalidCreatePromotionInputWithEmptyGameId_When_PostIsCalled_ShouldReturnBadRequest()
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithEmptyGameId();
            var jwtToken = await GetAdminAccessToken(adminUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreatePromotionResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain("Game ID is required.");
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithRegularUser_When_PostIsCalled_ShouldReturnForbidden()
        {
            var regularUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var game = await CreateGame(adminUser);
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithGameId(game.Id);
            var jwtToken = await GetRegularUserAccessToken(regularUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);

            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithoutAuthentication_When_PostIsCalled_ShouldReturnBadRequestOrUnauthorized()
        {
            var createPromotionRequest = CreatePromotionInputBuilder.Build();
            ClearAuthentication();

            var result = await DoPost(ValidUrl, createPromotionRequest);

            result.StatusCode.Should().Match(x =>
                x == HttpStatusCode.Unauthorized || x == HttpStatusCode.BadRequest,
                "because validation may run before authentication");
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithNonExistentGame_When_PostIsCalled_ShouldReturnBadRequest()
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithGameId(Guid.NewGuid());
            var jwtToken = await GetAdminAccessToken(adminUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreatePromotionResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain(e => e.Contains("Game not found"));
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithInvalidDiscount_When_PostIsCalled_ShouldReturnBadRequest()
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var game = await CreateGame(adminUser);
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithGameId(game.Id);
            createPromotionRequest.DiscountPercentage = 150;
            var jwtToken = await GetAdminAccessToken(adminUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreatePromotionResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain("Discount percentage must be less than or equal to 100.");
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithPastStartDate_When_PostIsCalled_ShouldReturnBadRequest()
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var game = await CreateGame(adminUser);
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithPastStartDate();
            createPromotionRequest.GameId = game.Id;
            var jwtToken = await GetAdminAccessToken(adminUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreatePromotionResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain("Start date cannot be in the past.");
        }

        [Fact]
        public async Task Given_CreatePromotionInputWithEndDateBeforeStartDate_When_PostIsCalled_ShouldReturnBadRequest()
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var game = await CreateGame(adminUser);
            var createPromotionRequest = CreatePromotionInputBuilder.BuildWithEndDateBeforeStartDate();
            createPromotionRequest.GameId = game.Id;
            var jwtToken = await GetAdminAccessToken(adminUser);

            var result = await DoAuthenticatedPost(ValidUrl, createPromotionRequest, jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreatePromotionResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeFalse();
            apiResponse.Data.Should().BeNull();
            apiResponse.ErrorMessages.Should().Contain("End date must be after the start date.");
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

        private async Task<Game> CreateGame(User adminUser)
        {
            var registerGameInput = RegisterGameInputBuilder.Build();
            var jwtToken = await GetAdminAccessToken(adminUser);

            await DoAuthenticatedPost("/api/v1/games", registerGameInput, jwtToken);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();
            var game = await dbContext.Games.FirstOrDefaultAsync(g => g.Name.Value == registerGameInput.Name);

            return game ?? throw new InvalidOperationException("Failed to create game");
        }

        private async Task VerifyPromotionExistsInDatabase(Guid gameId)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var promotionInDb = await dbContext.Promotions.FirstOrDefaultAsync(p => p.GameId == gameId);
            promotionInDb.Should().NotBeNull("promotion should be saved in database");
        }
    }
}
