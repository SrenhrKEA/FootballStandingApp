namespace Utilities
{
    public static class Constants
    {
        public const int MaxNumberOfRoundsInASeason = 32;
        public const int NumberOfRoundsInRegularSeason = 22;
        public const int ChampionshipPositionLimit = 6;
        public const int GoalLimit = 6; // for the sake of simplicity, the limit is 5 goals per team per match.

        // Define file paths as patterns to be used across classes
        public const string BaseDataPath = "./data";
        public const string BaseRoundsPath = "./data/rounds";
        public static string GetSetupFilePath() => Path.Combine(BaseDataPath, "setup.csv");
        public static string GetTeamsFilePath() => Path.Combine(BaseDataPath, "teams.csv");
        public static string GetRoundFilePath(int roundNumber) => Path.Combine(BaseRoundsPath, $"round-{roundNumber}.csv");
        public static string GetAltRoundFilePath(int roundNumber) => Path.Combine(BaseRoundsPath, $"round-{roundNumber}-a.csv");
    }
}