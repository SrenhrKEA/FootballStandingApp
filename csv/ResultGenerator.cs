namespace Csv
{
    public class ResultsGenerator
    {
        public static void GenerateResults(int iterations, string directoryPath = "./data/rounds")
        {
            Dictionary<string, string> teamStatus = new Dictionary<string, string>();

            DeleteFiles(directoryPath);

            List<string> teams = File.ReadAllLines("./data/teams.csv").Skip(1).Select(line => line.Split(';')[0]).ToList();

            for (int i = 1; i <= Math.Min(iterations, 22); i++) // Limit the loop to 22 iterations or 'iterations' if less
            {
                List<FootballMatch> matches = GenerateMatches(teams, i >= 11);
                string fileName = $"round-{i}.csv";
                string fullPath = Path.Combine(directoryPath, fileName);

                WriteMatchesToFile(matches,fullPath,fileName);

                // Rotate the teams for the next round
                RotateTeams(teams);
            }

            if (iterations >= 22)
            {
                // Calculate the status after all iterations
                teamStatus = Calc.Qualification.Calculate();

                // // Iterate through the dictionary and print each key-value pair.
                // foreach (var kvp in teamStatus)
                // {
                //     Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                // }
            }

            if (iterations > 22)
            {
                // Initialize lists to store championship and relegation teams
                List<string> championshipTeams = new List<string>();
                List<string> relegationTeams = new List<string>();

                // Compare each team's status and categorize them
                foreach (string team in teams)
                {
                    if (teamStatus.ContainsKey(team))
                    {
                        string status = teamStatus[team];
                        if (status == "Championship")
                        {
                            championshipTeams.Add(team);
                        }
                        else if (status == "Relegation")
                        {
                            relegationTeams.Add(team);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Status not found for team: {team}");
                    }
                }

                for (int i = 1; i <= Math.Min(iterations-22, 10); i++) // Limit the loop to 10 iterations or 'iterations-22' if less
                {
                    List<FootballMatch> championshipMatches = GenerateMatches(championshipTeams, i >= 5);
                    List<FootballMatch> relegationMatches = GenerateMatches(relegationTeams, i >= 5);
                    List<FootballMatch> matches = championshipMatches.Concat(relegationMatches).ToList();
                    string fileName = $"round-{i + 22}.csv";
                    string fullPath = Path.Combine(directoryPath, fileName);

                    WriteMatchesToFile(matches, fullPath, fileName);

                    // Rotate the teams for the next round
                    RotateTeams(championshipTeams);
                    RotateTeams(relegationTeams);

                }
            }
        }

        private static void RotateTeams(List<string> teams)
        {
            string temp = teams[teams.Count - 1];
            for (int i = teams.Count - 1; i > 1; i--)
            {
                teams[i] = teams[i - 1];
            }
            teams[1] = temp;
        }

        private static List<FootballMatch> GenerateMatches(List<string> teams, bool reverse)
        {
            List<FootballMatch> matches = new List<FootballMatch>();
            for (int i = 0; i < teams.Count / 2; i++)
            {
                var home = teams[i];
                var away = teams[teams.Count - i - 1];
                if (reverse)
                {
                    var temp = home;
                    home = away;
                    away = temp;
                }
                matches.Add(new FootballMatch(home, away));
            }
            return matches;
        }

        private static void WriteMatchesToFile(List<FootballMatch> matches, string fullPath, string fileName)
        {
            try
            {
                // Create a new file and add match results
                using (StreamWriter writer = new StreamWriter(fullPath, true))
                {
                    foreach (var match in matches)
                    {
                        writer.WriteLine($"{match.HomeTeam};{match.AwayTeam};{match.Score}");
                    }
                }

                Console.WriteLine($"File {fileName} created successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file {fileName}. Error: {ex.Message}");
            }
        }

        private static void DeleteFiles(string directoryPath)
        {
            // Check if the directory exists
            if (Directory.Exists(directoryPath))
            {
                try
                {
                    // Get all CSV files in the directory
                    string[] csvFiles = Directory.GetFiles(directoryPath, "*.csv");

                    // Delete each CSV file
                    foreach (string csvFile in csvFiles)
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
            else
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
            }
        }
    }
}
