using Calc;
using Csv;
class Program
{
    static void Main()
    {
        // Generate data 
        // ResultsGenerator.GenerateResults(34);

        // Load data from setup.csv, teams.csv, and match result files.
        // Parse data into appropriate data structures.

        // Calculate standings based on match results.

        // Sort the standings based on your criteria.

        // Display the standings with formatting and colors.

        int numberOfRoundsPlayed = 32;
        
        // DataGenerator.GenerateData(numberOfRoundsPlayed);

        List<Team> teams = Result.ProcessResults(numberOfRoundsPlayed);

        Console.WriteLine("Football League Standings:");
        foreach (var team in teams.OrderByDescending(t => t.Points))
        {
            Console.WriteLine(team.ToString());
        }

        // List<League> leagues = Load.Setup();
        // League league = leagues[0]; // Currently theres only one league, the superliga. More leagues require a program refiguration with a teams.csv for each league.

        // Score.Process();
    }
}
