using System.Net.Http.Headers;
namespace BlazorWebApp.Authentication.DTOs.Extensions
{
    public class HttpClientService(IHttpClientFactory _httpClientFactory,LocalStorageService _localStorageService)
    {
        private HttpClient CreateClient() => _httpClientFactory.CreateClient(ConstantValues.HttpClientName);
        public HttpClient GetPublicClient() => CreateClient();

        public async Task<HttpClient> GetPrivateClient()
        {
            try
            {
                var client = CreateClient();
                var localStorageDTO = await _localStorageService.GetModelFromToken();
                if (string.IsNullOrEmpty(localStorageDTO.Token)) return client;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ConstantValues.HttpClientHeaderScheme, localStorageDTO.Token);
                    return client;
            }
            catch { return new HttpClient(); }
        }
    }
}
