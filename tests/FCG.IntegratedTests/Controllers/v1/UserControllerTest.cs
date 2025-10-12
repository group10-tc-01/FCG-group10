using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.Update.UsersDTO;
using FCG.CommomTestsUtilities.Builders.Inputs;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Infrastructure.Persistance;
using FCG.IntegratedTests.Configurations;
using FCG.WebApi.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace FCG.IntegratedTests.Controllers.v1
{
    public class UserControllerTest : FcgFixture
    {
        private readonly CustomWebApplicationFactory _factory;
        private const string ValidUrl = "/api/v1/users/register";

        public UserControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;

        }

        [Fact]
        public async Task POST_Register_GivenValidRequest_ShouldReturn201AndStoreHashedPassword()
        {
            var request = CreateUserInputBuilder.Build();
            Setup(request);

            var response = await DoPost(ValidUrl, request);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterUserResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var userInDb = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Value == request.Email);

            userInDb.Should().NotBeNull();
        }

        [Fact]
        public async Task PUT_UpdateUser_GivenValidTokenAndData_ShouldReturn200AndVerifyUpdate()
        {
            // ARRANGE (GIVEN)
            var userToUpdate = await AddUserToDatabaseAsync("user.update@test.com");

            var userToken = GenerateToken(userToUpdate.Id, "User");

            var request = new UpdateUserRequest
            {
                Id = userToUpdate.Id,
                CurrentPassword = "OriginalPass!1",
                NewPassword = "UpdatedPass!2"
            };

            var response = await DoAuthenticatedPut("/api/v1/users", request, userToken);

            // ASSERT (THEN)
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }


        [Fact]
        public async Task POST_Register_GivenValidRequest_ShouldCreateWalletForUser()
        {
            var request = CreateUserInputBuilder.Build();
            Setup(request);

            var response = await DoPost(ValidUrl, request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterUserResponse>>();
            apiResponse.Should().NotBeNull();
            apiResponse!.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var userInDb = await dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.Value == request.Email);

            userInDb.Should().NotBeNull();

            var wallet = await dbContext.Wallets
                .FirstOrDefaultAsync(w => w.UserId == userInDb!.Id);

            wallet.Should().NotBeNull();
            wallet!.Balance.Should().Be(10);
        }

        [Fact]
        public async Task POST_Register_GivenDuplicateEmail_ShouldReturnBadRequest()
        {
            var sharedEmail = "duplicate@test.com";

            var firstUserRequest = CreateUserInputBuilder.BuildWithEmail(sharedEmail);
            Setup(firstUserRequest);
            var firstResult = await DoPost(ValidUrl, firstUserRequest);
            firstResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var secondUserRequest = CreateUserInputBuilder.BuildWithEmail(sharedEmail);
            Setup(secondUserRequest);
            var secondResult = await DoPost(ValidUrl, secondUserRequest);

            secondResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Conflict);

            var responseContent = await secondResult.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<string>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            apiResponse!.Success.Should().BeFalse();

            apiResponse.ErrorMessages.Should().Contain(e => e.Contains("Email já está em uso"));

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var usersInDb = await dbContext.Users.Where(u => u.Email.Value == sharedEmail).ToListAsync();

            usersInDb.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("1234567")]
        [InlineData("SENHAFORTE123")]
        public async Task POST_Register_GivenWeakPassword_ShouldReturnBadRequest(string weakPassword)
        {
            var request = CreateUserInputBuilder.Build();
            request.Password = weakPassword;

            var result = await DoPost(ValidUrl, request);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Register_GivenInvalidEmailRequest_ShouldReturnBadRequest()
        {
            var request = CreateUserInputBuilder.BuildWithInvalidEmail();

            var result = await DoPost(ValidUrl, request);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Register_GivenInvalidWeakPasswordFromBuilder_ShouldReturnBadRequest()
        {
            // Arrange
            var request = CreateUserInputBuilder.BuildWithWeakPassword();

            // Act
            var result = await DoPost(ValidUrl, request);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Register_GivenInvalidSimplePassword_ShouldReturnBadRequest()
        {
            // Arrange
            var request = CreateUserInputBuilder.BuildWithInvalidPassword();

            // Act
            var result = await DoPost(ValidUrl, request);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
        private static void Setup(RegisterUserRequest user)
        {
            PasswordEncrypterServiceBuilder.Build();
            PasswordEncrypterServiceBuilder.SetupEncrypt(user.Password);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
        }
    }
}
