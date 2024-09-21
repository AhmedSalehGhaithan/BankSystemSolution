using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Interface;
using BankSystem.SharedLibrarySolution.Responses;
namespace BankSystem.Application.Interface
{
    public interface ITransaction : IGenericInterface<Transaction>
    {
        Task<IEnumerable<Transaction>> GetTransactionsByBankAccountNumberAsync(string bankAccountNumber);
        Task<Response> DepositAsync(Transaction entity);
        Task<Response> WithdrawAsync(Transaction entity);
        Task<Response> TransferAsync(Transaction entity);

    }
}
