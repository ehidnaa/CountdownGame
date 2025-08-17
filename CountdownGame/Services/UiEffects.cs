namespace CountdownGame.Services;

public static class UiEffects
{
    public static async Task PressAsync(View v)
    {
        await v.ScaleTo(0.96, 80, Easing.SinOut);
        await v.ScaleTo(1.0, 100, Easing.SinIn);
    }
}
