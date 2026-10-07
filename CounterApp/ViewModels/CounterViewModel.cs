using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CounterApp.ViewModels;


public partial class CounterViewModel : ObservableObject
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int InitialValue { get; set; }
    public string ColorHex { get; set; } = "#512BD4";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResetCommand))]
    private int value;

    [ObservableProperty]
    private string name = "";

    [RelayCommand]
    private void Increment() => Value++;

    [RelayCommand]
    private void Decrement() => Value--;

    [RelayCommand(CanExecute = nameof(CanReset))]
    private void Reset() => Value = InitialValue;

    private bool CanReset() => Value != InitialValue;
}