namespace Csv 
{
    public class FootballMatch
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string Score { get; set; }

        public FootballMatch(string homeTeam, string awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
            Score = GenerateScore();
        }

        private string GenerateScore()
        {
            Random random = new Random();
            int homeGoals = random.Next(0, 6); // for the sake of simplicity, let's limit to 5 goals per team.
            int awayGoals = random.Next(0, 6);
            return $"{homeGoals}-{awayGoals}";
        }
    }
}