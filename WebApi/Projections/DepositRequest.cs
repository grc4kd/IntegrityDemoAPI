namespace IntegrityDemoAPI.Projections
{
    public record Deposit
    {
        public long customerId;
        public long accountId;
        public double amount;
    }
}