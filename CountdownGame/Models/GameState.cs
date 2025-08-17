namespace CountdownGame.Models;

public class GameState
{
    public Player Player1 { get; set; } = new();
    public Player Player2 { get; set; } = new();
    public int CurrentRound { get; set; } = 1;
    public int TotalRounds { get; set; } = 6;

    public void ResetRound()
    {
        
    }
}