using Microsoft.AspNetCore.Identity;

namespace BankSystem.Domain.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
    
}
