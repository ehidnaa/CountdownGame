using CountdownGame.Models;
using CountdownGame.Services;

namespace CountdownGame;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void OnStartGameClicked(object sender, EventArgs e)
    {
        var p1 = string.IsNullOrWhiteSpace(Player1Entry.Text) ? "Player 1" : Player1Entry.Text.Trim();
        var p2 = string.IsNullOrWhiteSpace(Player2Entry.Text) ? "Player 2" : Player2Entry.Text.Trim();

        GameSession.Current = new GameState
        {
            Player1 = new Player { Name = p1 },
            Player2 = new Player { Name = p2 }
        };

        
        Shell.Current.CurrentItem = Shell.Current.Items[0].Items[1];
    }
}
