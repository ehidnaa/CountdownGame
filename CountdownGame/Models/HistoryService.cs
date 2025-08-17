using System.Text.Json;
using CountdownGame.Models;

namespace CountdownGame.Services;

public static class HistoryService
{
    private const string FileName = "history.json";
    private static string PathFull => System.IO.Path.Combine(FileSystem.AppDataDirectory, FileName);

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true
    };

    public static async Task<List<GameRecord>> LoadAsync()
    {
        if (!File.Exists(PathFull)) return new List<GameRecord>();
        var json = await File.ReadAllTextAsync(PathFull);
        return JsonSerializer.Deserialize<List<GameRecord>>(json, _jsonOptions) ?? new();
    }

    public static async Task AppendAsync(GameRecord rec)
    {
        var list = await LoadAsync();
        list.Add(rec);
        var json = JsonSerializer.Serialize(list, _jsonOptions);
        Directory.CreateDirectory(FileSystem.AppDataDirectory);
        await File.WriteAllTextAsync(PathFull, json);
    }
}
