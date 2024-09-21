#region Transaction Controller
using BankSystem.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using BankSystem.SharedLibrarySolution.Responses;
using BankSystem.Domain.Entities;

namespace BankSystem.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(ITransaction _transaction,IMyCacheService _cacheService) : ControllerBase
    {
        [HttpGet("Get/Transactions")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
            => Ok(await _transaction.GetAllAsync());

        [HttpPost("Create/Transaction")]
        public async Task<ActionResult<Response>> CreateTransaction(Transaction transaction)
            => Ok(await _transaction.CreateAsync(transaction));

        [HttpGet("Get/Transaction/{id:int}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
            => Ok(await _transaction.FindByIdAsync(id));

        [HttpGet("GetByAccountNumber/{accountNumber}")]
        public async Task<ActionResult<Transaction>> GetByAccountNumber(string accountNumber)
            => Ok(await _transaction.GetTransactionsByBankAccountNumberAsync(accountNumber));
      
        [HttpPost("Deposit/Transaction")]
        public async Task<ActionResult<Response>> Deposit(Transaction transaction)
            => Ok(await _transaction.DepositAsync(transaction));

        [HttpPost("Withdraw/Transaction")]
        public async Task<ActionResult<Response>> Withdraw(Transaction transaction)
            => Ok(await _transaction.WithdrawAsync(transaction));

        [HttpPost("Transfer/Transaction")]
        public async Task<ActionResult<Response>> Transfer(Transaction transaction)
            => Ok(await _transaction.TransferAsync(transaction));

        [HttpDelete("Delete/Transaction/{transactionId:int}")]
        public async Task<ActionResult<Response>> DeleteTransaction(int transactionId)
        {
            var transaction = await _transaction.FindByIdAsync(transactionId);

            var response = await _transaction.DeleteAsync(transaction);

            return Ok(response);
        }
    }
}


#endregion