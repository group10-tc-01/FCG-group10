using FCG.IntegratedTests.Configurations;
using FluentAssertions;
using System.Net;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class LibraryControllerTest : FcgFixture
    {
        public LibraryControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Given_NoToken_When_GettingMyLibrary_Then_ShouldReturn401Unauthorized()
        {

            var response = await _httpClient.GetAsync("/api/v1/libraries/my-library");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}