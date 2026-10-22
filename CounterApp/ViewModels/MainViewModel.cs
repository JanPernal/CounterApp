using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CounterApp.Services;

namespace CounterApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ICounterStorage _storage;

    public MainViewModel(ICounterStorage storage)
    {
        _storage = storage;
    }
    public ObservableCollection<CounterViewModel> Counters {get; } = new();

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
}