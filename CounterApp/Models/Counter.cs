namespace CounterApp.Models;

public class Counter
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public int Value { get; set; } = 0;
    public int InitialValue { get; set; } = 0;
    public string ColorHex { get; set; } = ColorPalette.BaseColors[3];
}