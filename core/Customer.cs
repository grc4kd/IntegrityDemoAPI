namespace core;

public class Customer(string name)
{
    private string Name { get; set; } = name;

    public void ChangeName(string name)
    {
        Name = name;
    }
}
