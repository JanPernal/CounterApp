namespace CounterApp;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();
        RenderCounter();
    }

    private void OnAddBtnClicked(object? sender, EventArgs e)
    {
        count++;
        RenderCounter();
    }
    private void OnMinusBtnClicked(object? sender, EventArgs e)
    {
        count--;
        RenderCounter();
    }
    private void RenderCounter()
    {
        CounterLabel.Text = $"Counter: {count}";
        SemanticScreenReader.Announce(CounterLabel.Text);
    }
}