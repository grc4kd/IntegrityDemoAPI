namespace Core.Models;

public class Customer(long id = 0, string name = "")
{
    public long Id { get; set; } = id;
    public string Name { get; private set; } = name;
}
