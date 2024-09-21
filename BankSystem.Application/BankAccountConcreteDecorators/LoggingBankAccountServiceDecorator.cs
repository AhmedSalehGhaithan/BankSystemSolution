using BankSystem.Application.DecoratorBase;
using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Logs;
using BankSystem.SharedLibrarySolution.Responses;
namespace BankSystem.Application.BankAccountConcreteDecorators
{
    public class LoggingBankAccountServiceDecorator : BankAccountDecorator
    {
        public LoggingBankAccountServiceDecorator(IBankAccount inner)
            : base(inner)
        {
        }
        public override async Task<Response> CreateAsync(BankAccount entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Creating account with ID: {entity.Id} and Account Number: {entity.AccountNumber}");
                
                var result = await base.CreateAsync(entity);
               
                LogException.LogToFile($"[INFO] Create operation result: {result.Message}");
                
                return result;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;// Re-throwing exception after logging
            }
        }

        public override async Task<Response> DeleteAsync(BankAccount entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Deleting account with ID: {entity.Id} and Account Number: {entity.AccountNumber}");
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

        public override async Task<IEnumerable<BankAccount>> GetAllAsync()
        {
            try
            {
                LogException.LogToFile("[INFO] Retrieving all accounts");
                var accounts = await base.GetAllAsync();
                LogException.LogToFile($"[INFO] Retrieved {accounts.Count()} accounts");
                return accounts;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<BankAccount> FindByIdAsync(int id)
        {
            try
            {
                LogException.LogToFile($"[INFO] Retrieving account with ID: {id}");
                var account = await base.FindByIdAsync(id);
                LogException.LogToFile(account != null
                    ? $"[INFO] Retrieved account with ID: {id}"
                    : $"[INFO] No account found with ID: {id}");
                return account;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<BankAccount> GetByAccountNumberAsync(string accountNumber)
        {
            try
            {
                LogException.LogToFile($"[INFO] Retrieving account with Account Number: {accountNumber}");
                var account = await base.GetByAccountNumberAsync(accountNumber);
                LogException.LogToFile(account != null
                    ? $"[INFO] Retrieved account with Account Number: {accountNumber}"
                    : $"[INFO] No account found with Account Number: {accountNumber}");
                return account!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw; 
            }
        }

        public override async Task<Response> UpdateAsync(BankAccount entity)
        {
            try
            {
                LogException.LogToFile($"[INFO] Updating account with ID: {entity.Id} and Account Number: {entity.AccountNumber}");
                var result = await base.UpdateAsync(entity);
                LogException.LogToFile($"[INFO] Update operation result: {result.Message}");
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
