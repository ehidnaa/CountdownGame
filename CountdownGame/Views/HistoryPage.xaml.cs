using CountdownGame.Services;

namespace CountdownGame;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
        _ = LoadAsync();
    }

    private async Task LoadAsync()
    {
        HistoryList.ItemsSource = await HistoryService.LoadAsync();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadAsync();
    }
}