using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataContext;
using DataContext.Repositories;
using DataContext.Responses;
using DataContext.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using DataContext.Requests;
using Core.Currency;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly USD usdCurrency = new USD();
        private readonly AccountRepository _repository;

        public AccountController(DbContextOptions<AccountContext> options)
        {
            _repository = new AccountRepository(options);
        }

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
        [HttpPost]
        public ActionResult<CustomerAccount> PostCustomerAccount(CustomerAccount customerAccount)
        {
            _repository.AddAccount(customerAccount);

            return CreatedAtAction("GetCustomerAccount", new { id = customerAccount.Id }, customerAccount);
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
            var decimalDepositAmount = (decimal)depositRequest.Amount;
            if (decimalDepositAmount.Scale > usdCurrency.MinimumDenomination.Scale) 
            {
                return BadRequest($"Deposit amount: {depositRequest.Amount} has fractional value less than the minimum denomination for {usdCurrency.CurrencyCode}");
            }

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
            var decimalDepositAmount = (decimal)depositRequest.Amount;
            if (decimalDepositAmount.Scale > usdCurrency.MinimumDenomination.Scale) 
            {
                return BadRequest($"Withdrawal amount: {depositRequest.Amount} has fractional value less than the minimum denomination for {usdCurrency.CurrencyCode}");
            }

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
