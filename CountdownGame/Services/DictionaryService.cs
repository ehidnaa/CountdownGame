using System.Collections.Concurrent;

namespace CountdownGame.Services;

public static class DictionaryService
{
    private const string Url = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/cdwords.txt";
    private const string FileName = "cdwords.txt";

    private static readonly SemaphoreSlim _gate = new(1, 1);
    private static volatile bool _loaded = false;
    private static HashSet<string>? _words;

    private static string LocalPath =>
        Path.Combine(FileSystem.AppDataDirectory, FileName);

    public static async Task EnsureLoadedAsync()
    {
        if (_loaded) return;
        await _gate.WaitAsync();
        try
        {
            if (_loaded) return;

            // если файла нет — скачиваем и сохраняем
            if (!File.Exists(LocalPath))
            {
                using var http = new HttpClient();
                var txt = await http.GetStringAsync(Url);
                Directory.CreateDirectory(FileSystem.AppDataDirectory);
                await File.WriteAllTextAsync(LocalPath, txt);
            }

            // читаем локально в память
            var lines = await File.ReadAllLinesAsync(LocalPath);
            _words = new HashSet<string>(lines.Select(l => l.Trim().ToLowerInvariant())
                                              .Where(l => l.Length > 0));
            _loaded = true;
        }
        finally
        {
            _gate.Release();
        }
    }

    public static async Task<bool> ContainsAsync(string word)
    {
        if (string.IsNullOrWhiteSpace(word)) return false;
        await EnsureLoadedAsync();
        return _words!.Contains(word.Trim().ToLowerInvariant());
    }
}
