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
    public class AccountController : ControllerBase
    {
        private readonly AccountContext _context;
        private readonly AccountRepository _repository;

        public AccountController(AccountContext context)
        {
            _context = context;
            _repository = new AccountRepository(context);
        }


        // POST: api/Account/MakeDeposit
        /// <summary>
        /// Facilitate a request to deposit a dollar amount in a customers account.
        /// </summary>
        /// <returns><see cref="DepositResponse"/>a JSON response with deposit transaction data</returns>
        [ProducesErrorResponseType(typeof(BadRequest))]
        [HttpPost("MakeDeposit")]
        public async Task<ActionResult<DepositResponse>> MakeDeposit(DepositRequest depositRequest) {
            var customer = await _repository.GetAccountAsync(depositRequest.AccountId);

            if (customer.Id != depositRequest.CustomerId) {
                return BadRequest($"Customer ID does not match account for ${nameof(DepositRequest)}.");
            }

            return await _repository.MakeDepositAsync(customer, (decimal)depositRequest.Amount);
        }

        // GET: api/Account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerAccount>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAccount>> GetCustomerAccountAsync(long id)
        {
            var customerAccount = await _context.Accounts.FindAsync(id);

            if (customerAccount == null)
            {
                return NotFound();
            }

            return customerAccount;
        }

        // PUT: api/Account/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAccount(long id, CustomerAccount customerAccount)
        {
            if (id != customerAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(customerAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Account
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CustomerAccount>> PostCustomerAccount(CustomerAccount customerAccount)
        {
            _context.Accounts.Add(customerAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerAccount", new { id = customerAccount.Id }, customerAccount);
        }

        // DELETE: api/Account/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAccount(long id)
        {
            var customerAccount = await _context.Accounts.FindAsync(id);
            if (customerAccount == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(customerAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerAccountExists(long id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}
