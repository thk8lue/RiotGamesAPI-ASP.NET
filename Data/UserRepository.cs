using System;
using System.Data.SqlClient;
using RiotAPI.Models;
using RiotAPI.Models.Entities;

namespace RiotAPI.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> GetPuuidAsync(string gameName, string tagLine)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT puuid FROM TBL_USER WHERE gameName = @gameName AND tagLine = @tagLine";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@gameName", gameName);
                    command.Parameters.AddWithValue("@tagLine", tagLine);

                    return await command.ExecuteScalarAsync() as string;
                }
            }
        }

        public async Task SaveUserAsync(string gameName, string tagLine, string puuid)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var insertQuery = "INSERT INTO TBL_USER (gameName, tagLine, puuid) VALUES (@gameName, @tagLine, @puuid)";
                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@gameName", gameName);
                    insertCommand.Parameters.AddWithValue("@tagLine", tagLine);
                    insertCommand.Parameters.AddWithValue("@puuid", puuid);

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<string>> GetMatchIdsAsync(string puuid)
        {
            var matchIds = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT TOP 100 matchid FROM TBL_MATCH WHERE puuid = @puuid ORDER BY matchid DESC";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@puuid", puuid);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            matchIds.Add(reader["matchid"].ToString());
                        }
                    }
                }
            }

            return matchIds;
        }

        public async Task SaveMatchIdsAsync(string puuid, string matchId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var insertQuery = "INSERT INTO TBL_MATCH (puuid, matchid) VALUES (@puuid, @matchid)";
                using (var insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@puuid", puuid);
                    insertCommand.Parameters.AddWithValue("@matchid", matchId);

                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<MatchData> GetMatchDataAsync(string puuid, string matchId)
        {
                var matchData = new MatchData();

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT * FROM TBL_MATCH WHERE puuid = @puuid AND matchid = @matchid";
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@puuid", puuid);
                        command.Parameters.AddWithValue("@matchid", matchId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                matchData.matchid = reader["matchid"].ToString();
                                matchData.puuid = reader["puuid"].ToString();
                                matchData.participantsindex = reader["participantsindex"] != DBNull.Value ? Convert.ToInt32(reader["participantsindex"]) : 0;
                                matchData.teamindex = reader["teamindex"] != DBNull.Value ? Convert.ToInt32(reader["teamindex"]) : 0;
                                matchData.iswinner = reader["iswinner"] != DBNull.Value ? Convert.ToBoolean(reader["iswinner"]) : false;
                                matchData.championname = reader["championname"]?.ToString();
                                matchData.gamemodeid = reader["gamemodeid"] != DBNull.Value ? Convert.ToInt32(reader["gamemodeid"]) : 0;
                                matchData.kills = reader["kills"] != DBNull.Value ? Convert.ToInt32(reader["kills"]) : 0;
                                matchData.deaths = reader["deaths"] != DBNull.Value ? Convert.ToInt32(reader["deaths"]) : 0;
                                matchData.assists = reader["assists"] != DBNull.Value ? Convert.ToInt32(reader["assists"]) : 0;
                                matchData.spell1id = reader["spell1id"] != DBNull.Value ? Convert.ToInt32(reader["spell1id"]) : 0;
                                matchData.spell2id = reader["spell2id"] != DBNull.Value ? Convert.ToInt32(reader["spell2id"]) : 0;
                                matchData.runemainid = reader["runemainid"] != DBNull.Value ? Convert.ToInt32(reader["runemainid"]) : 0;
                                matchData.runesubid = reader["runesubid"] != DBNull.Value ? Convert.ToInt32(reader["runesubid"]) : 0;
                                matchData.item0id = reader["item0id"] != DBNull.Value ? Convert.ToInt32(reader["item0id"]) : 0;
                                matchData.item1id = reader["item1id"] != DBNull.Value ? Convert.ToInt32(reader["item1id"]) : 0;
                                matchData.item2id = reader["item2id"] != DBNull.Value ? Convert.ToInt32(reader["item2id"]) : 0;
                                matchData.item3id = reader["item3id"] != DBNull.Value ? Convert.ToInt32(reader["item3id"]) : 0;
                                matchData.item4id = reader["item4id"] != DBNull.Value ? Convert.ToInt32(reader["item4id"]) : 0;
                                matchData.item5id = reader["item5id"] != DBNull.Value ? Convert.ToInt32(reader["item5id"]) : 0;
                                matchData.item6id = reader["item6id"] != DBNull.Value ? Convert.ToInt32(reader["item6id"]) : 0;
                            }
                        }
                    }
                }

                return matchData;
        }

        public async Task SaveMatchDataAsync(MatchData matchData)
        {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    var updateQuery = @"
            UPDATE TBL_MATCH
            SET puuid = @puuid,
                matchid = @matchid,
                participantsindex = @participantsindex,
                teamindex = @teamindex,
                iswinner = @iswinner,
                championname = @championname,
                gamemodeid = @gamemodeid,
                kills = @kills,
                deaths = @deaths,
                assists = @assists,
                spell1id = @spell1id,
                spell2id = @spell2id,
                runemainid = @runemainid,
                runesubid = @runesubid,
                item0id = @item0id,
                item1id = @item1id,
                item2id = @item2id,
                item3id = @item3id,
                item4id = @item4id,
                item5id = @item5id,
                item6id = @item6id
            WHERE puuid = @puuid AND matchid = @matchid";

                    using (var updateCommand = new SqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@puuid", matchData.puuid);
                        updateCommand.Parameters.AddWithValue("@matchid", matchData.matchid);
                        updateCommand.Parameters.AddWithValue("@participantsindex", matchData.participantsindex);
                        updateCommand.Parameters.AddWithValue("@teamindex", matchData.teamindex);
                        updateCommand.Parameters.AddWithValue("@iswinner", matchData.iswinner);
                        updateCommand.Parameters.AddWithValue("@championname", matchData.championname);
                        updateCommand.Parameters.AddWithValue("@gamemodeid", matchData.gamemodeid);
                        updateCommand.Parameters.AddWithValue("@kills", matchData.kills);
                        updateCommand.Parameters.AddWithValue("@deaths", matchData.deaths);
                        updateCommand.Parameters.AddWithValue("@assists", matchData.assists);
                        updateCommand.Parameters.AddWithValue("@spell1id", matchData.spell1id);
                        updateCommand.Parameters.AddWithValue("@spell2id", matchData.spell2id);
                        updateCommand.Parameters.AddWithValue("@runemainid", matchData.runemainid);
                        updateCommand.Parameters.AddWithValue("@runesubid", matchData.runesubid);
                        updateCommand.Parameters.AddWithValue("@item0id", matchData.item0id);
                        updateCommand.Parameters.AddWithValue("@item1id", matchData.item1id);
                        updateCommand.Parameters.AddWithValue("@item2id", matchData.item2id);
                        updateCommand.Parameters.AddWithValue("@item3id", matchData.item3id);
                        updateCommand.Parameters.AddWithValue("@item4id", matchData.item4id);
                        updateCommand.Parameters.AddWithValue("@item5id", matchData.item5id);
                        updateCommand.Parameters.AddWithValue("@item6id", matchData.item6id);

                        await updateCommand.ExecuteNonQueryAsync();
                    }
                }
        }
    }
}
