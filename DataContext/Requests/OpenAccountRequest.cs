using Core.Models;

namespace DataContext.Requests
{
    public record struct OpenAccountRequest
    {
        public long CustomerId { get; set; }
        public double InitialDeposit { get; set; }
        public int AccountTypeId { get; set; }

        public readonly void Deconstruct(out long customerId, out double initialDeposit, out int accountTypeId)
        {
            customerId = CustomerId;
            initialDeposit = InitialDeposit;
            accountTypeId = AccountTypeId;
        }
    }
}
