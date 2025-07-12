namespace ProjectPortfolio.Global
{
    public class GameRegistry : Registry
    {
        public static GameRegistry Instance { get; private set; } 

        internal static void Initialize()
        {
            Asserts.IsNull(Instance);
            Instance = new GameRegistry();
        }
    }
}