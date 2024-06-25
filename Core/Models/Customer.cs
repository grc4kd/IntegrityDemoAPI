namespace Core.Models;

public class Customer(int id = 0, string name = "")
{
    public int Id { get; set; } = id;
    public string Name { get; private set; } = name;
}
