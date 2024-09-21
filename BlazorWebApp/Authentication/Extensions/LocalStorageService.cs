
using BlazorWebApp.Authentication.DTOs.Accounts.Request.Account;
using NetcodeHub.Packages.Extensions.LocalStorage;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace BlazorWebApp.Authentication.DTOs.Extensions
{
    public class LocalStorageService(ILocalStorageService _localStorageService)
    {
        private async Task<string> GetBrowserLocalStorage()
        {
            var tokenModel = await _localStorageService.GetEncryptedItemAsStringAsync(ConstantValues.BrowserStorageKey);
            return tokenModel!;
        }
        public async Task<LocalStorage> GetModelFromToken()
        {
            try
            {
                string token = await GetBrowserLocalStorage();
                if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
                    return new LocalStorage();
                return DeserializeJsonString<LocalStorage>(token);
            }
            catch { return new LocalStorage(); }
        }

        public async Task SetBrowserLocalStorage(LocalStorage localStorageDTO)
        {
            try
            {
                string token = SerializeObj(localStorageDTO);
                await _localStorageService.SaveAsEncryptedStringAsync(ConstantValues.BrowserStorageKey, token);
            }
            catch { }
        }
        
        public async Task RemoveTokenBrowserLocalStorage()
            => await _localStorageService.DeleteItemAsync(ConstantValues.BrowserStorageKey);

        private static string SerializeObj<T>(T modelObject)
            => JsonSerializer.Serialize(modelObject,JsonOption());

        private static T DeserializeJsonString<T>(string jsonString) 
            => JsonSerializer.Deserialize<T>(jsonString, JsonOption())!;

        private static JsonSerializerOptions JsonOption()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }

    }
}
