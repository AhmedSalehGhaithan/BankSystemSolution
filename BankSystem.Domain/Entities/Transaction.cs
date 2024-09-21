namespace BankSystem.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; } 
        public string? BankAccountNumber { get; set; } 
        public string? TransactionType { get; set; }
        public decimal Amount { get; set; } 
        public DateTime TransactionDate { get; set; }  = DateTime.Now;
        public string Description { get; set; } = string.Empty;
        public string? ReceiverAccountNumber { get; set; } = string.Empty;

    }

}
