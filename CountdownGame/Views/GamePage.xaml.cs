using System.Xml.Linq;
using CountdownGame.Models;
using CountdownGame.Services;
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

    private async void OnConsonantClicked(object? sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);

        if (_picked.Count >= 9) return;
        _picked.Add(LetterService.PickConsonant());
        RenderLetters();
    }

    private async void OnVowelClicked(object? sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);

        if (_picked.Count >= 9) return;
        _picked.Add(LetterService.PickVowel());
        RenderLetters();
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);

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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.FadeTo(1, 250, Easing.CubicOut);
    }

    private async void OnCheckWordsClicked(object? sender, EventArgs e)
    {
        await UiEffects.PressAsync((View)sender);

        var w1 = (Word1Entry.Text ?? "").Trim();
        var w2 = (Word2Entry.Text ?? "").Trim();

        bool w1fits = WordValidator.FitsLetters(w1, _picked);
        bool w2fits = WordValidator.FitsLetters(w2, _picked);

        bool w1ok = w1fits && await DictionaryService.ContainsAsync(w1);
        bool w2ok = w2fits && await DictionaryService.ContainsAsync(w2);

        int len1 = w1ok ? w1.Length : 0;
        int len2 = w2ok ? w2.Length : 0;

        string msg =
            $"P1: {(w1ok ? w1 : $"{w1} (invalid)")} → {len1} pts\n" +
            $"P2: {(w2ok ? w2 : $"{w2} (invalid)")} → {len2} pts";

        await DisplayAlert("Round results", msg, "OK");

        if (len1 > len2) _state.Player1.Score += len1;
        else if (len2 > len1) _state.Player2.Score += len2;
        else if (len1 == len2 && len1 > 0)
        {
            _state.Player1.Score += len1;
            _state.Player2.Score += len2;
        }

        P1Score.Text = _state.Player1.Score.ToString();
        P2Score.Text = _state.Player2.Score.ToString();

        _state.CurrentRound++;
        if (_state.CurrentRound <= _state.TotalRounds)
        {           
            _picked.Clear();
            RenderLetters();
            TimerLabel.Text = "30";
            Word1Entry.Text = "";
            Word2Entry.Text = "";
            Word1Entry.IsEnabled = false;
            Word2Entry.IsEnabled = false;
            CheckBtn.IsEnabled = false;
            ConsonantBtn.IsEnabled = true;
            VowelBtn.IsEnabled = true;
            StartBtn.IsEnabled = false;
        }
        else
        {
            await EndGameAsync();
        }
    }

    private async Task EndGameAsync()
    {
        string winner;
        if (_state.Player1.Score > _state.Player2.Score) winner = $"{_state.Player1.Name} wins!";
        else if (_state.Player2.Score > _state.Player1.Score) winner = $"{_state.Player2.Name} wins!";
        else winner = "Draw!";

        await DisplayAlert("Game over",
            $"{_state.Player1.Name}: {_state.Player1.Score}\n" +
            $"{_state.Player2.Name}: {_state.Player2.Score}\n\n{winner}",
            "OK");

        await HistoryService.AppendAsync(new Models.GameRecord
        {
            Timestamp = DateTimeOffset.Now,
            Player1Name = _state.Player1.Name,
            Player2Name = _state.Player2.Name,
            Player1Score = _state.Player1.Score,
            Player2Score = _state.Player2.Score
        });

        await Shell.Current.GoToAsync("//Home");
    }

}