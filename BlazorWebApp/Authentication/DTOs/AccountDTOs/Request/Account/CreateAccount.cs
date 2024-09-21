using System.ComponentModel.DataAnnotations;

namespace BlazorWebApp.Authentication.DTOs.Accounts.Request.Account
{
    public class CreateAccount : LoginModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}
