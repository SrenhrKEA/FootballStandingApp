namespace Calc
{
    public class Qualification
    {
        // Process the rounds and determine team qualification statuses.
        public static Dictionary<string, string> ProcessRounds()
        {
            Dictionary<string, int> teamPoints = new(); // Initialize a dictionary to track team points

            // Load team data from a CSV file and initialize their points to zero
            foreach (var line in File.ReadAllLines("./data/teams.csv").Skip(1))
            {
                var parts = line.Split(';');
                var abbreviation = parts[0];
                teamPoints[abbreviation] = 0;
            }

            // Process a series of rounds to calculate team points
            for (int i = 1; i <= 22; i++)
            {
                string filePath = $"./data/rounds/round-{i}.csv"; // Construct the file path for round data
                string filePathAlt = $"./data/rounds/round-{i}-a.csv"; // Construct an alternative file path

                // Check if the primary file path exists and process it
                if (File.Exists(filePath))
                {
                    ProcessMatches(filePath, teamPoints);
                }

                // Check if the alternative file path exists and process it
                if (File.Exists(filePathAlt))
                {
                    ProcessMatches(filePathAlt, teamPoints);
                }
            }

            // Sort teams by points in descending order
            var sortedTeams = teamPoints.ToList();
            sortedTeams.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            // Create a Dictionary of team abbreviations and their qualification statuses
            Dictionary<string, string> teamStatus = new();
            for (int i = 0; i < sortedTeams.Count; i++)
            {
                var team = sortedTeams[i];
                string status = i < 6 ? "Championship" : "Relegation"; // Determine qualification status
                teamStatus[team.Key] = status;
            }

            // Return the Dictionary containing team qualification statuses
            return teamStatus;
        }

        // Process all the matches of a round and calculate each team's points
        private static void ProcessMatches(string filePath, Dictionary<string, int> teamPoints)
        {
            foreach (var line in File.ReadAllLines(filePath).Skip(1))
            {
                var parts = line.Split(';');
                var homeTeam = parts[0];
                var awayTeam = parts[1];
                var score = parts[2].Split('-');
                int homeGoals = int.Parse(score[0]);
                int awayGoals = int.Parse(score[1]);

                // Update team points based on the match result
                if (homeGoals > awayGoals)
                {
                    teamPoints[homeTeam] += 3;
                }
                else if (homeGoals < awayGoals)
                {
                    teamPoints[awayTeam] += 3;
                }
                else
                {
                    // In case of a draw, both teams receive one point
                    teamPoints[homeTeam]++;
                    teamPoints[awayTeam]++;
                }
            }
        }
    }
}
