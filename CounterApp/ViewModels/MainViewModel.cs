using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CounterApp.Models;
using CounterApp.Services;

namespace CounterApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
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
    private bool CanAdd() => NewCounterName != "";

    [RelayCommand]
    private async Task Save()
    {
        var storage = new XmlCounterStorage();   // temporary, DI replaces this later
        await storage.SaveAsync(Counters.Select(vm => new Counter { Id = vm.Id
            ,Name = vm.Name
            ,Value = vm.Value
            ,InitialValue = vm.InitialValue
            ,ColorHex = vm.ColorHex }));
    }
}