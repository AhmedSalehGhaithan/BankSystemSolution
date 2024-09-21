#region Transaction Repository
using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.Infrastructure.Data;
using BankSystem.SharedLibrarySolution.Logs;
using BankSystem.SharedLibrarySolution.Responses;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankSystem.Infrastructure.Repositories
{
    public class TransactionRepository(BankAccountDbContext _context, IMyCacheService _cacheService) : ITransaction
    {
        public async Task<Response> CreateAsync(Transaction entity)
        {
            if (entity == null) return new Response(false, "Transaction entity cannot be null.");

            try
            {
                await _context.Transactions.AddAsync(entity);
                await _context.SaveChangesAsync();
                return new Response(true, "Transaction created successfully.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occurred while executing transaction");
            }
        }

        public async Task<Response> DeleteAsync(Transaction entity)
        {
            var check = await _context.Transactions.FindAsync(entity.Id);
            if (check == null) return new Response(false, $"Transaction with the id [{entity.Id}] is not found");

            try
            {
                _context.Transactions.Remove(check);
                await _context.SaveChangesAsync();
                return new Response(true, "Transaction Deleted Successfully");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error occurred while deleting transaction");
            }
        }

        public async Task<Transaction> FindByIdAsync(int id) =>
            await _context.Transactions.FindAsync(id) ?? throw new Exception("Transaction not found");

        public async Task<Response> DepositAsync(Transaction entity)
        {
            var account = await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == entity.BankAccountNumber);
            if (account == null) return new Response(false, $"Account not found with number {entity.BankAccountNumber}");

            account.Balance += entity.Amount;
            entity.TransactionType = "Deposit";

            await CreateAsync(entity);
            return new Response(true, $"Amount ${entity.Amount} added to {account.FullName}'s account.");
        }

        public async Task<Response> WithdrawAsync(Transaction entity)
        {
            var account = await _context.BankAccounts.FirstOrDefaultAsync(a => a.AccountNumber == entity.BankAccountNumber);
            if (account == null) return new Response(false, $"Account not found with number {entity.BankAccountNumber}");

            account.Balance -= entity.Amount;
            entity.TransactionType = "Withdraw";
            entity.TransactionDate = DateTime.Now;

            await CreateAsync(entity);
            return new Response(true, $"${entity.Amount} deducted from {account.FullName}'s account.");
        }

        public async Task<Response> TransferAsync(Transaction entity)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var sender = await _context.BankAccounts.FirstOrDefaultAsync(b => b.AccountNumber == entity.BankAccountNumber);
                    var receiver = await _context.BankAccounts.FirstOrDefaultAsync(b => b.AccountNumber == entity.ReceiverAccountNumber);

                    if (sender == null || receiver == null)
                        return new Response(false, "Both Sender and Receiver must have valid bank account numbers.");

                    if (sender.Balance < entity.Amount)
                        return new Response(false, "Insufficient balance in the sender's account.");

                    if (sender.AccountNumber == receiver.AccountNumber)
                        return new Response(false, "Cannot transfer to the same account.");

                    sender.Balance -= entity.Amount;
                    receiver.Balance += entity.Amount;
                    entity.TransactionType = "Transfer";

                    await CreateAsync(entity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new Response(true, $"Amount ${entity.Amount} successfully transferred from {sender.FullName} to {receiver.FullName}.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    LogException.LogExceptions(ex);
                    return new Response(false, "An error occurred while processing the transfer.");
                }
            });
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync() =>
            await _context.Transactions.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<Transaction>> GetTransactionsByBankAccountNumberAsync(string bankAccountNumber) =>
            await _context.Transactions.Where(t => t.BankAccountNumber == bankAccountNumber).ToListAsync();
    }
}
#endregion
