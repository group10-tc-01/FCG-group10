using FCG.Application.UseCases.Example.CreateExample;
using FCG.CommomTestsUtilities.Builders.Inputs.Example;
using FCG.IntegratedTests.Configurations;
using FCG.WebApi.Models;
using FluentAssertions;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class ExampleControllerTest : FcgFixture
    {
        public ExampleControllerTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Given_ValidCreateExampleInput_When_PostIsCalled_ShouldReturnCreatedResult()
        {
            // Arrange
            var validUrl = "/api/v1/example";
            var createExampleInput = CreateExampleInputBuilder.Build();

            // Act
            var result = await DoPost(validUrl, createExampleInput);
            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<CreateExampleOutput>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            apiResponse.Should().NotBeNull();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Example.Name.Should().Be(createExampleInput.Name);
            apiResponse.Data.Example.Description.Should().Be(createExampleInput.Description);
        }
    }
}
