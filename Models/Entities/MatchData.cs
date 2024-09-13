namespace RiotAPI.Models.Entities
{
    public class MatchData
    {
        public string matchid { get; set; }

        public string puuid { get; set; }

        public int participantsindex { get; set; }

        public int teamindex { get; set; }

        public bool iswinner { get; set; }

        public string championname { get; set; }

        public int gamemodeid { get; set; }

        public int kills { get; set; }

        public int deaths { get; set; }

        public int assists { get; set; }

        public int spell1id { get; set; }

        public int spell2id { get; set; }

        public int runemainid { get; set; }

        public int runesubid { get; set; }

        public int item0id { get; set; }

        public int item1id { get; set; }

        public int item2id { get; set; }

        public int item3id { get; set; }

        public int item4id { get; set; }

        public int item5id { get; set; }

        public int item6id { get; set; }
    }
    
}
