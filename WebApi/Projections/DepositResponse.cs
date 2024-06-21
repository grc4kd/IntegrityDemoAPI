namespace IntegrityDemoAPI.Projections
{
    public class DepositResponse
    {
        public long customerId;
        public long accountId;
        public double balance;
        public bool succeeded = false;
    }
}