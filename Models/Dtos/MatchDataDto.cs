namespace RiotAPI.Models.Dtos
{
    public class MatchDataDto
    {
        public string MatchId { get; set; }

        public string PuuId { get; set; }

        public int ParticipantsIndex { get; set; }

        public int TeamIndex { get; set; }

        public  int IsWinner { get; set; }

        public string ChampionName { get; set; }

        public int GameModeId { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists {  get; set; }

        public int Spell1Id { get; set; }

        public int Spell2Id { get; set; }

        public int RuneMainId { get; set; }

        public int RuneSubId { get; set; }

        public int Item0Id { get; set; }

        public int Item1Id { get; set; }

        public int Item2Id { get; set; }

        public int Item3Id { get; set; }

        public int Item4Id { get; set; }

        public int Item5Id { get; set; }

        public int Item6Id { get; set; }
    }
}
