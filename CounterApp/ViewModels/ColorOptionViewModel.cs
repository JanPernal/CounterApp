using CommunityToolkit.Mvvm.ComponentModel;

namespace CounterApp.ViewModels;

public partial class ColorOptionViewModel : ObservableObject
{
    [ObservableProperty] private bool isSelected;
    public required string Hex { get; init; }
}