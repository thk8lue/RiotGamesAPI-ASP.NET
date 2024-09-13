using RiotAPI.Models;
using RiotAPI.Models.Entities;

namespace RiotAPI.Services
{
    public interface IUserService
    {
        Task<PuuidResponse> GetPuuidAsync(string gameName, string tagLine);

        Task<List<string>> GetMatchIdsAsync(string puuid);

        Task<MatchData> GetMatchDataAsync(string gameName, string tagLine, string matchId);
    }
}
