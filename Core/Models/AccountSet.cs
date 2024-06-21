namespace Core.Models;

public record AccountSet : IAccountSet {
    private Dictionary<long, CustomerAccount> accountDict = new Dictionary<long, CustomerAccount>();

    public CustomerAccount GetAccount(long id) {
        if (!accountDict.TryGetValue(id, out CustomerAccount? value)) {
            throw new ArgumentOutOfRangeException(nameof(id), "ID could not be found in " + GetType().Name);
        }

        return value;
    }
}