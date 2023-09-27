namespace Csv
{
    public class Team
    {
        public string? Abbreviation { get; set; }
        public string? FullClubName { get; set; }
        public string? SpecialRanking { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesLost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference => GoalsFor - GoalsAgainst;
        public int Points { get; set; }
        public string? WinningStreak { get; set; }

        // Constructor
        public Team()
        { }

        public override string ToString()
        {
            return $"Abbreviation: {Abbreviation}, " +
                $"Full Club Name: {FullClubName}, " +
                $"Special Ranking: {SpecialRanking}, " +
                $"Games Played: {GamesPlayed}, " +
                $"Games Won: {GamesWon}, " +
                $"Games Drawn: {GamesDrawn}, " +
                $"Games Lost: {GamesLost}, " +
                $"Goals For: {GoalsFor}, " +
                $"Goals Against: {GoalsAgainst}, " +
                $"Goal Difference: {GoalDifference}, " +
                $"Points: {Points}, " +
                $"Winning Streak: {WinningStreak}";
        }
    }
}