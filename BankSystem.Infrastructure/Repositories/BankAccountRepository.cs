using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using BankSystem.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;

namespace BankSystem.Infrastructure.Repositories
{
    public class BankAccountRepository(BankAccountDbContext _context) : IBankAccount
    {
        public async Task<Response> CreateAsync(BankAccount entity)
        {
            try
            {
                if (entity == null)
                    return new Response(false, "The account entity cannot be null.");

                string generatedAccountNumber = await GenerateRandomNumber();

                entity.AccountNumber = generatedAccountNumber;

                if (await IsPhoneNumberAlreadyExist(entity.PhoneNumber!))
                    return new Response(false, $"This Phone number is already exist!");

                await _context.BankAccounts.AddAsync(entity);

                await _context.SaveChangesAsync();

                return new Response(true, "Account created successfully");
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred adding new account", ex);
            }
        }

        private async Task<bool> IsPhoneNumberAlreadyExist(string phoneNumber) =>
            await _context.BankAccounts.AnyAsync(_p => _p.PhoneNumber == phoneNumber);

        private async Task<string> GenerateRandomNumber()
        {
            try
            {
                string generatedAccountNumber;
                bool isUnique;
                Random random = new Random();

                do
                {
                    generatedAccountNumber = random.Next(100000000, 999999999).ToString();

                    var existingAccount = await _context.BankAccounts.FirstOrDefaultAsync(_u_ => _u_.AccountNumber == generatedAccountNumber);

                    isUnique = existingAccount == null;
                }
                while (!isUnique);

                return generatedAccountNumber;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred generating account number", ex);
            }
        }

        public async Task<Response> DeleteAsync(BankAccount entity)
        {
            try
            {
                var account = await FindByIdAsync(entity.Id);

                if (account is null)
                    return new Response(false, $"{entity.FullName} not found");

                _context.BankAccounts.Remove(account);

                await _context.SaveChangesAsync();

                return new Response(true, $"{entity.FullName} deleted successfully");
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred deleting account", ex);
            }
        }

        public async Task<BankAccount> FindByIdAsync(int id)
        {
            try
            {
                var account = await _context.BankAccounts.FindAsync(id);
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred retrieving account", ex);
            }
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync()
        {
            try
            {
                var accounts = await _context.BankAccounts.AsNoTracking().ToListAsync();
                return accounts;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred retrieving accounts", ex);
            }
        }

        public async Task<BankAccount> GetByAccountNumberAsync(string accountNumber)
        {
            try
            {
                var account = await _context.BankAccounts.FirstOrDefaultAsync(_u => _u.AccountNumber == accountNumber);
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred retrieving account", ex);
            }
        }

        public async Task<Response> UpdateAsync(BankAccount entity)
        {
            try
            {
                var account = await FindByIdAsync(entity.Id);

                if (account is null)
                    return new Response(false, $"{entity.FullName} not found");

                _context.Entry(account).State = EntityState.Detached;

                _context.BankAccounts.Update(entity);

                await _context.SaveChangesAsync();

                return new Response(true, $"{entity.FullName} updated successfully");
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred updating account", ex);
            }
        }
    }
}
