namespace CounterApp.Models;

public class Counter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public int Value { get; set; }
    public int InitaialValue { get; set; }
    public string ColorHex { get; set; } = "#512BD4";
}