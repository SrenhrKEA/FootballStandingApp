using Models;

namespace Utilities
{
    public class DisplayManager
    {
        public static void DisplayTeams(List<Team> teams, League league)
        {
            Console.WriteLine(league.LeagueName + " Standings:");
            // Header
            Console.WriteLine($"{PaddedString("Position", 9)} | " +
                            $"{PaddedString("Abbreviation", 15)} | " +
                            $"{PaddedString("Full Club Name", 20)} | " +
                            $"{PaddedString("Ranking", 10)} | " +
                            $"{PaddedString("Played", 7)} | " +
                            $"{PaddedString("Won", 4)} | " +
                            $"{PaddedString("Drawn", 6)} | " +
                            $"{PaddedString("Lost", 5)} | " +
                            $"{PaddedString("Goals For", 10)} | " +
                            $"{PaddedString("Goals Against", 14)} | " +
                            $"{PaddedString("Difference", 11)} | " +
                            $"{PaddedString("Points", 7)} | " +
                            $"{PaddedString("Streak", 7)}");
            Console.WriteLine(new string('-', 140));

            // Data
            for (int i = 0; i < teams.Count; i++)
            {
                if (teams[0].GamesPlayed >= 22)
                {
                    if (i < 6)
                        Console.ForegroundColor = ConsoleColor.Green; // Set the color to green for top 6 teams
                    else if (i >= teams.Count - 6)
                        Console.ForegroundColor = ConsoleColor.Red; // Set the color to red for bottom 6 teams
                    else
                        Console.ResetColor(); // Reset color for other teams
                }

                var team = teams[i];
                Console.WriteLine($"{PaddedString((i + 1).ToString(), 9)} | " + // Position column
                                $"{PaddedString(team.Abbreviation ?? "", 15)} | " +
                                $"{PaddedString(team.FullClubName ?? "", 20)} | " +
                                $"{PaddedString(team.SpecialRanking ?? "", 10)} | " +
                                $"{PaddedString(team.GamesPlayed.ToString(), 7)} | " +
                                $"{PaddedString(team.GamesWon.ToString(), 4)} | " +
                                $"{PaddedString(team.GamesDrawn.ToString(), 6)} | " +
                                $"{PaddedString(team.GamesLost.ToString(), 5)} | " +
                                $"{PaddedString(team.GoalsFor.ToString(), 10)} | " +
                                $"{PaddedString(team.GoalsAgainst.ToString(), 14)} | " +
                                $"{PaddedString(team.GoalDifference.ToString(), 11)} | " +
                                $"{PaddedString(team.Points.ToString(), 7)} | " +
                                $"{PaddedString(team.WinningStreak ?? "", 7)}");
            }
            Console.ResetColor(); // Reset console color at the end
        }

        // Helper function to get a padded string
        public static string PaddedString(string str, int totalLength)
        {
            return str.PadRight(totalLength);
        }
    }
}