namespace CountdownGame.Models;

public class GameRecord
{
    public DateTimeOffset Timestamp { get; set; }
    public string Player1Name { get; set; } = "";
    public string Player2Name { get; set; } = "";
    public int Player1Score { get; set; }
    public int Player2Score { get; set; }
}
