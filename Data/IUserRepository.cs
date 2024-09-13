using RiotAPI.Models.Entities;

namespace RiotAPI.Data
{
    public interface IUserRepository
    {
        Task<string> GetPuuidAsync(string gameName, string tagLine);
        Task SaveUserAsync(string gameName, string tagLine, string puuid);

        Task<List<string>> GetMatchIdsAsync(string puuid);

        Task SaveMatchIdsAsync(string puuid, string matchId);

        Task<MatchData> GetMatchDataAsync(string puuid, string matchId);

        Task SaveMatchDataAsync(MatchData matchData);
    }
}
