namespace Core.Models;

public interface IAccountSet
{
    public CustomerAccount GetAccount(long id);
}