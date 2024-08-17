public static class Game
{
    public static bool IsInitialized { get; private set; }
    
    public static void Initialize()
    {
        IsInitialized = true;
    }
}
