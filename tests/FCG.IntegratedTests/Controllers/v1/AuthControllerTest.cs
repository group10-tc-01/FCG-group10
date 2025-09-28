using FCG.Application.UseCases.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.IntegratedTests.Configurations;
using FCG.WebApi.Models;
using FluentAssertions;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class AuthControllerTest : FcgFixture
    {
        public AuthControllerTest(CustomWebApplicationFactory factory) : base(factory) { }

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
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.AccessToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.RefreshToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.ExpiresInMinutes.Should().BeGreaterThan(0);
        }
    }
}
