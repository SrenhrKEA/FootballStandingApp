namespace Calc
{
    public class Qualification
    {
        public static Dictionary<string, string> Process()
        {
            Dictionary<string, int> teamPoints = new();

            foreach (var line in File.ReadAllLines("./data/teams.csv").Skip(1))
            {
                var parts = line.Split(';');
                var abbreviation = parts[0];
                teamPoints[abbreviation] = 0;
            }

            for (int i = 1; i <= 22; i++)
            {
                string filePath = $"./data/rounds/round-{i}.csv";
                string filePathAlt = $"./data/rounds/round-{i}-a.csv";

                // Check if filePath exists and process it
                if (File.Exists(filePath))
                {
                    ProcessMatches(filePath, teamPoints);
                }

                // Check if filePathAlt exists and process it
                if (File.Exists(filePathAlt))
                {
                    ProcessMatches(filePathAlt, teamPoints);
                }
            }

            // Sort teams by points
            var sortedTeams = teamPoints.ToList();
            sortedTeams.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            // Create a Dictionary of team abbreviations and statuses
            Dictionary<string, string> teamStatus = new();
            for (int i = 0; i < sortedTeams.Count; i++)
            {
                var team = sortedTeams[i];
                string status = i < 6 ? "Championship" : "Relegation";
                teamStatus[team.Key] = status;
            }

            // Return the Dictionary
            return teamStatus;
        }

        // Processes all the matches of a round and calculates each team's points
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
                    teamPoints[homeTeam]++;
                    teamPoints[awayTeam]++;
                }
            }
        }
    }
}