using CountdownGame.Models;

namespace CountdownGame.Services;

public static class GameSession
{
    public static GameState Current { get; set; } = new();
}