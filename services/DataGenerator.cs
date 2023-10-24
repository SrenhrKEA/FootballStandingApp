using Models;
using Utilities;

namespace Services
{
    public class DataGenerator
    {
        public static void GenerateRounds(int numberOfRoundsPlayed)
        {
            numberOfRoundsPlayed = Math.Min(numberOfRoundsPlayed, Constants.MaxNumberOfRoundsInASeason);
            DeleteCSVFiles(Constants.BaseRoundsPath);

            List<string> teams = LoadTeams();

            // Generate matches for both regular and postseason rounds.
            for (int i = 1; i <= numberOfRoundsPlayed; i++)
            {
                List<FootballMatch> matches;

                if (i <= Constants.NumberOfRoundsInRegularSeason)
                {
                    matches = GenerateMatches(teams, i >= Constants.NumberOfRoundsInRegularSeason / 2);
                    RotateTeams(teams);
                }
                else if (i == Constants.NumberOfRoundsInRegularSeason + 1)
                {
                    var teamStatus = Qualification.ProcessRounds();
                    teams = CategorizeTeamsByStatus(teams, teamStatus);
                    matches = GeneratePostSeasonMatches(teams);
                }
                else
                {
                    matches = GeneratePostSeasonMatches(teams);
                    RotateTeams(teams);
                }

                WriteMatchesToFile(matches, i);
            }
        }

        private static List<string> LoadTeams()
        {
            return File.ReadAllLines(Constants.GetTeamsFilePath())
                .Skip(1)
                .Select(line => line.Split(';')[0])
                .ToList();
        }

        private static void RotateTeams(List<string> teams)
        {
            var temp = teams[^1];
            for (int i = teams.Count - 1; i > 1; i--)
                teams[i] = teams[i - 1];
            teams[1] = temp;
        }

        private static List<FootballMatch> GenerateMatches(List<string> teams, bool reverse)
        {
            List<FootballMatch> matches = new();
            int halfTeamCount = teams.Count / 2;
            for (int i = 0; i < halfTeamCount; i++)
            {
                var home = teams[i];
                var away = teams[teams.Count - 1 - i];

                if (reverse)
                    (home, away) = (away, home);

                matches.Add(new FootballMatch(home, away));
            }
            return matches;
        }

        private static List<FootballMatch> GeneratePostSeasonMatches(List<string> teams)
        {
            // Assuming teams list contains Championship teams followed by Relegation teams.
            var halfCount = teams.Count / 2;
            var championshipTeams = teams.GetRange(0, halfCount);
            var relegationTeams = teams.GetRange(halfCount, halfCount);

            var champMatches = GenerateMatches(championshipTeams, false);
            var relegMatches = GenerateMatches(relegationTeams, false);

            return champMatches.Concat(relegMatches).ToList();
        }

        private static void WriteMatchesToFile(List<FootballMatch> matches, int roundNumber)
        {
            var fileName = $"round-{roundNumber}.csv";
            var fullPath = Path.Combine(Constants.BaseRoundsPath, fileName);

            try
            {
                using StreamWriter writer = new(fullPath);
                writer.WriteLine("Home team;Away team;Score;Other");

                foreach (var match in matches)
                    writer.WriteLine($"{match.HomeTeam};{match.AwayTeam};{match.Score};{match.Other}");

                Console.WriteLine($"File {fileName} created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file {fileName}. Error: {ex.Message}");
            }
        }

        private static List<string> CategorizeTeamsByStatus(List<string> teams, Dictionary<string, string> teamStatus)
        {
            var championshipTeams = new List<string>();
            var relegationTeams = new List<string>();

            foreach (var team in teams)
            {
                if (teamStatus.TryGetValue(team, out var status))
                {
                    if (status == "Championship")
                        championshipTeams.Add(team);
                    else if (status == "Relegation")
                        relegationTeams.Add(team);
                }
                else
                {
                    Console.WriteLine($"Status not found for team: {team}");
                }
            }

            return championshipTeams.Concat(relegationTeams).ToList();
        }

        private static void DeleteCSVFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                return;
            }

            try
            {
                foreach (var csvFile in Directory.GetFiles(directoryPath, "*.csv"))
                {
                    File.Delete(csvFile);
                    Console.WriteLine($"Deleted file: {csvFile}");
                }
                Console.WriteLine("All CSV files deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
