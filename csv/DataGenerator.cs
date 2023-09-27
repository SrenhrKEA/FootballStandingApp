namespace Csv
{
    public class DataGenerator
    {
        // Generate rounds of football matches based on the number of rounds played.
        public static void GenerateRounds(int numberOfRoundsPlayed, string directoryPath = "./data/rounds")
        {
            Dictionary<string, string> teamStatus = new(); // Initialize a dictionary to track team statuses

            // Ensure that the maximum number of rounds is capped at 32
            if (numberOfRoundsPlayed > 32)
            {
                numberOfRoundsPlayed = 32;
            }

            // Delete all existing .csv files in the specified directory before generating new ones
            DeleteCSVFiles(directoryPath);

            // Load team abbreviations from a CSV file and initialize their points to zero
            List<string> teams = File.ReadAllLines("./data/teams.csv")
                .Skip(1)
                .Select(line => line.Split(';')[0])
                .ToList();

            // Generate and write matches for regular rounds (up to 22 rounds)
            for (int i = 1; i <= Math.Min(numberOfRoundsPlayed, 22); i++)
            {
                List<FootballMatch> matches = GenerateMatches(teams, i >= 11);
                string fileName = $"round-{i}.csv"; // Regular rounds
                string fileNameAlt = $"round-{i}-a.csv"; // Alternative rounds for postponed and cancelled games
                string fullPath = Path.Combine(directoryPath, fileName);
                string fullPathAlt = Path.Combine(directoryPath, fileNameAlt);

                // Write matches to CSV files
                WriteMatchesToFile(matches, fullPath, fileName, fullPathAlt, fileNameAlt);

                // Rotate the teams for the next round
                RotateTeams(teams);
            }

            // After 22 rounds or more, calculate team qualification statuses
            if (numberOfRoundsPlayed >= 22)
            {
                teamStatus = Calc.Qualification.ProcessRounds();
                Console.WriteLine("Qualifications:");
                foreach (var kvp in teamStatus)
                {
                    Console.WriteLine($"Key: {kvp.Key}, Value: {kvp.Value}");
                }
            }

            // Generate and write matches for championship and relegation rounds (up to 10 rounds each)
            if (numberOfRoundsPlayed > 22)
            {
                // Initialize lists to store championship and relegation teams
                List<string> championshipTeams = new();
                List<string> relegationTeams = new();

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

                // Generate and write matches for championship and relegation rounds
                for (int i = 1; i <= Math.Min(numberOfRoundsPlayed - 22, 10); i++)
                {
                    List<FootballMatch> championshipMatches = GenerateMatches(championshipTeams, i >= 5);
                    List<FootballMatch> relegationMatches = GenerateMatches(relegationTeams, i >= 5);
                    List<FootballMatch> matches = championshipMatches.Concat(relegationMatches).ToList();
                    string fileName = $"round-{i + 22}.csv";
                    string fileNameAlt = $"round-{i + 22}-a.csv";
                    string fullPath = Path.Combine(directoryPath, fileName);
                    string fullPathAlt = Path.Combine(directoryPath, fileNameAlt);

                    // Write matches to CSV files
                    WriteMatchesToFile(matches, fullPath, fileName, fullPathAlt, fileNameAlt);

                    // Rotate the teams for the next round
                    RotateTeams(championshipTeams);
                    RotateTeams(relegationTeams);
                }
            }
        }

        // Rotate a list of teams to simulate fixture rotation
        private static void RotateTeams(List<string> teams)
        {
            string temp = teams[teams.Count - 1];
            for (int i = teams.Count - 1; i > 1; i--)
            {
                teams[i] = teams[i - 1];
            }
            teams[1] = temp;
        }

        // Generate a list of football matches based on a list of teams
        private static List<FootballMatch> GenerateMatches(List<string> teams, bool reverse)
        {
            List<FootballMatch> matches = new();
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

        // Write a list of football matches to CSV files
        private static void WriteMatchesToFile(List<FootballMatch> matches, string fullPath, string fileName, string fullPathAlt, string fileNameAlt)
        {
            List<FootballMatch> otherMatches = new();
            try
            {
                // Create a new file and add match results
                using (StreamWriter writer = new(fullPath, true))
                {
                    writer.WriteLine($"Home team;Away team;Score;Other");
                    foreach (var match in matches)
                    {
                        if (string.IsNullOrEmpty(match.Other))
                            writer.WriteLine($"{match.HomeTeam};{match.AwayTeam};{match.Score}");
                        else
                            otherMatches.Add(match);
                    }
                }

                Console.WriteLine($"File {fileName} created successfully!");

                if (otherMatches.Count != 0)
                {
                    // Create a new file and add match results
                    using (StreamWriter writer = new(fullPathAlt, true))
                    {
                        writer.WriteLine($"Home team;Away team;Score;Other");
                        foreach (var match in otherMatches)
                        {
                            writer.WriteLine($"{match.HomeTeam};{match.AwayTeam};{match.Score};{match.Other}");
                        }
                    }

                    Console.WriteLine($"File {fileNameAlt} created successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating file {fileNameAlt}. Error: {ex.Message}");
            }
        }

        // Delete all .csv files in a specified directory
        private static void DeleteCSVFiles(string directoryPath)
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
