namespace CountdownGame.Services;

public static class WordValidator
{
    
    public static bool FitsLetters(string word, IEnumerable<char> picked)
    {
        if (string.IsNullOrWhiteSpace(word)) return false;

        var need = new Dictionary<char, int>();
        foreach (var ch in word.Trim().ToUpperInvariant())
        {
            if (ch < 'A' || ch > 'Z') return false;
            if (!need.TryAdd(ch, 1))
                need[ch]++;
        }

        var have = new Dictionary<char, int>();
        foreach (var ch in picked.Select(c => char.ToUpperInvariant(c)))
        {
            if (!have.TryAdd(ch, 1))
                have[ch]++;
        }

        foreach (var kv in need)
        {
            if (!have.TryGetValue(kv.Key, out var cnt) || cnt < kv.Value)
                return false;
        }
        return true;
    }
}
