using System.Text.Json;

namespace FCG.IntegratedTests.Configurations
{
    public class FcgFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public FcgFixture(CustomWebApplicationFactory factory) => _httpClient = factory.CreateClient();

        protected async Task<HttpResponseMessage> DoPost<T>(string url, T content)
        {
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, stringContent);
        }
    }
}
