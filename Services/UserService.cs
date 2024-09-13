using System.Text.Json;
using RiotAPI.Data;
using RiotAPI.Models;
using RiotAPI.Models.Entities;

namespace RiotAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly string apiKey;

        public UserService(IUserRepository repository, IConfiguration configuration)
        {
            userRepository = repository;
            apiKey = configuration["RiotApiKey"];
        }

        public async Task<PuuidResponse> GetPuuidAsync(string gameName, string tagLine)
        {
            var puuid = await userRepository.GetPuuidAsync(gameName, tagLine);

            if (puuid == null)
            {
                var client = new HttpClient();
                var url = $"https://asia.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gameName}/{tagLine}?api_key={apiKey}";

                var response = await client.GetStringAsync(url);
                var riotData = JsonSerializer.Deserialize<PuuidResponse>(response);

                if (riotData?.puuid != null)
                {
                    await userRepository.SaveUserAsync(gameName, tagLine, riotData.puuid);
                }

                return riotData;
            }

            return new PuuidResponse
            {
                puuid = puuid,
                gameName = gameName,
                tagLine = tagLine
            };
        }

        public async Task<List<string>> GetMatchIdsAsync(string puuid)
        {
            var matchIds = await userRepository.GetMatchIdsAsync(puuid);

            if (matchIds == null || matchIds.Count == 0)
            {
                var client = new HttpClient();
                var url = $"https://asia.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids?start=0&count=100&api_key={apiKey}";

                var response = await client.GetStringAsync(url);
                var riotData = JsonSerializer.Deserialize<List<string>>(response);

                if (riotData != null)
                {
                    foreach (var matchId in riotData)
                    {
                        await userRepository.SaveMatchIdsAsync(puuid, matchId);
                    }
                }

                return riotData;
            }

            return matchIds;
        }

        public async Task<MatchData> GetMatchDataAsync(string gameName, string tagLine, string matchId)
        {
                var puuid = await userRepository.GetPuuidAsync(gameName, tagLine);
                var matchData = await userRepository.GetMatchDataAsync(puuid, matchId);

            if (matchData.championname == null)
            {
                var client = new HttpClient();
                var url = $"https://asia.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={apiKey}";

                var response = await client.GetStringAsync(url);
                var riotData = JsonSerializer.Deserialize<JsonElement>(response);

                matchData.puuid = puuid;
                matchData.matchid = matchId;

                var metadata = riotData.GetProperty("metadata");
                var info = riotData.GetProperty("info");
                var participants = info.GetProperty("participants");
                var participantData = participants[matchData.participantsindex];

                // participants 배열에서 puuid와 일치하는 인덱스를 찾아서 matchData 객체에 저장
                int participantsIndex = -1;
                var participantArray = metadata.GetProperty("participants");
                for (int i = 0; i < participantArray.GetArrayLength(); i++)
                {
                    if (participantArray[i].GetString() == puuid)
                    {
                        participantsIndex = i;
                        break;
                    }
                }
                // puuid와 일치하는 참가자가 없을 경우 예외 처리
                if (participantsIndex == -1)
                {
                    throw new Exception("Participants not found for the given puuid.");
                }
                matchData.participantsindex = participantsIndex;

                // matchData에 팀 인덱스 저장
                matchData.teamindex = participantsIndex < 5 ? 0 : 1;

                // matchData에 승패 저장
                matchData.iswinner = info.GetProperty("teams")[matchData.teamindex].GetProperty("win").GetBoolean();

                // 챔피언 이름 저장
                matchData.championname = participantData.GetProperty("championName").GetString();

                // 게임 모드 id 저장
                matchData.gamemodeid = info.GetProperty("queueId").GetInt32();

                // 킬, 데스, 어시스트
                matchData.kills = participantData.GetProperty("kills").GetInt32();
                matchData.deaths = participantData.GetProperty("deaths").GetInt32();
                matchData.assists = participantData.GetProperty("assists").GetInt32();

                // 스펠
                matchData.spell1id = participantData.GetProperty("summoner1Id").GetInt32();
                matchData.spell2id = participantData.GetProperty("summoner2Id").GetInt32();

                // 룬
                var perks = participantData.GetProperty("perks").GetProperty("styles");
                matchData.runemainid = perks[0].GetProperty("selections").GetProperty("perk").GetInt32();
                matchData.runesubid = perks[1].GetProperty("style").GetInt32();

                // 아이템
                matchData.item0id = participantData.GetProperty("item0").GetInt32();
                matchData.item1id = participantData.GetProperty("item1").GetInt32();
                matchData.item2id = participantData.GetProperty("item2").GetInt32();
                matchData.item3id = participantData.GetProperty("item3").GetInt32();
                matchData.item4id = participantData.GetProperty("item4").GetInt32();
                matchData.item5id = participantData.GetProperty("item5").GetInt32();
                matchData.item6id = participantData.GetProperty("item6").GetInt32();

                await userRepository.SaveMatchDataAsync(matchData);
                return matchData;
            }

            return matchData;
        }
    }
}
