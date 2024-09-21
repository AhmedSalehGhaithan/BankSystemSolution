using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Responses;
namespace BankSystem.Application.DecoratorBase
{
    public abstract class BankAccountDecorator(IBankAccount _inner) : IBankAccount
    {
        public virtual Task<Response> CreateAsync(BankAccount entity) => _inner.CreateAsync(entity);
        public virtual Task<Response> DeleteAsync(BankAccount entity) => _inner.DeleteAsync(entity);
        public virtual Task<BankAccount> FindByIdAsync(int id) => _inner.FindByIdAsync(id);
        public virtual Task<IEnumerable<BankAccount>> GetAllAsync() => _inner.GetAllAsync();
        public virtual Task<BankAccount> GetByAccountNumberAsync(string accountNumber) => _inner.GetByAccountNumberAsync(accountNumber);
        public virtual Task<Response> UpdateAsync(BankAccount entity) => _inner.UpdateAsync(entity);
    }

}
