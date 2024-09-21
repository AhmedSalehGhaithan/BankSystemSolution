using BankSystem.Application.DTOs;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Interface;
using BankSystem.SharedLibrarySolution.Responses;

namespace BankSystem.Application.Interface
{
    public interface IBankAccount : IGenericInterface<BankAccount>
    {
        Task<Response> UpdateAsync(BankAccount entity);
        Task<BankAccount> GetByAccountNumberAsync(string accountNumber);
    }
}
