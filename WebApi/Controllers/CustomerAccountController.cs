using Microsoft.AspNetCore.Mvc;
using DataContext.Repositories;
using DataContext.Responses;
using DataContext.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using DataContext.Requests;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerAccountController(ICustomerAccountRepository repository) : ControllerBase
    {
        private readonly ICustomerAccountRepository _repository = repository;
        private static readonly int MaxAccountListLength = 100;

        // GET: api/Account
        [HttpGet]
        public IEnumerable<CustomerAccount> GetCustomerAccounts()
        {
            return _repository.GetCustomerAccounts(MaxAccountListLength);
        }

        // GET: api/Account/5
        [HttpGet("{accountId}")]
        public CustomerAccount? GetCustomerAccount(int accountId)
        {
            return _repository.GetCustomerAccount(accountId);
        }

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("OpenAccount")]
        public OpenAccountResponse OpenCustomerAccount(OpenAccountRequest openAccountRequest)
        {
            return _repository.OpenCustomerAccount(openAccountRequest);
        }

        // POST: api/Account/MakeDeposit
        /// <summary>
        /// Facilitate a request to deposit a dollar amount in a customers account.
        /// </summary>
        /// <returns><see cref="DepositResponse"/>a JSON response with deposit transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("MakeDeposit")]
        public IAccountResponse MakeDeposit(DepositRequest depositRequest)
        {
            return _repository.MakeDeposit(depositRequest);
        }

        // POST: api/Account/MakeDeposit
        /// <summary>
        /// Facilitate a request to deposit a dollar amount in a customers account.
        /// </summary>
        /// <returns><see cref="WithdrawalResponse"/>a JSON response with withdrawal transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("MakeWithdrawal")]
        public IAccountResponse MakeWithdrawal(WithdrawalRequest withdrawalRequest)
        {
            return _repository.MakeWithdrawal(withdrawalRequest);
        }

        // POST: api/Account/CloseAccount
        /// <summary>
        /// Facilitate a request to close a customers account.
        /// </summary>
        /// <returns><see cref="CloseAccountResponse"/>a JSON response with withdrawal transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("CloseAccount")]
        public CloseAccountResponse CloseCustomerAccount(CloseAccountRequest closeAccountRequest)
        {
            return _repository.CloseCustomerAccount(closeAccountRequest);
        }
    }
}
