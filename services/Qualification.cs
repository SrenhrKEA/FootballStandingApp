using Services;
using Utilities;

namespace Services
{
    public class Qualification
    {
        public static Dictionary<string, string> ProcessRounds()
        {
            var teamPoints = LoadData.LoadInitialTeamPoints();

            for (int i = 1; i <= Constants.NumberOfRoundsInRegularSeason; i++)
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
                        ProcessMatches(filePath, teamPoints);
                    }
                }
            }

            return DetermineQualificationStatuses(teamPoints);
        }

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

        private static Dictionary<string, string> DetermineQualificationStatuses(Dictionary<string, int> teamPoints)
        {
            return teamPoints.OrderByDescending(pair => pair.Value)
                .ToDictionary(
                    pair => pair.Key,
                    pair => teamPoints.Keys.ToList().IndexOf(pair.Key) < Constants.ChampionshipPositionLimit ? "Championship" : "Relegation"
                );
        }
    }
}
