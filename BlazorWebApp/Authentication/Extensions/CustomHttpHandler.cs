using BlazorWebApp.Authentication.DTOs.Accounts.Request.Account;
using BlazorWebApp.Authentication.Service;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
namespace BlazorWebApp.Authentication.DTOs.Extensions
{
    public class CustomHttpHandler(LocalStorageService _localStorageService,
       HttpClientService _httpClientService,
       NavigationManager _navigationManager,
       IAccountService _accountService) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                bool loginUrl = request.RequestUri!.AbsoluteUri.Contains(ConstantValues.LoginRoute);

                bool registerUrl = request.RequestUri.AbsoluteUri.Contains(ConstantValues.RegisterRoute);

                bool refreshTokenUrl = request.RequestUri.AbsoluteUri.Contains(ConstantValues.RefreshTokenRoute);

                bool adminCreateUrl = request.RequestUri.AbsoluteUri.Contains(ConstantValues.CreateAdminRoute);

                if (loginUrl || registerUrl || refreshTokenUrl || adminCreateUrl)
                    return await base.SendAsync(request, cancellationToken);

                var result = await base.SendAsync(request, cancellationToken);
                if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //get token from local storage
                    var tokenModel = await _localStorageService.GetModelFromToken();
                    if (tokenModel == null) return result;

                    //call for refresh token
                    var newJtwToken = await GetRefreshToken(tokenModel.Refresh!);
                    if (string.IsNullOrEmpty(newJtwToken)) return result;

                    request.Headers.Authorization = new AuthenticationHeaderValue(ConstantValues.HttpClientHeaderScheme, newJtwToken);
                    return await base.SendAsync(request, cancellationToken);
                }
                return result;
            }
            catch { return null!; }
        }

        private async Task<string> GetRefreshToken(string refreshToken)
        {
            try
            {
                var client = _httpClientService.GetPublicClient();
                var response = await _accountService.RefreshTokenAsync(new RefreshToken() { Token = refreshToken });
                if (response == null || response.Token == null)
                {
                    await ClearBrowserStorage();
                    NavigateToLogin();
                    return null!;
                }

                await _localStorageService.RemoveTokenBrowserLocalStorage();
                await _localStorageService.SetBrowserLocalStorage(new LocalStorage() { Refresh = response.RefreshToken, Token = response.Token });
                return response.Token;
            }
            catch (Exception ex) { return null!; }
        }

        private void NavigateToLogin() => _navigationManager.NavigateTo(_navigationManager.BaseUri, true, true);

        private async Task ClearBrowserStorage() => await _localStorageService.RemoveTokenBrowserLocalStorage();
    }
}
