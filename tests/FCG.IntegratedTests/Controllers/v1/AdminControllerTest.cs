using FCG.Application.UseCases.Admin.GetUserById;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations;
using FCG.WebApi.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class AdminControllerIntegrationTests : FcgFixture
    {
        public AdminControllerIntegrationTests(CustomWebApplicationFactory factory)
             : base(factory)
        {
        }

        [Fact]
        public async Task Given_AdminToken_When_GettingUsersList_Then_ShouldReturn200WithPaginatedData()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // Act
            var response = await _httpClient.GetAsync("/api/v1/admin/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
            content.Should().Contain("\"items\":");
            content.Should().Contain("\"totalCount\":");
            content.Should().Contain("\"currentPage\":");
            content.Should().Contain("\"pageSize\":");
        }

        [Fact]
        public async Task Given_AdminToken_When_GettingUsersWithPagination_Then_ShouldReturnCorrectPageSize()
        {
            // Arrange
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // Act
            var response = await _httpClient.GetAsync("/api/v1/admin/users?pageNumber=1&pageSize=1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"pageSize\":1");
            content.Should().Contain("\"currentPage\":1");
        }

        [Fact]
        public async Task Given_AdminToken_When_FilteringByEmail_Then_ShouldReturnFilteredResults()
        {
            // Given
            var existingUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync($"/api/v1/admin/users?emailFilter={existingUser.Email.Value}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain(existingUser.Email.Value);
        }


        [Fact]
        public async Task Given_AdminToken_When_FilteringByRole_Then_ShouldReturnFilteredResults()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users?roleFilter=User");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
        }

        [Fact]
        public async Task Given_CommonUserToken_When_GettingUsersList_Then_ShouldReturn403Forbidden()
        {
            // Given
            var regularUser = Factory.CreatedUsers.First();
            var userToken = GenerateToken(regularUser.Id, "User");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_NoToken_When_GettingUsersList_Then_ShouldReturn401Unauthorized()
        {
            // Given
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_InvalidToken_When_GettingUsersList_Then_ShouldReturn401Unauthorized()
        {
            // Given - Generate a token with wrong secret key
            var invalidConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"JwtSettings:SecretKey", "this-is-an-invalid-secret-key-that-will-fail-validation-12345"},
                    {"JwtSettings:Issuer", "TestIssuer"},
                    {"JwtSettings:Audience", "TestAudience"},
                    {"JwtSettings:AccessTokenExpirationMinutes", "60"}
                }!)
                .Build();
            
            var adminUser = Factory.CreatedAdminUsers.First();
            var invalidToken = TokenServiceBuilder.GenerateToken(invalidConfiguration, adminUser.Id, "Admin");
            
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", invalidToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_AdminTokenAndNonExistentUserId_When_GettingUserDetails_Then_ShouldReturn404NotFound()
        {
            // Given
            var nonExistentId = Guid.NewGuid();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync($"/api/v1/admin/users/{nonExistentId}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Given_NoToken_When_GettingUserDetails_Then_ShouldReturn401Unauthorized()
        {
            // Given
            var existingUser = Factory.CreatedUsers.First();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            // When
            var response = await _httpClient.GetAsync($"/api/v1/admin/users/{existingUser.Id}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


        [Fact]
        public async Task Given_CommonUserToken_When_GettingUserDetails_Then_ShouldReturn403Forbidden()
        {
            // Given
            var existingUser = Factory.CreatedUsers.First();
            var regularUser = Factory.CreatedUsers.First();
            var userToken = GenerateToken(regularUser.Id, "User"); // Token de usu�rio comum
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            // When
            var response = await _httpClient.GetAsync($"/api/v1/admin/users/{existingUser.Id}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_InvalidUserId_When_GettingUserDetails_Then_ShouldReturn400BadRequest()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users/invalid-guid");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GET_UserById_GivenValidIdAndAdminToken_ShouldReturn200AndUserData()
        {
            var expectedEmail = $"search.{Guid.NewGuid()}@adminsearch.com";

            var userToFind = await AddUserToDatabaseAsync(expectedEmail, "PasswordSearch!1");

            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");

            var url = $"/api/v1/admin/users/{userToFind.Id}";

            // Act
            var response = await DoAuthenticatedGet(url, adminToken);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<GetUserByIdResponse>>();

            apiResponse.Should().NotBeNull("a resposta da API n�o deve ser nula.");
            apiResponse!.Data.Id.Should().Be(userToFind.Id);
            apiResponse.Data.Email.Should().Be(expectedEmail, "O email deve bater com o usu�rio criado.");
        }

        [Fact]
        public async Task Given_DatabaseWithOnlyCommonUsers_When_CheckingForAdmins_Then_ShouldReturnFalse()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var users = await dbContext.Users.ToListAsync();
            dbContext.Users.RemoveRange(users);

            var wallets = await dbContext.Wallets.ToListAsync();
            dbContext.Wallets.RemoveRange(wallets);

            var libraries = await dbContext.Libraries.ToListAsync();
            dbContext.Libraries.RemoveRange(libraries);

            await dbContext.SaveChangesAsync();

            var userRepository = scope.ServiceProvider.GetRequiredService<IReadOnlyUserRepository>();

            // When
            var result = await userRepository.AnyAdminAsync();

            // Then
            result.Should().BeFalse("o banco de dados n�o deve conter nenhum administrador.");
        }

        [Fact]
        public async Task Given_AdminToken_When_CreatingUserWithValidData_Then_ShouldReturn201Created()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Name = "New Test User",
                Email = "newuser@test.com",
                Password = "ValidP@ssw0rd!123",
                Role = 1 // User role
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
            content.Should().Contain("New Test User");
            content.Should().Contain("newuser@test.com");
        }

        [Fact]
        public async Task Given_AdminToken_When_CreatingAdminUser_Then_ShouldReturn201WithAdminRole()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Name = "New Admin User",
                Email = "newadmin@test.com",
                Password = "AdminP@ssw0rd!123",
                Role = 0 // Admin role
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
            content.Should().Contain("New Admin User");
            content.Should().Contain("\"role\":0");
        }

        [Fact]
        public async Task Given_NoToken_When_CreatingUser_Then_ShouldReturn401Unauthorized()
        {
            // Given
            var request = new
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "ValidP@ssw0rd!123",
                Role = 1
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_RegularUserToken_When_CreatingUser_Then_ShouldReturn403Forbidden()
        {
            // Given
            var regularUser = Factory.CreatedUsers.First();
            var userToken = GenerateToken(regularUser.Id, "User");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            var request = new
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "ValidP@ssw0rd!123",
                Role = 1
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_AdminToken_When_CreatingUserWithDuplicateEmail_Then_ShouldReturn400BadRequest()
        {
            // Given
            var existingUser = Factory.CreatedUsers.First();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Name = "Duplicate Email User",
                Email = existingUser.Email.Value,
                Password = "ValidP@ssw0rd!123",
                Role = 1
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_AdminToken_When_CreatingUserWithInvalidEmail_Then_ShouldReturn400BadRequest()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Name = "Test User",
                Email = "invalid-email",
                Password = "ValidP@ssw0rd!123",
                Role = 1
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_AdminToken_When_CreatingUserWithEmptyName_Then_ShouldReturn400BadRequest()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Name = "",
                Email = "test@test.com",
                Password = "ValidP@ssw0rd!123",
                Role = 1
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_AdminToken_When_CreatingUserWithWeakPassword_Then_ShouldReturn400BadRequest()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "weak",
                Role = 1
            };

            // When
            var response = await _httpClient.PostAsJsonAsync("/api/v1/admin/users", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_AdminToken_When_DepositingToWallet_Then_ShouldReturn200OK()
        {
            // Given
            var user = Factory.CreatedUsers.First();
            var walletId = user.Wallet!.Id;
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Amount = 100.00m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{user.Id}/wallet/{walletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
            content.Should().Contain("\"depositedAmount\":100");
        }

        [Fact]
        public async Task Given_NoToken_When_DepositingToWallet_Then_ShouldReturn401Unauthorized()
        {
            // Given
            var userId = Guid.NewGuid();
            var walletId = Guid.NewGuid();
            var request = new
            {
                Amount = 100.00m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{userId}/wallet/{walletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_RegularUserToken_When_DepositingToWallet_Then_ShouldReturn403Forbidden()
        {
            // Given
            var regularUser = Factory.CreatedUsers.First();
            var walletId = regularUser.Wallet!.Id;
            var userToken = GenerateToken(regularUser.Id, "User");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            var request = new
            {
                Amount = 100.00m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{regularUser.Id}/wallet/{walletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_AdminToken_When_DepositingWithInvalidAmount_Then_ShouldReturn400BadRequest()
        {
            // Given
            var user = Factory.CreatedUsers.First();
            var walletId = user.Wallet!.Id;
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Amount = -50.00m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{user.Id}/wallet/{walletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_AdminToken_When_DepositingToNonExistentWallet_Then_ShouldReturn404NotFound()
        {
            // Given
            var nonExistentUserId = Guid.NewGuid();
            var nonExistentWalletId = Guid.NewGuid();
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Amount = 100.00m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{nonExistentUserId}/wallet/{nonExistentWalletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Given_AdminToken_When_DepositingZeroAmount_Then_ShouldReturn400BadRequest()
        {
            // Given
            var user = Factory.CreatedUsers.First();
            var walletId = user.Wallet!.Id;
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Amount = 0m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{user.Id}/wallet/{walletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Given_AdminToken_When_DepositingToWalletThatDoesNotBelongToUser_Then_ShouldReturn400BadRequest()
        {
            // Given
            var user1 = Factory.CreatedUsers.First();
            var user2 = Factory.CreatedUsers.Last();
            var user2WalletId = user2.Wallet!.Id; // Wallet from different user
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            var request = new
            {
                Amount = 100.00m
            };

            // When
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/admin/users/{user1.Id}/wallet/{user2WalletId}/deposit", request);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

    }
}
