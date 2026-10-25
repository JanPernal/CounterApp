using CommunityToolkit.Mvvm.ComponentModel;

namespace CounterApp.ViewModels;

public partial class ColorOptionViewModel : ObservableObject
{
    public required string Hex { get; init; }
    [ObservableProperty] private bool isSelected;
}