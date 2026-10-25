using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CounterApp.Models;

namespace CounterApp.ViewModels;


public partial class CounterViewModel : ObservableObject
{
    public event Action? Changed;
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ColorHex { get; set; } = ColorPalette.Default;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ResetCommand))]
    private int initialValue;

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
    public Counter ToModel() => new Counter {Id = Id, Name = Name, Value = Value, InitialValue = InitialValue, ColorHex = ColorHex};
    public static CounterViewModel FromModel(Counter c) => new CounterViewModel {Id = c.Id, Name = c.Name, Value = c.Value, InitialValue = c.InitialValue, ColorHex = c.ColorHex};

    partial void OnValueChanged(int value)
    {
        Changed?.Invoke();
    }
}