using Model;

namespace Service
{
    public class LoadData
    {
        public static List<League> Setup()
        {
            List<League> leagues = new();

            foreach (var setupTuple in File.ReadAllLines("./data/setup.csv")
            .Skip(1)
            .Select(line => line
            .Split(';'))
            .Select(parts => (parts[0], parts[1], parts[2], parts[3], parts[4], parts[5])))
            {
                League league = new()
                {
                    LeagueName = setupTuple.Item1,
                    PromotedToChampionsLeague = int.Parse(setupTuple.Item2),
                    PromotedToEuropeLeague = int.Parse(setupTuple.Item3),
                    PromotedToConferenceLeague = int.Parse(setupTuple.Item4),
                    PromotedToUpperLeague = int.Parse(setupTuple.Item5),
                    RelegatedToLowerLeague = int.Parse(setupTuple.Item6)
                };
                leagues.Add(league);
            }
            return leagues;
        }

        public static List<Team> Teams()
        {
            List<Team> teams = new();

            foreach (var teamTuple in File.ReadAllLines("./data/teams.csv")
            .Skip(1)
            .Select(line => line
            .Split(';'))
            .Select(parts => (parts[0], parts[1], parts[2])))
            {
                Team team = new()
                {
                    Abbreviation = teamTuple.Item1.ToString(),
                    FullClubName = teamTuple.Item2.ToString(),
                    SpecialRanking = teamTuple.Item3.ToString()
                };
                teams.Add(team);
            }
            return teams;
        }
    }
}