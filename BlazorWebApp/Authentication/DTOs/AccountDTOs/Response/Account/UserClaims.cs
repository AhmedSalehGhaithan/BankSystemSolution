namespace BlazorWebApp.Authentication.DTOs.Accounts.Response.Account
{
    public record UserClaims(string FullName = null!,string UserName = null!,string Email = null!,string Role = null!);
}
