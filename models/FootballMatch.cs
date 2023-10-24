using Utilities;

namespace Models 
{
    public class FootballMatch
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Score { get; set; }
        public string Other { get; set; }


        public FootballMatch(string homeTeam, string awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Score = GenerateScore();
            Other = RollDiceOutcome(50);
        }

        private static string GenerateScore()
        {
            Random random = new();
            int homeGoals = random.Next(0, Constants.GoalLimit);
            int awayGoals = random.Next(0, Constants.GoalLimit);
            return $"{homeGoals}-{awayGoals}";
        }

        private static string RollDiceOutcome(int sides) // Takes die of different side values.
        {
            Random random = new();
            int roll = random.Next(1, sides+1); // Generates a random number between 1 and sides (inclusive).

            if (roll == sides-1 || roll == sides-2)
            {
                return "Postponed";
            }
            else if (roll == sides)
            {
                return "Cancelled";
            }
            else
            {
                // Nothing happens.
                return null!;
            }
        }
    }
}