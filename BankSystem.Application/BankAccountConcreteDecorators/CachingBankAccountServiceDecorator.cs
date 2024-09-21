using BankSystem.Application.Constant;
using BankSystem.Application.DecoratorBase;
using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Responses;

namespace BankSystem.Infrastructure.BankAccountConcreteDecorators
{
    public class CachingBankAccountServiceDecorator : BankAccountDecorator
    {
        private readonly IMyCacheService _cacheService;

        public CachingBankAccountServiceDecorator(IBankAccount inner, IMyCacheService cacheService): base(inner)
        {
            _cacheService = cacheService;
        }

        public override async Task<IEnumerable<BankAccount>> GetAllAsync()
        {
           
            var cachedAccounts = _cacheService.GetData<IEnumerable<BankAccount>>(ConstantValues.AllBankAccountsKey);

            if (cachedAccounts != null)
            {
                return cachedAccounts;
            }

            var accounts = await base.GetAllAsync();
            _cacheService.SetData(ConstantValues.AllBankAccountsKey, accounts, DateTimeOffset.Now.AddDays(1));
            return accounts;
        }

        public override async Task<BankAccount> FindByIdAsync(int id)
        {
            var cacheKey = $"Account_{id}";
            var cachedAccount = _cacheService.GetData<BankAccount>(cacheKey);

            if (cachedAccount != null)
            {
                return cachedAccount;
            }

            var account = await base.FindByIdAsync(id);
            if (account != null)
            {
                _cacheService.SetData(cacheKey, account, DateTimeOffset.Now.AddDays(1));
            }
            return account!;
        }

        public override async Task<BankAccount> GetByAccountNumberAsync(string accountNumber)
        {
            var cacheKey = $"Account_{accountNumber}";
            var cachedAccount = _cacheService.GetData<BankAccount>(cacheKey);

            if (cachedAccount != null)
            {
                return cachedAccount;
            }

            var account = await base.GetByAccountNumberAsync(accountNumber);
            if (account != null)
            {
                _cacheService.SetData(cacheKey, account, DateTimeOffset.Now.AddDays(1));
            }
            return account!;
        }

        public override async Task<Response> CreateAsync(BankAccount entity)
        {
            var result = await base.CreateAsync(entity);
            if (result.Flag)
            {
                // Clear cache
                _cacheService.RemoveData(ConstantValues.AllBankAccountsKey);

                _cacheService.RemoveData($"Account_{entity.Id}");

                _cacheService.RemoveData($"Account_{entity.AccountNumber}");
            }
            return result;
        }

        public override async Task<Response> DeleteAsync(BankAccount entity)
        {
            var result = await base.DeleteAsync(entity);
            if (result.Flag)
            {
                _cacheService.RemoveData(ConstantValues.AllBankAccountsKey);

                _cacheService.RemoveData($"Account_{entity.Id}");

                _cacheService.RemoveData($"Account_{entity.AccountNumber}");
            }
            return result;
        }

        public override async Task<Response> UpdateAsync(BankAccount entity)
        {
            var result = await base.UpdateAsync(entity);
            if (result.Flag)
            {
                _cacheService.RemoveData(ConstantValues.AllBankAccountsKey);

                _cacheService.RemoveData($"Account_{entity.Id}");

                _cacheService.RemoveData($"Account_{entity.AccountNumber}");
            }
            return result;
        }
    }
}
