using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CounterApp.Models;

namespace CounterApp.ViewModels;

public partial class CounterViewModel : ObservableObject
{
    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ResetCommand))]
    private int initialValue;

    [ObservableProperty] private string name = "";

    [ObservableProperty] [NotifyCanExecuteChangedFor(nameof(ResetCommand))]
    private int value;

    public Guid Id { get; set; } = Guid.NewGuid();
    public string ColorHex { get; set; } = ColorPalette.Default;
    public event Action? Changed;

    [RelayCommand]
    private void Increment()
    {
        Value++;
    }

    [RelayCommand]
    private void Decrement()
    {
        Value--;
    }

    [RelayCommand(CanExecute = nameof(CanReset))]
    private async Task Reset()
    {
        var confirmed = await Shell.Current.DisplayAlertAsync(
            "Reset counter", $"Reset \"{Name}\"?", "Reset", "Cancel");
        if (!confirmed) return;
        Value = InitialValue;
    }

    private bool CanReset()
    {
        return Value != InitialValue;
    }

    public Counter ToModel()
    {
        return new Counter { Id = Id, Name = Name, Value = Value, InitialValue = InitialValue, ColorHex = ColorHex };
    }

    public static CounterViewModel FromModel(Counter c)
    {
        return new CounterViewModel
            { Id = c.Id, Name = c.Name, Value = c.Value, InitialValue = c.InitialValue, ColorHex = c.ColorHex };
    }

    partial void OnValueChanged(int value)
    {
        Changed?.Invoke();
    }
}