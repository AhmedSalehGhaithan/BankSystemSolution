
using BlazorWebApp.Authentication.DTOs.Accounts.Request.Account;
using BlazorWebApp.Authentication.DTOs.Accounts.Response.Account;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWebApp.Authentication.DTOs.Extensions
{
    public class CustomAuthenticationStateProvider(LocalStorageService _localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenModel = await _localStorageService.GetModelFromToken();
            if (string.IsNullOrEmpty(tokenModel.Token))
                return await Task.FromResult(new AuthenticationState(anonymous));

            var getUserClaim = DyCryptToken(tokenModel.Token);
            if(getUserClaim == null) return await Task.FromResult(new AuthenticationState(anonymous));

            var claimsPrincipal = SetClaimsPrincipal(getUserClaim);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task UpdateAuthenticationState(LocalStorage localStorageDTO)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if(localStorageDTO.Token != null || localStorageDTO.Refresh != null)
            {
                await _localStorageService.SetBrowserLocalStorage(localStorageDTO);
                var getUserClaim = DyCryptToken(localStorageDTO.Token);
                claimsPrincipal = SetClaimsPrincipal(getUserClaim);
            }
            else { await _localStorageService.RemoveTokenBrowserLocalStorage(); }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }

        public static ClaimsPrincipal SetClaimsPrincipal(UserClaims claims)
        {
            if (claims.Email is null) return new ClaimsPrincipal();
            return new ClaimsPrincipal(new ClaimsIdentity([
                new(ClaimTypes.Name,claims.UserName),
                new(ClaimTypes.Email,claims.Email),
                new (ClaimTypes.Role,claims.Role),
                new Claim("FullName", claims.FullName)
            ],ConstantValues.AuthenticationType));
        }

        public static UserClaims DyCryptToken(string JwtToken)
        {
            try
            {
                if(string.IsNullOrEmpty(JwtToken)) return new UserClaims();

                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(JwtToken);

                var name = token.Claims.FirstOrDefault(_n_ => _n_.Type == ClaimTypes.Name)!.Value;
                var email = token.Claims.FirstOrDefault(_n_ => _n_.Type == ClaimTypes.Email)!.Value;
                var role = token.Claims.FirstOrDefault(_n_ => _n_.Type == ClaimTypes.Role)!.Value;
                var fullName = token.Claims.FirstOrDefault(_n_ => _n_.Type == "FullName")!.Value;

                return new UserClaims(fullName, name, email, role);

            }catch { return null!; }
        }
    }
}
