using FCG.Application.UseCases.Authentication.Login;
using FCG.Application.UseCases.Games.GetAll;
using FCG.Application.UseCases.Games.Register;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Inputs.Games.Register;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Models.Pagination;
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
        private const string LoginUrl = "/api/v1/auth/login";

        public GamesControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        #region Register Tests

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

        #endregion

        #region GetAll Tests

        [Fact]
        public async Task Given_AuthenticatedUser_When_GetAllIsCalled_ShouldReturnOkWithPagedGames()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            await CreateMultipleGamesForGetAll(15);
            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?PageNumber=1&PageSize=10", jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedListResponse<GetAllGamesOutput>>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Items.Should().HaveCountGreaterOrEqualTo(10);
            apiResponse.Data.TotalCount.Should().BeGreaterOrEqualTo(15);
            apiResponse.Data.CurrentPage.Should().Be(1);
            apiResponse.Data.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Given_FilterByName_When_GetAllIsCalled_ShouldReturnFilteredGames()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = await GetAdminAccessToken(adminUser);

            await CreateGameWithName("The Witcher 3", adminToken);
            await CreateGameWithName("Witcher 2", adminToken);
            await CreateGameWithName("Cyberpunk 2077", adminToken);

            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?Name=Witcher&PageNumber=1&PageSize=10", jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedListResponse<GetAllGamesOutput>>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Items.Should().HaveCountGreaterOrEqualTo(2);
            apiResponse.Data.Items.Should().OnlyContain(g => g.Name.Contains("Witcher"));
        }

        [Fact]
        public async Task Given_FilterByCategory_When_GetAllIsCalled_ShouldReturnFilteredGames()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = await GetAdminAccessToken(adminUser);

            await CreateGameWithCategory("Dark Souls", GameCategory.RPG, adminToken);
            await CreateGameWithCategory("Skyrim", GameCategory.RPG, adminToken);
            await CreateGameWithCategory("Call of Duty", GameCategory.Action, adminToken);

            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?Category=RPG&PageNumber=1&PageSize=10", jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedListResponse<GetAllGamesOutput>>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Items.Should().HaveCountGreaterOrEqualTo(2);
        }

        [Fact]
        public async Task Given_FilterByPriceRange_When_GetAllIsCalled_ShouldReturnFilteredGames()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = await GetAdminAccessToken(adminUser);

            await CreateGameWithPrice("Budget Game", 9.99m, adminToken);
            await CreateGameWithPrice("Mid Game", 29.99m, adminToken);
            await CreateGameWithPrice("Premium Game", 69.99m, adminToken);

            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?MinPrice=20&MaxPrice=50&PageNumber=1&PageSize=10", jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedListResponse<GetAllGamesOutput>>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Items.Should().HaveCountGreaterOrEqualTo(1);
            apiResponse.Data.Items.Should().OnlyContain(g => g.Price >= 20m && g.Price <= 50m);
        }

        [Fact]
        public async Task Given_MultipleFilters_When_GetAllIsCalled_ShouldReturnFilteredGames()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = await GetAdminAccessToken(adminUser);

            await CreateGameWithDetails("Elden Ring", GameCategory.RPG, 59.99m, adminToken);
            await CreateGameWithDetails("Dark Souls RPG", GameCategory.RPG, 39.99m, adminToken);
            await CreateGameWithDetails("Bloodborne", GameCategory.Action, 29.99m, adminToken);

            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet(
                $"{ValidUrl}?Name=Dark&Category=RPG&MinPrice=30&MaxPrice=50&PageNumber=1&PageSize=10",
                jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedListResponse<GetAllGamesOutput>>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Items.Should().HaveCountGreaterOrEqualTo(1);
            apiResponse.Data.Items.Should().OnlyContain(g =>
                g.Name.Contains("Dark") &&
                g.Category == "RPG" &&
                g.Price >= 30m &&
                g.Price <= 50m);
        }

        [Fact]
        public async Task Given_SecondPage_When_GetAllIsCalled_ShouldReturnCorrectPagedData()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            await CreateMultipleGamesForGetAll(25);
            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?PageNumber=2&PageSize=10", jwtToken);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<PagedListResponse<GetAllGamesOutput>>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.CurrentPage.Should().Be(2);
            apiResponse.Data.Items.Should().HaveCountGreaterOrEqualTo(10);
        }

        [Fact]
        public async Task Given_UnauthenticatedUser_When_GetAllIsCalled_ShouldReturnUnauthorized()
        {
            // Arrange
            ClearAuthentication();

            // Act
            var result = await DoGet($"{ValidUrl}?PageNumber=1&PageSize=10");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_InvalidPageNumber_When_GetAllIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?PageNumber=0&PageSize=10", jwtToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_InvalidPageSize_When_GetAllIsCalled_ShouldReturnBadRequest()
        {
            // Arrange
            var regularUser = Factory.CreatedUsers.First();
            var jwtToken = await GetRegularUserAccessToken(regularUser);

            // Act
            var result = await DoAuthenticatedGet($"{ValidUrl}?PageNumber=1&PageSize=0", jwtToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Helpers

        private async Task<string> GetAdminAccessToken(User adminUser)
        {
            var loginInput = LoginInputBuilder.BuildWithValues(adminUser.Email.Value, adminUser.Password);
            Setup(adminUser);

            var loginResult = await DoPost(LoginUrl, loginInput);
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

            var loginResult = await DoPost(LoginUrl, loginInput);
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

        private async Task CreateMultipleGamesForGetAll(int count)
        {
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = await GetAdminAccessToken(adminUser);

            for (int i = 0; i < count; i++)
            {
                var registerGameInput = RegisterGameInputBuilder.BuildWithName($"Game {Guid.NewGuid()} {i + 1}");
                await DoAuthenticatedPost(ValidUrl, registerGameInput, adminToken);
            }
        }

        private async Task CreateGameWithName(string name, string adminToken)
        {
            var registerGameInput = RegisterGameInputBuilder.BuildWithName(name);
            await DoAuthenticatedPost(ValidUrl, registerGameInput, adminToken);
        }

        private async Task CreateGameWithCategory(string name, GameCategory category, string adminToken)
        {
            var registerGameInput = RegisterGameInputBuilder.BuildWithNameAndCategory(name, category);
            await DoAuthenticatedPost(ValidUrl, registerGameInput, adminToken);
        }

        private async Task CreateGameWithPrice(string name, decimal price, string adminToken)
        {
            var registerGameInput = RegisterGameInputBuilder.BuildWithNameAndPrice(name, price);
            await DoAuthenticatedPost(ValidUrl, registerGameInput, adminToken);
        }

        private async Task CreateGameWithDetails(string name, GameCategory category, decimal price, string adminToken)
        {
            var registerGameInput = RegisterGameInputBuilder.BuildWithDetails(name, category, price);
            await DoAuthenticatedPost(ValidUrl, registerGameInput, adminToken);
        }

        #endregion
    }
}