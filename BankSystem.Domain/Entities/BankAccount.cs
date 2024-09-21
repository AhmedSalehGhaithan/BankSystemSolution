using System.ComponentModel.DataAnnotations;

namespace BankSystem.Domain.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }
        [RegularExpression("^[a-zA-Z]+([ '-][a-zA-Z]+)*$", ErrorMessage = "The name must only contain alphabetic characters, spaces, or hyphens.")]
        public string? FullName { get; set; } = string.Empty;
        [RegularExpression("^[0-9]+$", ErrorMessage = "The input must only contain numeric characters.")]

        public string? PhoneNumber { get; set; } = string.Empty;
        public string? AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }
}
