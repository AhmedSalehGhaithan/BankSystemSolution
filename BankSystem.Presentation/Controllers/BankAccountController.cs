using BankSystem.Application.Interface;
using BankSystem.Domain.Entities;
using BankSystem.SharedLibrarySolution.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BankSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController(IBankAccount _bankAccountService) : ControllerBase
    {
        [HttpGet("GetAccounts")]
        public async Task<ActionResult<IEnumerable<BankAccount>>> GetAccounts()
           =>  Ok(await _bankAccountService.GetAllAsync());

        [HttpPost("Create/Account")]
        public async Task<ActionResult<Response>> CreateAccount(BankAccount accountDTO) 
            => Ok(await _bankAccountService.CreateAsync(accountDTO));


        [HttpGet("Get/Account/{id:int}")]
        public async Task<ActionResult<BankAccount>> GetAccountById(int id) 
            => Ok(await _bankAccountService.FindByIdAsync(id));

        [HttpGet("GetByAccountNumber/{accountNumber}")]
        public async Task<ActionResult<BankAccount>> GetByAccountNumber(string accountNumber)
            => Ok(await _bankAccountService.GetByAccountNumberAsync(accountNumber));

        [HttpPut("Update/Account")]
        public async Task<ActionResult<Response>> UpdateBankAccount(BankAccount account)
            => Ok(await _bankAccountService.UpdateAsync(account));

        [HttpDelete("Delete/Account/{accountId:int}")]
        public async Task<ActionResult<Response>> DeleteBankAccount(int accountId)
        {
            var account = await _bankAccountService.FindByIdAsync(accountId);

            var response = await _bankAccountService.DeleteAsync(account);
            return Ok(response);
        }
    }
}
