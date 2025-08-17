namespace CountdownGame.Services;

public static class LetterService
{
    private static readonly char[] VowelsBag;   
    private static readonly char[] Consonants = "BCDFGHJKLMNPQRSTVWXYZ".ToCharArray();

    static LetterService()
    {
        var list = new List<char>(67);
        list.AddRange(Enumerable.Repeat('A', 15));
        list.AddRange(Enumerable.Repeat('E', 21));
        list.AddRange(Enumerable.Repeat('I', 13));
        list.AddRange(Enumerable.Repeat('O', 13));
        list.AddRange(Enumerable.Repeat('U', 5));
        VowelsBag = list.ToArray();
    }

    public static char PickVowel()
    {
        var idx = Random.Shared.Next(VowelsBag.Length);
        return VowelsBag[idx];
    }

    public static char PickConsonant()
    {
        var idx = Random.Shared.Next(Consonants.Length);
        return Consonants[idx];
    }
}
