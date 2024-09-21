using BankSystem.Application.Constant;
using BankSystem.Application.DecoratorBase;
using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Responses;
using System.Collections.Generic;

namespace BankSystem.Infrastructure.TransactionConcreteDecorators
{
    public class CachingTransactionServiceDecorator : TransactionDecorator
    {
        private readonly IMyCacheService _cacheService;

        public CachingTransactionServiceDecorator(ITransaction inner, IMyCacheService cacheService): base(inner)
        {
            _cacheService = cacheService;
        }

        public override async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            var cachedTransactions = _cacheService.GetData<IEnumerable<Transaction>>(ConstantValues.AllTransactionsKey);

            if (cachedTransactions != null)
            {
                return cachedTransactions;
            }

            var transactions = await base.GetAllAsync();
            _cacheService.SetData(ConstantValues.AllTransactionsKey, transactions, DateTimeOffset.Now.AddDays(1));
            return transactions;
        }

        public override async Task<Transaction> FindByIdAsync(int id)
        {
            var cacheKey = $"Transaction_{id}";
            var cachedTransaction = _cacheService.GetData<Transaction>(cacheKey);

            if (cachedTransaction != null)
            {
                return cachedTransaction;
            }

            var transaction = await base.FindByIdAsync(id);
            if (transaction != null)
            {
                _cacheService.SetData(cacheKey, transaction, DateTimeOffset.Now.AddDays(1));
            }
            return transaction!;
        }

        public override async Task<IEnumerable<Transaction>> GetTransactionsByBankAccountNumberAsync(string bankAccountNumber)
        {
            var cacheKey = $"Transactions_{bankAccountNumber}";
            var cachedTransactions = _cacheService.GetData<IEnumerable<Transaction>>(cacheKey);

            if (cachedTransactions != null)
            {
                return cachedTransactions;
            }

            var transactions = await base.GetTransactionsByBankAccountNumberAsync(bankAccountNumber);
            _cacheService.SetData(cacheKey, transactions, DateTimeOffset.Now.AddDays(1));
            return transactions;
        }

        public override async Task<Response> CreateAsync(Transaction entity)
        {
            var result = await base.CreateAsync(entity);
            if (result.Flag)
            {
                ClearCache(entity);
            }
            return result;
        }

        public override async Task<Response> DeleteAsync(Transaction entity)
        {
            var result = await base.DeleteAsync(entity);
            if (result.Flag)
            {
                ClearCache(entity);
            }
            return result;
        }

        public override async Task<Response> DepositAsync(Transaction entity)
        {
            var result = await base.DepositAsync(entity);
            if (result.Flag)
            {
                ClearCache(entity);
            }
            return result;
        }

        public override async Task<Response> WithdrawAsync(Transaction entity)
        {
            var result = await base.WithdrawAsync(entity);
            if (result.Flag)
            {
                ClearCache(entity);
            }
            return result;
        }

        public override async Task<Response> TransferAsync(Transaction entity)
        {
            var result = await base.TransferAsync(entity);
            if (result.Flag)
            {
                ClearCache(entity);
            }
            return result;
        }

        private void ClearCache(Transaction entity)
        {
            // Clear relevant cache entries
            _cacheService.RemoveData(ConstantValues.AllTransactionsKey);

            _cacheService.RemoveData(ConstantValues.AllBankAccountsKey);

            _cacheService.RemoveData($"Transaction_{entity.Id}");

            _cacheService.RemoveData($"Transactions_{entity.BankAccountNumber}");

            if (!string.IsNullOrEmpty(entity.ReceiverAccountNumber))
            {
                _cacheService.RemoveData($"Transactions_{entity.ReceiverAccountNumber}");
            }
        }
    }
}
