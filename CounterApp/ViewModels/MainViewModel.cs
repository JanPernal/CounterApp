using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CounterApp.Models;
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

    [RelayCommand(CanExecute = nameof(CanAdd))]
    private void AddCounter()
    {
        CounterViewModel newCounter = new();
        newCounter.Name = NewCounterName;
        Counters.Add(newCounter);
        NewCounterName = "";
    }
    private bool CanAdd() => !string.IsNullOrWhiteSpace(NewCounterName);

    [RelayCommand]
    private async Task Save()
    {
        await _storage.SaveAsync(Counters.Select(vm => vm.ToModel()));
    }

    [RelayCommand]
    private async Task Load()
    {
        var counters = await _storage.LoadAsync();
        Counters.Clear();
        foreach (var counter in counters)
        {
            Counters.Add(CounterViewModel.FromModel(counter));
        }
    }
}