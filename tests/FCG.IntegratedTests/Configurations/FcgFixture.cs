using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.ValueObjects;
using FCG.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

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

        #region POST Helpers

        protected async Task<HttpResponseMessage> DoPost<T>(string url, T content)
        {
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, stringContent);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedPost<T>(string url, T content, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, stringContent);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedPostWithoutContent(string url, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            return await _httpClient.PostAsync(url, null);
        }

        #endregion

        #region PUT Helpers

        protected async Task<HttpResponseMessage> DoAuthenticatedPut<T>(string url, T content, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PutAsync(url, stringContent);
        }

        #endregion

        #region PATCH Helpers

        protected async Task<HttpResponseMessage> DoPatch<T>(string url, T content)
        {
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = stringContent
            };
            return await _httpClient.SendAsync(request);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedPatch<T>(string url, T content, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
            {
                Content = stringContent
            };
            return await _httpClient.SendAsync(request);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedPatchWithoutContent(string url, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            return await _httpClient.SendAsync(request);
        }

        #endregion

        #region GET Helpers

        protected async Task<HttpResponseMessage> DoGet(string url)
        {
            return await _httpClient.GetAsync(url);
        }

        protected async Task<HttpResponseMessage> DoAuthenticatedGet(string url, string jwtToken)
        {
            SetAuthenticationHeader(jwtToken);
            return await _httpClient.GetAsync(url);
        }

        #endregion

        #region Database Helpers
        protected async Task<User> AddUserToDatabaseAsync(string email, string password = "OriginalPass!1", Role role = Role.User)
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Email.Value == email);
            if (existingUser != null) return existingUser;

            var user = User.Create("Test User", email, password, role);
            var wallet = Wallet.Create(user.Id);
            var library = Library.Create(user.Id);

            dbContext.Users.Add(user);
            dbContext.Wallets.Add(wallet);
            dbContext.Libraries.Add(library);

            await dbContext.SaveChangesAsync();
            return user;
        }
        protected async Task<Game> AddGameToDatabaseAsync(string name = "Default Test Game", decimal price = 59.99m, GameCategory category = GameCategory.Action)
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var existingGame = await dbContext.Games.FirstOrDefaultAsync(g => g.Name.Value == name);
            if (existingGame != null) return existingGame;

            var game = Game.Create(Name.Create(name), "Test Description", Price.Create(price), GameCategory.Action);
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();
            return game;
        }
        protected async Task<LibraryGame> AddGameToLibraryAsync(Guid userId, Guid gameId, decimal purchasePrice = 49.99m, GameStatus status = GameStatus.Active)
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();

            var library = await dbContext.Libraries.FirstOrDefaultAsync(l => l.UserId == userId);
            if (library == null)
            {
                throw new InvalidOperationException($"Cannot add game to library: Library not found for UserId {userId}. Ensure AddUserToDatabaseAsync was called first.");
            }
            var existingEntry = await dbContext.LibraryGames.FirstOrDefaultAsync(lg => lg.LibraryId == library.Id && lg.GameId == gameId);
            if (existingEntry != null) return existingEntry;

            var libraryGame = LibraryGame.Create(library.Id, gameId, Price.Create(purchasePrice));
            if (status == GameStatus.Suspended)
            {
                libraryGame.Suspend();
            }

            dbContext.LibraryGames.Add(libraryGame);
            await dbContext.SaveChangesAsync();
            return libraryGame;
        }
        protected async Task ClearLibraryGamesAsync()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<FcgDbContext>();
            await dbContext.LibraryGames.ExecuteDeleteAsync(); // Cuidado: Apaga TUDO
        }

        #endregion

        #region Authentication Helpers

        protected string GenerateToken(Guid userId, string role)
        {
            return TokenServiceBuilder.GenerateToken(_configuration, userId, role);
        }

        protected void ClearAuthentication()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        private void SetAuthenticationHeader(string jwtToken)
        {
            if (!string.IsNullOrEmpty(jwtToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            }
        }

        #endregion
    }
}