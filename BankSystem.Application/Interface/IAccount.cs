using BankSystem.Application.DTOs.AccountDTOs.Request.Account;
using BankSystem.Application.DTOs.AccountDTOs.Response;
using BankSystem.Application.DTOs.AccountDTOs.Response.Account;
using BankSystem.SharedLibrarySolution.Responses;

namespace BankSystem.Application.Interface
{
    public interface IAccount
    {
        Task CreateAdmin();
        Task<Response> CreateAccountAsync(CreateAccountDTO model);
        Task<LoginResponse> LoginAccountAsync(LoginDTO model);
        Task<Response> CreateRoleAsync(CreateRoleDTO model);
        Task<Response> DeleteUserAsync(string email);
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenDTO model);
        CreateAccountDTO CreateFirstAdmin();
        Task<Response> ChangeUserRoleAsync(ChangeUserRoleRequestDTO model);
        Task<IEnumerable<GetRoleDTO>> GetRolesAsync();
        Task<IEnumerable<GetUsersWithRolesResponseDTO>> GetUsersWithRoles();
    }
}
