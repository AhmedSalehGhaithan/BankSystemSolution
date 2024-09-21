
using BlazorWebApp.Authentication.DTOs.Accounts.Request.Account;
using BlazorWebApp.Authentication.DTOs.Accounts.Response;
using BlazorWebApp.Authentication.DTOs.Accounts.Response.Account;
using BlazorWebApp.DTOs;


namespace BlazorWebApp.Authentication.Service
{
    public interface IAccountService
    {
        Task CreateAdminAtFirstStart();
        Task<Responses> DeleteAccountAsync(string email);
        Task<Responses> RegisterAccountAsync(CreateAccount model);
        Task<LoginResponse> LoginAccountAsync(LoginModel model);
        Task<Responses> CreateRoleAsync(CreateRole model);
        Task<LoginResponse> RefreshTokenAsync(RefreshToken model);
        Task<IEnumerable<GetRole>> GetRolesAsync();
        Task<IEnumerable<GetUsersWithRolesResponse>> GetUsersWithRolesAsync();
        Task<Responses> ChangeUserRoleAsync(ChangeUserRoleRequest model);
    }
}
