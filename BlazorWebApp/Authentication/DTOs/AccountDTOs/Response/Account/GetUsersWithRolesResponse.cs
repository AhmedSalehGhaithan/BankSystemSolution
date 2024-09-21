namespace BlazorWebApp.Authentication.DTOs.Accounts.Response.Account
{
    public class GetUsersWithRolesResponse
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public string? RoleId { get; set; }
    }
}
