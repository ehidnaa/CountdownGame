using CountdownGame.Models;
using CountdownGame.Services;

namespace CountdownGame;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 250, Easing.CubicOut);
    }

    private async void OnStartGameClicked(object sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);
        await Navigation.PushAsync(new GamePage());
    }

    private async void OnGoToSettingsClicked(object sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);
        await Navigation.PushAsync(new SettingsPage());
    }
}
