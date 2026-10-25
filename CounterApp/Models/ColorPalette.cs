namespace CounterApp.Models;

public static class ColorPalette
{
    public static string Default => BaseColors[5];

    public static IReadOnlyList<string> BaseColors { get; } =
    [
        "#E40000",
        "#E46A00",
        "#E4C500",
        "#4AC200",
        "#0A9488",
        "#0090E4",
        "#2E00E4",
        "#6E00E4",
        "#AF00E4",
        "#E400AF"
    ];
}