namespace BankSystem.Application.DTOs.AccountDTOs.Response.Account
{
    public record UserClaimsDTO(string FullName = null!,string UserName = null!,string Email = null!,string Role = null!);
}
