using Models;
using Utilities;

namespace Services
{
    public class Result
    {
        // Process a specified number of rounds and return a list of teams with updated statistics.
        public static List<Team> ProcessRounds()
        {
            List<Team> teams = LoadData.Teams(); // Loads the teams from a data source
            var streaks = new Dictionary<string, Queue<string>>(); // Initialize a dictionary to track streaks for each team

            for (int i = 1; i <= Constants.MaxNumberOfRoundsInASeason; i++)
            {
                // An array of patterns to try for each round.
                string[] filePathPatterns = {
                    Constants.GetRoundFilePath(i),
                    Constants.GetAltRoundFilePath(i)
                };

                foreach (var filePath in filePathPatterns)
                {
                    if (File.Exists(filePath))
                    {
                        ProcessMatches(filePath, teams, streaks);
                    }
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

                var homeTeamObj = FindTeamByAbbreviation(teams, homeTeam);
                var awayTeamObj = FindTeamByAbbreviation(teams, awayTeam);

                if (homeTeamObj == null || awayTeamObj == null)
                {
                    // Handle or log the error if a team is not found
                    continue;
                }
                UpdateGames(homeTeamObj, awayTeamObj, homeGoals, awayGoals);
                UpdateGoals(homeTeamObj, awayTeamObj, homeGoals, awayGoals);
                UpdatePoints(homeTeamObj, awayTeamObj, homeGoals, awayGoals);
                UpdateStreaks(streaks, homeTeam, awayTeam, homeGoals, awayGoals);
            }

            TrimStreaks(streaks);
            UpdateTeamsWinningStreaks(teams, streaks);
        }

        public static Team? FindTeamByAbbreviation(List<Team> teams, string abbreviation)
        {
            return teams.FirstOrDefault(t => t.Abbreviation == abbreviation);
        }

        private static void UpdateGoals(Team home, Team away, int homeGoals, int awayGoals)
        {
            home.GoalsFor += homeGoals;
            home.GoalsAgainst += awayGoals;
            away.GoalsFor += awayGoals;
            away.GoalsAgainst += homeGoals;
        }

        public static void UpdatePoints(Team home, Team away, int homeGoals, int awayGoals)
        {
            // Update points and game statistics for the home and away teams
            if (homeGoals > awayGoals)
            {
                // Home team won
                home.Points += 3;
            }
            else if (homeGoals < awayGoals)
            {
                // Away team won
                away.Points += 3;
            }
            else
            {
                // Draw
                home.Points++;
                away.Points++;
            }
        }

        private static void UpdateGames(Team home, Team away, int homeGoals, int awayGoals)
        {
            home.GamesPlayed++;
            away.GamesPlayed++;

            // Update points and game statistics for the home and away teams
            if (homeGoals > awayGoals)
            {
                // Home team won
                home.GamesWon++;
                away.GamesLost++;
            }
            else if (homeGoals < awayGoals)
            {
                // Away team won
                away.GamesWon++;
                home.GamesLost++;
            }
            else
            {
                // Draw
                home.GamesDrawn++;
                away.GamesDrawn++;
            }
        }

        private static void UpdateStreaks(Dictionary<string, Queue<string>> streaks, string home, string away, int homeGoals, int awayGoals)
        {
            if (!streaks.TryGetValue(home, out var homeStreak))
            {
                homeStreak = new Queue<string>();
                streaks[home] = homeStreak;
            }

            if (!streaks.TryGetValue(away, out var awayStreak))
            {
                awayStreak = new Queue<string>();
                streaks[away] = awayStreak;
            }

            if (homeGoals > awayGoals)
            {
                homeStreak.Enqueue("W");
                awayStreak.Enqueue("L");
            }
            else if (homeGoals < awayGoals)
            {
                homeStreak.Enqueue("L");
                awayStreak.Enqueue("W");
            }
            else
            {
                homeStreak.Enqueue("D");
                awayStreak.Enqueue("D");
            }
        }

        private static void TrimStreaks(Dictionary<string, Queue<string>> streaks)
        {
            foreach (var queue in streaks.Values)
            {
                while (queue.Count > 5)
                {
                    queue.Dequeue();
                }
            }
        }

        private static void UpdateTeamsWinningStreaks(List<Team> teams, Dictionary<string, Queue<string>> streaks)
        {
            foreach (var team in teams)
            {
                if (streaks.TryGetValue(team.Abbreviation!, out var teamStreak))
                    team.WinningStreak = string.Join("", teamStreak);
            }
        }
    }
}
