using Csv; // Import the Csv namespace

namespace Calc
{
    public class Result
    {
        // Process a specified number of rounds and return a list of teams with updated statistics.
        public static List<Team> ProcessRounds()
        {
            List<Team> teams = Load.Teams(); // Loads the teams from a data source
            var streaks = new Dictionary<string, Queue<string>>(); // Initialize a dictionary to track streaks for each team

            int maxRounds = 32; // Maximum number of rounds to process

            for (int i = 1; i <= maxRounds; i++)
            {
                string filePath = $"./data/rounds/round-{i}.csv"; // Construct the file path for round data
                string filePathAlt = $"./data/rounds/round-{i}-a.csv"; // Construct an alternative file path

                // Check if the primary file path exists and process it
                if (File.Exists(filePath))
                {
                    ProcessMatches(filePath, teams, streaks);
                }

                // Check if the alternative file path exists and process it
                if (File.Exists(filePathAlt))
                {
                    ProcessMatches(filePathAlt, teams, streaks);
                }
            }

            return teams; // Return the updated list of teams
        }

        // Process all the matches in a specified CSV file and update team statistics.
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

                // Find the corresponding team objects for the home and away teams
                var homeTeamObj = teams.First(t => t.Abbreviation == homeTeam);
                var awayTeamObj = teams.First(t => t.Abbreviation == awayTeam);

                // Update team statistics based on the match result
                homeTeamObj.GoalsFor += homeGoals;
                homeTeamObj.GoalsAgainst += awayGoals;
                awayTeamObj.GoalsFor += awayGoals;
                awayTeamObj.GoalsAgainst += homeGoals;
                homeTeamObj.GamesPlayed++;
                awayTeamObj.GamesPlayed++;

                // Initialize streak tracking queues for home and away teams if they don't exist
                if (!streaks.ContainsKey(homeTeam))
                    streaks[homeTeam] = new Queue<string>();
                if (!streaks.ContainsKey(awayTeam))
                    streaks[awayTeam] = new Queue<string>();

                if (homeGoals > awayGoals)
                {
                    // Update points and game statistics for the home and away teams
                    homeTeamObj.Points += 3;
                    homeTeamObj.GamesWon++;
                    awayTeamObj.GamesLost++;

                    // Enqueue match result codes for streak tracking
                    streaks[homeTeam].Enqueue("W");
                    streaks[awayTeam].Enqueue("L");
                }
                else if (homeGoals < awayGoals)
                {
                    // Update points and game statistics for the home and away teams
                    awayTeamObj.Points += 3;
                    awayTeamObj.GamesWon++;
                    homeTeamObj.GamesLost++;

                    // Enqueue match result codes for streak tracking
                    streaks[homeTeam].Enqueue("L");
                    streaks[awayTeam].Enqueue("W");
                }
                else
                {
                    // Update points and game statistics for a draw
                    homeTeamObj.Points++;
                    awayTeamObj.Points++;
                    homeTeamObj.GamesDrawn++;
                    awayTeamObj.GamesDrawn++;

                    // Enqueue match result codes for streak tracking
                    streaks[homeTeam].Enqueue("D");
                    streaks[awayTeam].Enqueue("D");
                }
            }

            // Trim streak tracking queues to a maximum length of 5
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
