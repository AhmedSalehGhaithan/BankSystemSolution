using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Responses;

namespace BankSystem.Application.DecoratorBase
{
    public abstract class TransactionDecorator(ITransaction _inner) : ITransaction
    {
        public virtual Task<Response> CreateAsync(Transaction entity) => _inner.CreateAsync(entity);
        public virtual Task<Response> DeleteAsync(Transaction entity) => _inner.DeleteAsync(entity);
        public virtual Task<IEnumerable<Transaction>> GetAllAsync() => _inner.GetAllAsync();
        public virtual Task<Transaction> FindByIdAsync(int id) => _inner.FindByIdAsync(id);
        public virtual Task<IEnumerable<Transaction>> GetTransactionsByBankAccountNumberAsync(string bankAccountNumber) => _inner.GetTransactionsByBankAccountNumberAsync(bankAccountNumber);
        public virtual Task<Response> DepositAsync(Transaction entity) => _inner.DepositAsync(entity);
        public virtual Task<Response> WithdrawAsync(Transaction entity) => _inner.WithdrawAsync(entity);
        public virtual Task<Response> TransferAsync(Transaction entity) => _inner.TransferAsync(entity);
    }
}
