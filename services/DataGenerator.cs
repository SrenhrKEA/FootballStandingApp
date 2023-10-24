using Models;
using Utilities;

namespace Services
{
    public class DataGenerator
    {
        public static void GenerateRounds(int numberOfRoundsPlayed, List<Team> teams)
        {
            numberOfRoundsPlayed = Math.Min(numberOfRoundsPlayed, Constants.MaxNumberOfRoundsInASeason);
            DeleteCSVFiles(Constants.BaseRoundsPath);
            List<string> teamsByAbbrevation = GetTeamsByAbbreviation(teams);

            // Generate matches for both regular and postseason rounds.
            for (int i = 1; i <= numberOfRoundsPlayed; i++)
            {
                List<FootballMatch> matches;

                if (i <= Constants.NumberOfRoundsInRegularSeason)
                {
                    matches = GenerateMatches(teamsByAbbrevation, i >= Constants.NumberOfRoundsInRegularSeason / 2);
                    RotateTeams(teamsByAbbrevation);
                }
                else if (i == Constants.NumberOfRoundsInRegularSeason + 1)
                {
                    var teamStatus = Qualification.ProcessRounds(teamsByAbbrevation);
                    teamsByAbbrevation = CategorizeTeamsByStatus(teamsByAbbrevation, teamStatus);
                    matches = GeneratePostSeasonMatches(teamsByAbbrevation);
                }
                else
                {
                    matches = GeneratePostSeasonMatches(teamsByAbbrevation);
                    RotateTeams(teamsByAbbrevation);
                }

                WriteMatchesToFile(matches, i);
            }
        }
        private static List<string> GetTeamsByAbbreviation(List<Team> teams)
        {
            return teams.Select(team => team.Abbreviation!).ToList();
        }


        private static void RotateTeams(List<string> teamsByAbbrevation)
        {
            var temp = teamsByAbbrevation[^1];
            for (int i = teamsByAbbrevation.Count - 1; i > 1; i--)
                teamsByAbbrevation[i] = teamsByAbbrevation[i - 1];
            teamsByAbbrevation[1] = temp;
        }

        private static List<FootballMatch> GenerateMatches(List<string> teamsByAbbrevation, bool reverse)
        {
            List<FootballMatch> matches = new();
            int halfTeamCount = teamsByAbbrevation.Count / 2;
            for (int i = 0; i < halfTeamCount; i++)
            {
                var home = teamsByAbbrevation[i];
                var away = teamsByAbbrevation[teamsByAbbrevation.Count - 1 - i];

                if (reverse)
                    (home, away) = (away, home);

                matches.Add(new FootballMatch(home, away));
            }
            return matches;
        }

        private static List<FootballMatch> GeneratePostSeasonMatches(List<string> teamsByAbbrevation)
        {
            // Assuming teams list contains Championship teams followed by Relegation teams.
            var halfCount = teamsByAbbrevation.Count / 2;
            var championshipTeams = teamsByAbbrevation.GetRange(0, halfCount);
            var relegationTeams = teamsByAbbrevation.GetRange(halfCount, halfCount);

            var champMatches = GenerateMatches(championshipTeams, false);
            var relegMatches = GenerateMatches(relegationTeams, false);

            return champMatches.Concat(relegMatches).ToList();
        }

        private static void WriteMatchesToFile(List<FootballMatch> matches, int roundNumber)
        {
            var baseFileName = $"round-{roundNumber}";
            var mainFilePath = Path.Combine(Constants.BaseRoundsPath, $"{baseFileName}.csv");
            var altFilePath = Path.Combine(Constants.BaseRoundsPath, $"{baseFileName}-a.csv");

            List<FootballMatch> otherMatches = new();

            try
            {
                using (StreamWriter writer = new(mainFilePath, true))
                {
                    writer.WriteLine("Home team;Away team;Score;Other");
                    foreach (var match in matches)
                    {
                        if (string.IsNullOrEmpty(match.Other))
                            writer.WriteLine($"{match.HomeTeam};{match.AwayTeam};{match.Score}");
                        else
                            otherMatches.Add(match);
                    }
                }
                Console.WriteLine($"File {baseFileName}.csv created successfully!");

                if (otherMatches.Any())
                {
                    WriteToFileWithOtherInfo(altFilePath, otherMatches);
                    Console.WriteLine($"File {baseFileName}-a.csv created successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file {baseFileName}-a.csv. Error: {ex.Message}");
            }
        }

        private static void WriteToFileWithOtherInfo(string filePath, List<FootballMatch> matches)
        {
            using StreamWriter writer = new(filePath, true);
            writer.WriteLine("Home team;Away team;Score;Other");
            foreach (var match in matches)
            {
                writer.WriteLine($"{match.HomeTeam};{match.AwayTeam};{match.Score};{match.Other}");
            }
        }


        private static List<string> CategorizeTeamsByStatus(List<string> teamsByAbbrevation, Dictionary<string, string> teamStatus)
        {
            var championshipTeams = new List<string>();
            var relegationTeams = new List<string>();

            foreach (var team in teamsByAbbrevation)
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
