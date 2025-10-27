using FCG.Application.UseCases.AdminUsers.GetById;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories.UserRepository;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations;
using FCG.WebApi.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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
            // Given
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users");

            // Then
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
            // Given
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users?pageNumber=1&pageSize=1");

            // Then
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
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
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
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
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
            var userToken = GenerateToken(Guid.NewGuid(), "User");
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
            // Given
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "invalid.token.here");

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
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
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
            var userToken = GenerateToken(Guid.NewGuid(), "User"); // Token de usuário comum
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
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/admin/users/invalid-guid");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task GET_List_WhenInternalErrorOccurs_ShouldReturn500InternalServerError()
        {
            var factory = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var mockRepo = new Mock<IReadOnlyUserRepository>();

                    mockRepo.Setup(repo => repo.GetQueryableAllUsers(
                            It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new Exception("Simulação de falha de serviço não mapeada."));

                    services.RemoveAll<IReadOnlyUserRepository>();
                    services.AddScoped<IReadOnlyUserRepository>(sp => mockRepo.Object);
                });
            });

            using var client = factory.CreateClient();

            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            // ACT (WHEN)
            var response = await client.GetAsync("/api/v1/admin/users");

            // ASSERT (THEN)
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task GET_UserById_GivenValidIdAndAdminToken_ShouldReturn200AndUserData()
        {
            var expectedEmail = $"search.{Guid.NewGuid()}@adminsearch.com";

            var userToFind = await AddUserToDatabaseAsync(expectedEmail, "PasswordSearch!1");

            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");

            var url = $"/api/v1/admin/users/{userToFind.Id}";

            // ACT (WHEN)
            var response = await DoAuthenticatedGet(url, adminToken);

            // ASSERT (THEN)
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserDetailResponse>>();

            apiResponse.Should().NotBeNull("a resposta da API não deve ser nula.");
            apiResponse!.Data.Id.Should().Be(userToFind.Id);
            apiResponse.Data.Email.Should().Be(expectedEmail, "O email deve bater com o usuário criado.");
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
            result.Should().BeFalse("o banco de dados não deve conter nenhum administrador.");
        }

        [Fact]
        public async Task GET_List_WhenDomainExceptionOccurs_ShouldReturn400BadRequest()
        {
            // ARRANGE (GIVEN)
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");

            var domainExceptionMessage = "Erro de filtro no domínio. (Teste de cobertura)";

            var factory = Factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var mockRepo = new Mock<IReadOnlyUserRepository>();

                    mockRepo.Setup(repo => repo.GetQueryableAllUsers(
                        It.IsAny<string?>(), It.IsAny<string?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                        .ThrowsAsync(new DomainException(domainExceptionMessage));

                    services.RemoveAll<IReadOnlyUserRepository>();
                    services.AddScoped<IReadOnlyUserRepository>(sp => mockRepo.Object);
                });
            });

            using var client = factory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

            // ACT (WHEN)
            var response = await client.GetAsync("/api/v1/admin/users");

            // ASSERT (THEN)
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeFalse();

            apiResponse.ErrorMessages.Should().Contain(e => e.ToString()!.Contains(domainExceptionMessage));
        }

    }
}