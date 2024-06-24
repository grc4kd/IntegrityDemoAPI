public class EnvironmentFixture : IDisposable
{
    public EnvironmentFixture()
    {
        Environment.SetEnvironmentVariable("DefaultCurrencyCode", "USD");
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
    {
        Environment.SetEnvironmentVariable("DefaultCurrencyCode", null);
    }
}