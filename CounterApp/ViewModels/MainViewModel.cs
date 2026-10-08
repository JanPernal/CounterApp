using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CounterApp.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public ObservableCollection<CounterViewModel> Counters {get; } = new();

    [ObservableProperty]
    private string newCounterName = "";

    [RelayCommand]
    private void AddCounter()
    {
        CounterViewModel newCounter = new();
        newCounter.Name = NewCounterName;
        Counters.Add(newCounter);
        NewCounterName = "";
    }
}