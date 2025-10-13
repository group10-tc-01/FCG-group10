﻿using FCG.Application.UseCases.Authentication.Login;
using FCG.Application.UseCases.Users.Register.UsersDTO;
using FCG.Application.UseCases.Users.RoleManagement.RoleManagementDTO;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs;
using FCG.CommomTestsUtilities.Builders.Inputs.Authentication.Login;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
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
        private const string RegisterUrl = "/api/v1/users/register";
        private const string UpdateRoleUrl = "/api/v1/users/admin/update-role";
        private const string LoginUrl = "/api/v1/auth/login";


        public UserControllerTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task POST_Register_GivenValidRequest_ShouldReturn201AndStoreHashedPassword()
        {
            var request = CreateUserInputBuilder.Build();
            Setup(request);

            var response = await DoPost(RegisterUrl, request);

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
        public async Task POST_Register_GivenDuplicateEmail_ShouldReturnBadRequest()
        {
            var sharedEmail = "duplicate@test.com";

            var firstUserRequest = CreateUserInputBuilder.BuildWithEmail(sharedEmail);
            Setup(firstUserRequest);
            var firstResult = await DoPost(RegisterUrl, firstUserRequest);
            firstResult.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var secondUserRequest = CreateUserInputBuilder.BuildWithEmail(sharedEmail);
            Setup(secondUserRequest);
            var secondResult = await DoPost(RegisterUrl, secondUserRequest);

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

            var result = await DoPost(RegisterUrl, request);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Register_GivenInvalidEmailRequest_ShouldReturnBadRequest()
        {
            var request = CreateUserInputBuilder.BuildWithInvalidEmail();

            var result = await DoPost(RegisterUrl, request);

            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Register_GivenInvalidWeakPasswordFromBuilder_ShouldReturnBadRequest()
        {
            // Arrange
            var request = CreateUserInputBuilder.BuildWithWeakPassword();

            // Act
            var result = await DoPost(RegisterUrl, request);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task POST_Register_GivenInvalidSimplePassword_ShouldReturnBadRequest()
        {
            // Arrange
            var request = CreateUserInputBuilder.BuildWithInvalidPassword();

            // Act
            var result = await DoPost(RegisterUrl, request);

            // Assert
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        #region RoleManagement

        [Fact]
        public async Task PATCH_UpdateUserRole_AsAdmin_ShouldReturnOk()
        {
            // Arrange
            var admin = UserBuilder.BuildAdmin();
            var userToPromote = UserBuilder.BuildRegularUser();

            await PersistUserAsync(admin);
            await PersistUserAsync(userToPromote);

            var adminToken = await LoginAndGetJwtAsync(admin);

            var request = new RoleManagementRequest(userToPromote.Id, Role.Admin);

            // Act
            var result = await DoAuthenticatedPatch(UpdateRoleUrl, request, adminToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await result.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<RoleManagementResponse>>(responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            apiResponse!.Success.Should().BeTrue();
            apiResponse.Data.UserId.Should().Be(userToPromote.Id);
            apiResponse.Data.Role.Should().Be(Role.Admin);
        }

        [Fact]
        public async Task PATCH_UpdateUserRole_AsNonAdmin_ShouldReturnForbidden()
        {
            // Arrange
            var normalUser = UserBuilder.BuildRegularUser();
            var userToPromote = UserBuilder.BuildRegularUser();

            // Persistir usuários
            await PersistUserAsync(normalUser);
            await PersistUserAsync(userToPromote);

            var userToken = await LoginAndGetJwtAsync(normalUser);

            var request = new RoleManagementRequest(userToPromote.Id, Role.Admin);

            // Act
            var result = await DoAuthenticatedPatch(UpdateRoleUrl, request, userToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task PATCH_UpdateUserRole_InvalidInput_ShouldReturnBadRequest()
        {
            // Arrange
            var admin = UserBuilder.BuildAdmin();
            await PersistUserAsync(admin);

            var adminToken = await LoginAndGetJwtAsync(admin);

            var request = new RoleManagementRequest(Guid.Empty, (Role)999);

            // Act
            var result = await DoAuthenticatedPatch(UpdateRoleUrl, request, adminToken);

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region Helpers

        private async Task PersistUserAsync(User user)
        {
            Setup(user);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }

        private async Task<string> LoginAndGetJwtAsync(User user)
        {
            var loginInput = LoginInputBuilder.BuildWithValues(user.Email, user.Password.Value);
            var loginResult = await DoPost(LoginUrl, loginInput);
            loginResult.StatusCode.Should().Be(HttpStatusCode.OK);

            var loginContent = await loginResult.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<LoginOutput>>(loginContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            loginApiResponse!.Success.Should().BeTrue();
            return loginApiResponse.Data.AccessToken;
        }

        private static void Setup(User user)
        {
            PasswordEncrypterServiceBuilder.Build();
            PasswordEncrypterServiceBuilder.SetupEncrypt(user.Password);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
        }

        private static void Setup(RegisterUserRequest user)
        {
            PasswordEncrypterServiceBuilder.Build();
            PasswordEncrypterServiceBuilder.SetupEncrypt(user.Password);
            PasswordEncrypterServiceBuilder.SetupIsValid(true);
        }

        #endregion
    }
}
