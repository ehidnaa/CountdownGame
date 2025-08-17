using CountdownGame.Services;

namespace CountdownGame;

public partial class HistoryPage : ContentPage
{
    public HistoryPage()
    {
        InitializeComponent();
        _ = LoadAsync();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 250, Easing.CubicOut);
    }

    private async Task LoadAsync()
    {
        HistoryList.ItemsSource = await HistoryService.LoadAsync();
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);

        await LoadAsync();
    }
}