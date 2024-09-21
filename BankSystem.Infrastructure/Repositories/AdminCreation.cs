using BankSystem.Application.Constant;
using BankSystem.Application.DTOs.AccountDTOs.Request.Account;
using Ghaithan.Administration.ConstantValues;
using BankSystem.Application.Interface;
namespace BankSystem.Infrastructure.Repositories
{
    public class AdminCreation : IAdminConstants
    {
        public string Name => Administration.Name;
        public string Email => Administration.Email;
        public string Password => Administration.Password;
        public CreateAccountDTO PrepearAdmin()
        {
            return new CreateAccountDTO()
            {
                Name = Administration.Name,
                EmailAddress = Administration.Email,
                Password = Administration.Password,
                Role = ConstantValues.Role.Admin,
                ConfirmPassword = Administration.Password
            };
        }
    }

}
