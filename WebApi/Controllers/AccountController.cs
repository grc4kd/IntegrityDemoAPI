using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataContext;
using DataContext.Repositories;
using DataContext.Responses;
using DataContext.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using DataContext.Requests;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(DbContextOptions<AccountContext> options) : ControllerBase
    {
        private readonly AccountRepository _repository = new AccountRepository(options);

        // GET: api/Account
        [HttpGet]
        public ActionResult<IEnumerable<CustomerAccount>> GetAccounts()
        {
            return _repository.GetAccountList();
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<CustomerAccount> GetCustomerAccountAsync(long id)
        {
            var customerAccount = _repository.GetAccount(id);

            if (customerAccount == null)
            {
                return NotFound();
            }

            return customerAccount;
        }

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutCustomerAccount(long id, CustomerAccount customerAccount)
        {
            if (id != customerAccount.Id)
            {
                return BadRequest();
            }

            var result = _repository.PutCustomerAccount(id, customerAccount);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("OpenAccount")]
        public ActionResult<CustomerAccount> OpenCustomerAccount(OpenAccountRequest request)
        {
            var openAccountResponse = _repository.OpenAccount(request);

            return CreatedAtAction("GetCustomerAccount", new { id = openAccountResponse.CustomerId }, openAccountResponse);
        }

        // POST: api/Account/MakeDeposit
        /// <summary>
        /// Facilitate a request to deposit a dollar amount in a customers account.
        /// </summary>
        /// <returns><see cref="DepositResponse"/>a JSON response with deposit transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("MakeDeposit")]
        public ActionResult<DepositResponse> MakeDeposit(DepositRequest depositRequest)
        {
            var account = _repository.GetAccount(depositRequest.AccountId);

            if (account == null)
            {
                return NotFound($"Could not find customer account, request: ${depositRequest}");
            }

            if (account.Customer.Id != depositRequest.CustomerId)
            {
                return BadRequest($"Customer ID does not match account for {nameof(DepositRequest)}.");
            }

            return _repository.MakeDeposit(account, (decimal)depositRequest.Amount);
        }

        // POST: api/Account/MakeDeposit
        /// <summary>
        /// Facilitate a request to deposit a dollar amount in a customers account.
        /// </summary>
        /// <returns><see cref="WithdrawalResponse"/>a JSON response with withdrawal transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("MakeWithdrawal")]
        public ActionResult<WithdrawalResponse> MakeWithdrawal(DepositRequest depositRequest)
        {
            var account = _repository.GetAccount(depositRequest.AccountId);

            if (account == null)
            {
                return NotFound($"Could not find customer account, request: {depositRequest}");
            }

            if (account.Customer.Id != depositRequest.CustomerId)
            {
                return BadRequest($"Customer ID does not match account for {nameof(DepositRequest)}.");
            }

            return _repository.MakeWithdrawal(account, (decimal)depositRequest.Amount);
        }

        // POST: api/Account/CloseAccount
        /// <summary>
        /// Facilitate a request to close a customers account.
        /// </summary>
        /// <returns><see cref="CloseAccountResponse"/>a JSON response with withdrawal transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("CloseAccount")]
        public ActionResult<CloseAccountResponse> CloseAccount(CloseAccountRequest request)
        {
            var account = _repository.GetAccount(request.AccountId);

            if (account == null)
            {
                return NotFound($"Could not find customer account, request: {request}");
            }

            if (account.Customer.Id != request.CustomerId)
            {
                return BadRequest($"Customer ID does not match account for {nameof(DepositRequest)}.");
            }

            if (account.Balance != 0) {
                return BadRequest("Account can only be closed if the balance is exactly 0.");
            }

            return _repository.CloseAccount(request.AccountId);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomerAccount(long id)
        {
            var result = _repository.DeleteAccount(id);

            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
