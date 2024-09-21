namespace BlazorWebApp.Authentication.DTOs.Accounts.Request.Account
{
    public record ChangeUserRoleRequest(string userEmail, string RoleName);
}
