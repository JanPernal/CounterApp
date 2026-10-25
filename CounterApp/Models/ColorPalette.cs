namespace CounterApp.Models;

public static class ColorPalette
{
    public const int Count = 20;
    public static string Default => BaseColors[12];
    public static IReadOnlyList<string> BaseColors { get; }
        = Enumerable.Range(0, Count).Select(i => Color.FromHsla(i / (double)Count, 1.0, 0.45).ToArgbHex()).ToList();
}