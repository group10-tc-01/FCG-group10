using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.FunctionalTests.Helpers;
using FCG.Infrastructure.Persistance;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace FCG.IntegratedTests.Configurations
{
    public class FcgFixture : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly HttpClient _httpClient;
        protected readonly CustomWebApplicationFactory Factory;
        private readonly IConfiguration _configuration;

        public FcgFixture(CustomWebApplicationFactory factory)
        {
            Factory = factory;
            _httpClient = factory.CreateClient();
            _configuration = factory.Services.GetRequiredService<IConfiguration>();

        }

        protected async Task<HttpResponseMessage> DoPost<T>(string url, T content)
        {
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, stringContent);
        }
        protected async Task<HttpResponseMessage> DoAuthenticatedPut<T>(string url, T content, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(url, stringContent);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedPost<T>(string url, T content, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, stringContent);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedPostWithoutContent(string url, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            return await _httpClient.PostAsync(url, null);
        }
        protected async Task<User> AddUserToDatabaseAsync(string email, string password = "OriginalPass!1")
        {
            // 1. Cria um escopo de serviço
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            // 2. Cria as entidades
            var user = User.Create("Test User", email, password, Role.User);
            var wallet = Wallet.Create(user.Id); // Cria a Wallet com saldo inicial 10

            // 3. Persiste no banco de dados
            dbContext.Users.Add(user);
            dbContext.Wallets.Add(wallet);

            await dbContext.SaveChangesAsync();

            return user;
        }
        protected string GenerateToken(Guid userId, string role)
        {
            return TestTokenGenerator.GenerateToken(_configuration, userId, role);
        }
        private void SetAuthenticationHeader(string jwtToken)
        {
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }
        }

        protected void ClearAuthentication()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
