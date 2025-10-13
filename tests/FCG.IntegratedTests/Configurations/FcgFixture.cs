using System.Net.Http.Headers;
using System.Text.Json;

namespace FCG.IntegratedTests.Configurations
{
    public class FcgFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;
        protected readonly CustomWebApplicationFactory Factory;

        public FcgFixture(CustomWebApplicationFactory factory)
        {
            Factory = factory;
            _httpClient = factory.CreateClient();
        }

        #region POST Helpers

        protected async Task<HttpResponseMessage> DoPost<T>(string url, T content)
        {
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(url, stringContent);
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

        #endregion

        #region PATCH Helpers

        protected async Task<HttpResponseMessage> DoPatch<T>(string url, T content)
        {
            var json = JsonSerializer.Serialize(content);
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
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
            var stringContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
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
