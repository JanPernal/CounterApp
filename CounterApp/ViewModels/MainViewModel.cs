using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CounterApp.Models;
using CounterApp.Services;

namespace CounterApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ICounterStorage _storage;

    public IReadOnlyList<ColorOptionViewModel> AvailableColors { get; } =
        ColorPalette.BaseColors
            .Select(c => new ColorOptionViewModel { Hex = c, IsSelected = c == ColorPalette.Default })
            .ToList();

    public MainViewModel(ICounterStorage storage)
    {
        _storage = storage;
    }
    public ObservableCollection<CounterViewModel> Counters {get; } = new();

    [ObservableProperty]
    private string newColorHex = ColorPalette.Default;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCounterCommand))]
    private string newCounterName = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddCounterCommand))]
    private string newInitialValue = "";

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private async Task AddCounter()
    {
        CounterViewModel newCounter = new();
        newCounter.Name = NewCounterName;
        TryGetInitialValue(out int parsedInitialValue);
        newCounter.Value = newCounter.InitialValue = parsedInitialValue;
        newCounter.ColorHex = NewColorHex;
        NewInitialValue = "";
        NewCounterName = "";
        AddToList(newCounter);
        await SaveAsync();
    }
    private bool CanAdd() => !string.IsNullOrWhiteSpace(NewCounterName) && TryGetInitialValue(out _);

    private async Task SaveAsync() => await _storage.SaveAsync(Counters.Select(vm => vm.ToModel()));

    [RelayCommand]
    private async Task Load()
    {
        var counters = await _storage.LoadAsync();
        Counters.Clear();
        foreach (var counter in counters)
        {
            AddToList(CounterViewModel.FromModel(counter));
        }
    }

    private void AddToList(CounterViewModel counter)
    {
        counter.Changed += OnCounterChanged;
        Counters.Add(counter);
    }

    private async void OnCounterChanged()
    {
        try
        {
            await SaveAsync();
        }
        catch (Exception ex)
        {
            Debug.Write(ex.ToString());
        }

    }

    private bool TryGetInitialValue(out int value)
    {
        if (string.IsNullOrWhiteSpace(NewInitialValue))
        {
            value = 0;
            return true;
        }
        return int.TryParse(NewInitialValue, out value);
    }

    [RelayCommand]
    private async Task DeleteCounter(CounterViewModel counter)
    {
        bool confirmed = await Shell.Current.DisplayAlertAsync(
            "Delete counter", $"Delete \"{counter.Name}\"?", "Delete", "Cancel");
        if (!confirmed) return;
        counter.Changed -= OnCounterChanged;
        Counters.Remove(counter);
        await SaveAsync();
    }

    [RelayCommand]
    private void SelectColor(ColorOptionViewModel option)
    {
        NewColorHex = option.Hex;
        foreach (var color in AvailableColors)
        {
            color.IsSelected = color == option;
        }
    }
}