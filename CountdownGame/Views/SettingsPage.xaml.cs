using CountdownGame.Services;

namespace CountdownGame;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    private async void OnBackgroundChanged(object sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);

        var selected = BackgroundPicker.SelectedItem?.ToString();

        if (selected == "Light")
        {
            Application.Current.Resources["AppBackground"] = new SolidColorBrush(Colors.White);
        }
        else if (selected == "Dark")
        {
            Application.Current.Resources["AppBackground"] = new SolidColorBrush(Colors.Black);
        }
        else if (selected == "Blue Gradient")
        {
            Application.Current.Resources["AppBackground"] = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
            {
                new GradientStop(Colors.DarkBlue, 0.0f),
                new GradientStop(Colors.LightBlue, 1.0f)
            }
            };
        }
        else if (selected == "Purple Gradient")
        {
            Application.Current.Resources["AppBackground"] = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromArgb("#8E2DE2"), 0.0f),
                new GradientStop(Color.FromArgb("#4A00E0"), 1.0f)
            }
            };
        }
        else if (selected == "Fire Gradient")
        {
            Application.Current.Resources["AppBackground"] = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromArgb("#FF512F"), 0.0f),
                new GradientStop(Color.FromArgb("#F09819"), 1.0f)
            }
            };
        }
        else if (selected == "Green Gradient")
        {
            Application.Current.Resources["AppBackground"] = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
            {
                new GradientStop(Color.FromArgb("#11998E"), 0.0f),
                new GradientStop(Color.FromArgb("#38EF7D"), 1.0f)
            }
            };
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 250, Easing.CubicOut);
    }
}