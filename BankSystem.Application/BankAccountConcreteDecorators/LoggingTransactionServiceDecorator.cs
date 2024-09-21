using BankSystem.Application.DecoratorBase;
using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Logs;
using BankSystem.SharedLibrarySolution.Responses;

namespace BankSystem.Application.TransactionConcreteDecorators
{
    public class LoggingTransactionServiceDecorator : TransactionDecorator
    {
        public LoggingTransactionServiceDecorator(ITransaction inner)
            : base(inner)
        {
        }

        public override async Task<Response> CreateAsync(Transaction entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Creating transaction with ID: {entity.Id}");
                var result = await base.CreateAsync(entity);
                LogException.LogToFile($"[INFO] Create operation result: {result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; // Re-throwing exception after logging
            }
        }

        public override async Task<Response> DeleteAsync(Transaction entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Deleting transaction with ID: {entity.Id}");
                var result = await base.DeleteAsync(entity);
                LogException.LogToFile($"[INFO] Delete operation result: {result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            try
            {
                LogException.LogToFile("[INFO] Retrieving all transactions");
                var transactions = await base.GetAllAsync();
                LogException.LogToFile($"[INFO] Retrieved {transactions.Count()} transactions");
                return transactions;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }

        public override async Task<Transaction> FindByIdAsync(int id)
        {
            try
            {
                LogException.LogToFile($"[INFO] Retrieving transaction with ID: {id}");
                var transaction = await base.FindByIdAsync(id);
                LogException.LogToFile(transaction != null
                    ? $"[INFO] Retrieved transaction with ID: {id}"
                    : $"[INFO] No transaction found with ID: {id}");
                return transaction!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<IEnumerable<Transaction>> GetTransactionsByBankAccountNumberAsync(string bankAccountNumber)
        {
            try
            {
                LogException.LogToFile($"[INFO] Retrieving transactions for bank account number: {bankAccountNumber}");
                var transactions = await base.GetTransactionsByBankAccountNumberAsync(bankAccountNumber);
                LogException.LogToFile($"[INFO] Retrieved {transactions.Count()} transactions for account number: {bankAccountNumber}");
                return transactions;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<Response> DepositAsync(Transaction entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Depositing ${entity.Amount} to account number: {entity.BankAccountNumber}");
                var result = await base.DepositAsync(entity);
                LogException.LogToFile($"[INFO] Deposit operation result: {result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<Response> WithdrawAsync(Transaction entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Withdrawing ${entity.Amount} from account number: {entity.BankAccountNumber}");
                var result = await base.WithdrawAsync(entity);
                LogException.LogToFile($"[INFO] Withdrawal operation result: {result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }

        public override async Task<Response> TransferAsync(Transaction entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Transferring ${entity.Amount} from account number: {entity.BankAccountNumber} to account number: {entity.ReceiverAccountNumber}");
                var result = await base.TransferAsync(entity);
                LogException.LogToFile($"[INFO] Transfer operation result: {result.Message}");
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }
    }
}
