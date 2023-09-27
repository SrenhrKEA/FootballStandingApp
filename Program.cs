using Model;
using Service;
using Utility;
class Program
{
    static void Main()
    {
        // Generate data 
        int numberOfRoundsPlayed = 32; // Can be set to any number
        DataGenerator.GenerateRounds(numberOfRoundsPlayed);

        // Load data from setup.csv, teams.csv, and match result files.
        List<League> leagues = LoadData.Setup();
        League league = leagues[0]; // Currently, the program isn't setup for multiple leagues. This would require a seperate teams.csv for each league, and changing the code base to handle it.
        // Console.WriteLine(league);

        // Calculate standings based on match results.
        List<Team> teams = Result.ProcessRounds();

        // Sort the standings based on your criteria.
        teams = TeamSorter.Sort(teams);

        // Display the standings with formatting and colors.
        DisplayManager.DisplayTeams(teams, league);
    }
}
