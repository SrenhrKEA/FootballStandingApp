using Models;
using Services;
using Utilities;

class Program
{
    static void Main()
    {
        // Display greeting message
        Console.WriteLine("Welcome to the League Manager!");

        while (true) // This loop will keep the program running until the user decides to exit
        {
            // Display options to the user
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Generate new data");
            Console.WriteLine("2. Use existing data");
            Console.WriteLine("3. Exit");
            string choice = Console.ReadLine()!;

            // Handle user's choice
            switch (choice)
            {
                case "1":
                    // Prompt the user for the number of rounds
                    Console.WriteLine("\nPlease enter the number of rounds played: ");
                    if (!int.TryParse(Console.ReadLine(), out int numberOfRoundsPlayed))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue; // Skip the rest of the loop and prompt the user again
                    }

                    // Generate data 
                    DataGenerator.GenerateRounds(numberOfRoundsPlayed);
                    break;

                case "2":
                    // Do nothing, proceed with existing data
                    break;

                case "3":
                    // Exit the program
                    Console.WriteLine("Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    continue; // Skip the rest of the loop and prompt the user again
            }

            // Load data from setup.csv, teams.csv, and match result files.
            List<League> leagues = LoadData.Setup();
            League league = leagues[0];

            // Calculate standings based on match results.
            List<Team> teams = Result.ProcessRounds();

            // Sort the standings based on your criteria.
            teams = TeamSorter.Sort(teams);

            // Display the standings with formatting and colors.
            DisplayManager.DisplayTeams(teams, league);

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            // Console.Clear(); // Clear the console for the next iteration
        }
    }
}
