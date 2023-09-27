using Csv;

namespace Calc
{
    public class Result
    {
        public static List<Team> ProcessResults(int numberOfRoundsPlayed)
        {
            List<Team> teams = Load.Teams(); // Loads the teams here
            var streaks = new Dictionary<string, Queue<string>>(); // Initialize streaks here

            for (int i = 1; i <= numberOfRoundsPlayed; i++)
            {
                string filePath = $"./data/rounds/round-{i}.csv";
                string filePathAlt = $"./data/rounds/round-{i}-a.csv";

                // Check if filePath exists and process it
                if (File.Exists(filePath))
                {
                    ProcessMatches(filePath, teams, streaks);
                }

                // Check if filePathAlt exists and process it
                if (File.Exists(filePathAlt))
                {
                    ProcessMatches(filePathAlt, teams, streaks);
                }
            }

            return teams;
        }

        // Processes all the matches of a round
        private static void ProcessMatches(string filePath, List<Team> teams, Dictionary<string, Queue<string>> streaks)
        {
            foreach (var line in File.ReadAllLines(filePath).Skip(1))
            {
                var parts = line.Split(';');
                var homeTeam = parts[0];
                var awayTeam = parts[1];
                var score = parts[2].Split('-');
                int homeGoals = int.Parse(score[0]);
                int awayGoals = int.Parse(score[1]);

                var homeTeamObj = teams.First(t => t.Abbreviation == homeTeam);
                var awayTeamObj = teams.First(t => t.Abbreviation == awayTeam);

                homeTeamObj.GoalsFor += homeGoals;
                homeTeamObj.GoalsAgainst += awayGoals;
                awayTeamObj.GoalsFor += awayGoals;
                awayTeamObj.GoalsAgainst += homeGoals;
                homeTeamObj.GamesPlayed++;
                awayTeamObj.GamesPlayed++;

                if (!streaks.ContainsKey(homeTeam))
                    streaks[homeTeam] = new Queue<string>();
                if (!streaks.ContainsKey(awayTeam))
                    streaks[awayTeam] = new Queue<string>();

                if (homeGoals > awayGoals)
                {
                    homeTeamObj.Points += 3;
                    homeTeamObj.GamesWon++;
                    awayTeamObj.GamesLost++;

                    streaks[homeTeam].Enqueue("W");
                    streaks[awayTeam].Enqueue("L");
                }
                else if (homeGoals < awayGoals)
                {
                    awayTeamObj.Points += 3;
                    awayTeamObj.GamesWon++;
                    homeTeamObj.GamesLost++;

                    streaks[homeTeam].Enqueue("L");
                    streaks[awayTeam].Enqueue("W");
                }
                else
                {
                    homeTeamObj.Points++;
                    awayTeamObj.Points++;
                    homeTeamObj.GamesDrawn++;
                    awayTeamObj.GamesDrawn++;

                    streaks[homeTeam].Enqueue("D");
                    streaks[awayTeam].Enqueue("D");
                }
            }

            foreach (var queue in streaks.Values)
            {
                while (queue.Count > 5)
                {
                    queue.Dequeue();
                }
            }

            // Update teams' winning streaks after processing the round
            foreach (var team in teams)
            {
                if (streaks.ContainsKey(team.Abbreviation!))
                    team.WinningStreak = string.Join("", streaks[team.Abbreviation!]);
            }
        }
    }
}