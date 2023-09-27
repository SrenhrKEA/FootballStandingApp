# FootballStandingApp

## Description
This program is part of a school assignment and is  designed to manage and display sports league standings based on match results. It offers functionality to generate sample data, load data from CSV files, calculate standings, sort teams, and display the standings in a visually appealing format.

## Usage
# Step 1: Generate Data
```
int numberOfRoundsPlayed = 32; // Set the desired number of rounds
DataGenerator.GenerateRounds(numberOfRoundsPlayed);
```
This step generates sample match data for the league. You can adjust the number of rounds played as needed.

# Step 2: Load Data
```
List<League> leagues = LoadData.Setup();
League league = leagues[0]; // Currently, the program handles a single league.
```
Load data from setup.csv, teams.csv, and match result files. The program currently supports a single league. Make sure the necessary CSV files are present and correctly formatted.

# Step 3: Calculate Standings
```
List<Team> teams = Result.ProcessRounds();
```
Calculate the league standings based on the match results.

# Step 4: Sort Standings
```
teams = TeamSorter.Sort(teams);
```
Sort the league standings based on your desired criteria. Modify the sorting logic in TeamSorter as needed.

# Step 5: Display Standings
```
DisplayManager.DisplayTeams(teams, league);
```
Display the league standings with formatting and colors. The DisplayManager class handles the presentation of the standings.

## Notes
The program is currently set up to handle a single league. Extending it to support multiple leagues would require adjustments to the data structure and codebase.
