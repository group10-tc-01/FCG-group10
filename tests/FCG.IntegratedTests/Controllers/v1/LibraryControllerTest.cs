using FCG.IntegratedTests.Configurations;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class LibraryControllerTest : FcgFixture
    {
        public LibraryControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Given_AuthenticatedUser_When_GettingMyLibrary_Then_ShouldReturn200OK()
        {
            // Given
            var user = Factory.CreatedUsers.First();
            var userToken = GenerateToken(user.Id, "User");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/library/my-library");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
            content.Should().Contain("\"libraryId\":");
            content.Should().Contain("\"userId\":");
            content.Should().Contain("\"games\":");
        }

        [Fact]
        public async Task Given_NoToken_When_GettingMyLibrary_Then_ShouldReturn401Unauthorized()
        {
            // Given
            // No token set

            // When
            var response = await _httpClient.GetAsync("/api/v1/library/my-library");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Given_AuthenticatedUserWithGames_When_GettingMyLibrary_Then_ShouldReturnGamesInLibrary()
        {
            // Given
            var user = Factory.CreatedUsers.First();
            var userToken = GenerateToken(user.Id, "User");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/library/my-library");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
            content.Should().Contain("\"games\":");
        }

        [Fact]
        public async Task Given_AdminUser_When_GettingMyLibrary_Then_ShouldReturn200OK()
        {
            // Given
            var adminUser = Factory.CreatedAdminUsers.First();
            var adminToken = GenerateToken(adminUser.Id, "Admin");
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", adminToken);

            // When
            var response = await _httpClient.GetAsync("/api/v1/library/my-library");

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("\"success\":true");
        }
    }
}
