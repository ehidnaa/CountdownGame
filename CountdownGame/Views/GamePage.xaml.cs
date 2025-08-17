using System.Xml.Linq;
using CountdownGame.Models;
using CountdownGame.Services;
using GameKit;
using Microsoft.Maui.Controls;

namespace CountdownGame;

public partial class GamePage : ContentPage
{
    private readonly List<char> _picked = new(); // chosen letters (max 9)
    private CancellationTokenSource? _timerCts;
    private GameState _state => GameSession.Current;

    public GamePage()
    {
        InitializeComponent();

        // show names & scores
        P1Name.Text = _state.Player1.Name;
        P2Name.Text = _state.Player2.Name;
        P1Score.Text = _state.Player1.Score.ToString();
        P2Score.Text = _state.Player2.Score.ToString();

        RenderLetters();
    }

    private void RenderLetters()
    {
        LettersPanel.Children.Clear();
        foreach (var ch in _picked)
        {
            LettersPanel.Children.Add(new Frame
            {
                Padding = 8,
                CornerRadius = 8,
                Content = new Label { Text = ch.ToString(), FontSize = 20, HorizontalOptions = LayoutOptions.Center }
            });
        }

        // block choose when 9 letters picked
        bool canPick = _picked.Count < 9;
        ConsonantBtn.IsEnabled = canPick;
        VowelBtn.IsEnabled = canPick;
        StartBtn.IsEnabled = _picked.Count == 9;
    }

    private void OnConsonantClicked(object? sender, EventArgs e)
    {
        if (_picked.Count >= 9) return;
        _picked.Add(LetterService.PickConsonant());
        RenderLetters();
    }

    private void OnVowelClicked(object? sender, EventArgs e)
    {
        if (_picked.Count >= 9) return;
        _picked.Add(LetterService.PickVowel());
        RenderLetters();
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        if (_picked.Count != 9)
        {
            await DisplayAlert("Info", "Pick 9 letters first.", "OK");
            return;
        }

        // disable pick, start 30s
        ConsonantBtn.IsEnabled = VowelBtn.IsEnabled = StartBtn.IsEnabled = false;
        Word1Entry.IsEnabled = Word2Entry.IsEnabled = false;
        CheckBtn.IsEnabled = false;

        _timerCts?.Cancel();
        _timerCts = new CancellationTokenSource();

        for (int t = 30; t >= 0; t--)
        {
            TimerLabel.Text = t.ToString();
            try
            {
                await Task.Delay(1000, _timerCts.Token);
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        // enable word input after timer
        Word1Entry.IsEnabled = Word2Entry.IsEnabled = true;
        CheckBtn.IsEnabled = true;
    }

    private async void OnCheckWordsClicked(object? sender, EventArgs e)
    {
        // now just notification
        await DisplayAlert("Next step", "Dictionary check and scoring will be added next.", "OK");
    }
}