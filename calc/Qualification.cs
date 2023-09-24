namespace Calc
{
    public class Qualification
    {
        public static Dictionary<string, string> Calculate()
        {
            // Initialize the points dictionary and read teams from file
            Dictionary<string, int> teamPoints = new Dictionary<string, int>();
            // Dictionary<string, string> fullNames = new Dictionary<string, string>();

            foreach (var line in File.ReadAllLines("./data/teams.csv").Skip(1))
            {
                var parts = line.Split(';');
                var abbreviation = parts[0];
                // var fullName = parts[1];
                teamPoints[abbreviation] = 0;
                // fullNames[abbreviation] = fullName;
            }

            for (int i = 1; i <= 22; i++)
            {
                string fileName = $"./data/rounds/round-{i}.csv";
                string fileNameAlt = $"./data/rounds/round-{i}-a.csv";
                // bool isFirstLine = true; // Flag to check if it's the first line

                // Check if fileName exists and process it
                if (File.Exists(fileName))
                {
                    ProcessFile(fileName, teamPoints);
                }

                // Check if fileNameAlt exists and process it
                if (File.Exists(fileNameAlt))
                {
                    ProcessFile(fileNameAlt, teamPoints);
                }
            }

            // Sort teams by points
            var sortedTeams = teamPoints.ToList();
            sortedTeams.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));

            // Create a Dictionary of team abbreviations and statuses
            Dictionary<string, string> teamStatus = new Dictionary<string, string>();
            for (int i = 0; i < sortedTeams.Count; i++)
            {
                var team = sortedTeams[i];
                string status = i < 6 ? "Championship" : "Relegation";
                teamStatus[team.Key] = status;
            }

            // Return the Dictionary
            return teamStatus;
        }
        
        // Define a separate method to process a file
        private static void ProcessFile(string filePath, Dictionary<string, int> teamPoints)
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