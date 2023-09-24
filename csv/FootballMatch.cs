namespace Csv 
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
            Other = RollDiceOutcome(20);
        }

        private string GenerateScore()
        {
            Random random = new Random();
            int homeGoals = random.Next(0, 6); // for the sake of simplicity, the limit is 5 goals per team.
            int awayGoals = random.Next(0, 6);
            return $"{homeGoals}-{awayGoals}";
        }

        private string RollDiceOutcome(int sides) // Takes die of different side values.
        {
            Random random = new Random();
            int roll = random.Next(1, sides+1); // Generates a random number between 1 and sides (inclusive).

            if (roll == sides-1)
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