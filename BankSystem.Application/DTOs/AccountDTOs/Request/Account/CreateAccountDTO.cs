using System.ComponentModel.DataAnnotations;

namespace BankSystem.Application.DTOs.AccountDTOs.Request.Account
{
    public class CreateAccountDTO : LoginDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
