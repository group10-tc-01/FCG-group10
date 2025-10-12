//using FCG.Application.UseCases.Users.Update.UsersDTO;
//using FCG.FunctionalTests.Helpers;
//using FCG.WebApi;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;

//namespace FCG.IntegratedTests.Controllers.v1
//{
//    public class AdminControllerTest : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly HttpClient _client;
//        private readonly IConfiguration _configuration;

//        public AdminControllerTest(WebApplicationFactory<Program> factory)
//        {
//            _client = factory.CreateClient();
//            _configuration = factory.Services.GetRequiredService<IConfiguration>();
//        }
//        private string GenerateToken(Guid userId, string role)
//        {
//            return TestTokenGenerator.GenerateToken(_configuration, userId, role);
//        }
//[Fact]
//public async Task Given_CommonUserToken_When_GettingList_Then_ShouldReturn403Forbidden()
//{
//    var commonUserToken = GenerateToken(Guid.NewGuid(), "User");
//    _client.DefaultRequestHeaders.Authorization =
//        new AuthenticationHeaderValue("Bearer", commonUserToken);

//    var response = await _client.GetAsync("/api/v1/admin/users");

//    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
//}

//[Fact]
//public async Task Given_AdminToken_When_GettingNonExistentId_Then_ShouldReturn404NotFound()
//{
//    // ARRANGE
//    var nonExistentId = Guid.NewGuid();

//    var adminToken = TestTokenGenerator.GenerateToken(_configuration, Guid.NewGuid(), "Admin");

//    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

//    // ACT (WHEN)
//    var response = await _client.GetAsync($"/api/v1/admin/{nonExistentId}");

//    // ASSERT (THEN)
//    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
//}

//[Fact]
//public async Task Given_AdminToken_When_GettingList_Then_ShouldReturn200AndValidFormat()
//{
//    // ARRANGE
//    var adminToken = TestTokenGenerator.GenerateToken(_configuration, Guid.NewGuid(), "Admin");
//    _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminToken);

//    // ACT
//    var response = await _client.GetAsync("/api/v1/admin/users");

//    // ASSERT
//    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//    var content = await response.Content.ReadAsStringAsync();

//    Assert.Contains("\"success\":true", content);
//    Assert.Contains("\"data\":", content);
//    Assert.Contains("\"currentPage\":", content);
//    Assert.Contains("\"totalCount\":", content);
//    Assert.Contains("\"pageSize\":", content);
//    Assert.Contains("\"totalPages\":", content);
//    Assert.Contains("\"hasPrevious\":", content);
//    Assert.Contains("\"hasNext\":", content);
//    Assert.Contains("\"items\":[", content);
//    Assert.Contains("\"errorMessages\":null", content);
//}
//[Fact]
//public async Task Given_NoToken_When_GettingList_Then_ShouldReturn401Unauthorized()
//{
//    _client.DefaultRequestHeaders.Authorization = null;

//    var response = await _client.GetAsync("/api/v1/admin/users");

//    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//}
//[Fact]
//public async Task Given_InvalidTokenFormat_When_GettingList_Then_ShouldReturn401Unauthorized()
//{
//    var invalidToken = "Bearer asdñfkj234asdfjlkj43209841094.INVALID";
//    _client.DefaultRequestHeaders.Authorization =
//        new AuthenticationHeaderValue("Bearer", invalidToken);

//    var response = await _client.GetAsync("/api/v1/admin/users");

//    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//}
//[Fact]
//public async Task Given_CommonUserToken_When_GettingDetailById_Then_ShouldReturn403Forbidden()
//{
//    // ARRANGE
//    var existingUserId = Guid.NewGuid();
//    var commonUserToken = GenerateToken(existingUserId, "User");
//    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", commonUserToken);

//    // ACT
//    var response = await _client.GetAsync($"/api/v1/admin/users/{existingUserId}");

//    // ASSERT
//    Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
//}
//[Fact]
//public async Task Given_NoToken_When_GettingDetailById_Then_ShouldReturn401Unauthorized()
//{
//    // ARRANGE
//    var existingUserId = Guid.NewGuid();
//    _client.DefaultRequestHeaders.Authorization = null;

//    // ACT
//    var response = await _client.GetAsync($"/api/v1/admin/users/{existingUserId}");

//    // ASSERT
//    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//}
//[Fact]
//public async Task Given_InvalidTokenFormat_When_GettingDetailById_Then_ShouldReturn401Unauthorized()
//{
//    // ARRANGE
//    var existingUserId = Guid.NewGuid();
//    var invalidToken = "Bearer asdñfkj234asdfjlkj43209841094.INVALID";
//    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", invalidToken);

//    // ACT
//    var response = await _client.GetAsync($"/api/v1/admin/users/{existingUserId}");

//    // ASSERT
//    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//}
//    }
//}


using FCG.FunctionalTests.Helpers;
using FCG.IntegratedTests.Configurations;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Headers;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class AdminControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;
        private readonly IConfiguration _configuration;

        public AdminControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _configuration = factory.Services.GetRequiredService<IConfiguration>();
        }

        [Fact]
        public async Task Given_AdminToken_When_GettingUsersList_Then_ShouldReturn200WithPaginatedData()
        {
            // Given
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _client.GetAsync("/api/v1/admin/users");

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
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _client.GetAsync("/api/v1/admin/users?pageNumber=1&pageSize=1");

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
            var existingUser = _factory.CreatedUsers.First();
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _client.GetAsync($"/api/v1/admin/users?emailFilter={existingUser.Email.Value}");

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
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _client.GetAsync("/api/v1/admin/users?roleFilter=User");

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
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            // When
            var response = await _client.GetAsync("/api/v1/admin/users");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_NoToken_When_GettingUsersList_Then_ShouldReturn401Unauthorized()
        {
            // Given
            _client.DefaultRequestHeaders.Authorization = null;

            // When
            var response = await _client.GetAsync("/api/v1/admin/users");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_InvalidToken_When_GettingUsersList_Then_ShouldReturn401Unauthorized()
        {
            // Given
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "invalid.token.here");

            // When
            var response = await _client.GetAsync("/api/v1/admin/users");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_AdminTokenAndNonExistentUserId_When_GettingUserDetails_Then_ShouldReturn404NotFound()
        {
            // Given
            var nonExistentId = Guid.NewGuid();
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _client.GetAsync($"/api/v1/admin/users/{nonExistentId}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Given_CommonUserToken_When_GettingUserDetails_Then_ShouldReturn403Forbidden()
        {
            // Given
            var existingUser = _factory.CreatedUsers.First();
            var userToken = GenerateToken(Guid.NewGuid(), "User");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            // When
            var response = await _client.GetAsync($"/api/v1/admin/users/{existingUser.Id}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Given_NoToken_When_GettingUserDetails_Then_ShouldReturn401Unauthorized()
        {
            // Given
            var existingUser = _factory.CreatedUsers.First();
            _client.DefaultRequestHeaders.Authorization = null;

            // When
            var response = await _client.GetAsync($"/api/v1/admin/users/{existingUser.Id}");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_InvalidUserId_When_GettingUserDetails_Then_ShouldReturn400BadRequest()
        {
            // Given
            var adminToken = GenerateToken(Guid.NewGuid(), "Admin");
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _client.GetAsync("/api/v1/admin/users/invalid-guid");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        private string GenerateToken(Guid userId, string role)
        {
            return TestTokenGenerator.GenerateToken(_configuration, userId, role);
        }

    }
}