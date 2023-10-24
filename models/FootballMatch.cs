using Utilities;

namespace Models
{
    public class FootballMatch
    {
        public string HomeTeam { get; }
        public string AwayTeam { get; }
        public string Score { get; }
        public string Other { get; }

        private static readonly Random Random = new Random();

        public FootballMatch(string homeTeam, string awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Score = GenerateScore();
            Other = GenerateOutcome();
        }

        private static string GenerateScore()
        {
            int homeGoals = Random.Next(0, Constants.GoalLimit);
            int awayGoals = Random.Next(0, Constants.GoalLimit);
            return $"{homeGoals}-{awayGoals}";
        }

        private static string GenerateOutcome()
        {
            int result = Random.Next(1, Constants.TotalOutcomes + 1); // Generates a random number.

            switch (result)
            {
                case int r when r == Constants.TotalOutcomes - 1 || r == Constants.TotalOutcomes - 2:
                    return "Postponed";
                case int r when r == Constants.TotalOutcomes:
                    return "Cancelled";
                default:
                    return "";
            }
        }
    }
}
