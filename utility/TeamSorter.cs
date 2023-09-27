using Model;

namespace Utility
{
    class TeamSorter
    {
        public static List<Team> Sort(List<Team> teams)
        {
            return teams
                .OrderByDescending(t => t.Points) // 1. By points
                .ThenByDescending(t => t.GoalDifference) // 2. By goal difference
                .ThenByDescending(t => t.GoalsFor) // 3. By goals for
                .ThenBy(t => t.GoalsAgainst) // 4. By goals against
                .ThenBy(t => t.FullClubName, StringComparer.OrdinalIgnoreCase).ToList(); // 5. Alphabetically
        }
    }
}