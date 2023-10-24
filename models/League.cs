namespace Models
{
    public class League
    {
        public string? LeagueName { get; set; }
        public int PromotedToChampionsLeague { get; set; }
        public int PromotedToEuropeLeague { get; set; }
        public int PromotedToConferenceLeague { get; set; }
        public int PromotedToUpperLeague { get; set; }
        public int RelegatedToLowerLeague { get; set; }


        // Constructor
        public League()
        { }

        // ToString method to represent the object's data as a string
        public override string ToString()
        {
            return $"League Name: {LeagueName}, " +
                $"Promoted to Champions League: {PromotedToChampionsLeague}, " +
                $"Promoted to Europe League: {PromotedToEuropeLeague}, " +
                $"Promoted to Conference League: {PromotedToConferenceLeague}, " +
                $"Promoted to Upper League: {PromotedToUpperLeague}, " +
                $"Relegated to Lower League: {RelegatedToLowerLeague}";
        }
    }
}