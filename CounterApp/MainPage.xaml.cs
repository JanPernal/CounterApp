using CounterApp.ViewModels;
using CounterApp.Pages;

namespace CounterApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

    }

    private async void counterBtn1_Clicked(object? sender, EventArgs e)
    {
        var counterViewModel = new CounterViewModel()
        {
            Name = "Counter 1",
            Value = 0,
            InitialValue = 0,
            ColorHex = "#FF2BD4",
        };
        var counterPage = new CounterPage();
        counterPage.BindingContext = counterViewModel;
        await Navigation.PushAsync(counterPage);
    }

    private async void counterBtn2_Clicked(object? sender, EventArgs e)
    {
        var counterViewModel = new CounterViewModel()
        {
            Name = "Counter 2",
            Value = 0,
            InitialValue = 1,
            ColorHex = "#F1200F",
        };

        var counterPage = new CounterPage();
        counterPage.BindingContext = counterViewModel;
        await Navigation.PushAsync(counterPage);
    }

    private async void counterBtn3_Clicked(object? sender, EventArgs e)
    {
        var counterViewModel = new CounterViewModel()
        {
            Name = "Counter 3",
            Value = 0,
            InitialValue = 1,
            ColorHex = "#512BD4",
        };

        var counterPage = new CounterPage();
        counterPage.BindingContext = counterViewModel;
        await Navigation.PushAsync(counterPage);
    }
}