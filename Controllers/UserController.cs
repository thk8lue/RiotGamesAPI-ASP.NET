using Microsoft.AspNetCore.Mvc;
using RiotAPI.Services;
using System.Data.SqlClient;
using System.Text.Json;

namespace RiotAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService service)
        {
            userService = service;
        }

        [HttpGet("/get-puuid/{gameName}/{tagLine}")]
        public async Task<IActionResult> GetPuuid(string gameName, string tagLine)
        {
            try
            {
                var result = await userService.GetPuuidAsync(gameName, tagLine);

                if (result == null)
                    return NotFound(new { message = "User not found." });

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return Problem($"Error calling Riot API: {ex.Message}", statusCode: 500);
            }
            catch (SqlException ex)
            {
                return Problem($"Database error: {ex.Message}", statusCode: 500);
            }
            catch (JsonException ex)
            {
                return Problem($"Error parsing Riot API response: {ex.Message}", statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("/get-MatchIds/{puuid}")]
        public async Task<IActionResult> GetMatchIds(string puuid)
        {
            try
            {
                var result = await userService.GetMatchIdsAsync(puuid);

                if (result == null)
                    return NotFound(new { message = "User not found." });

                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return Problem($"Error calling Riot API: {ex.Message}", statusCode: 500);
            }
            catch (SqlException ex)
            {
                return Problem($"Database error: {ex.Message}", statusCode: 500);
            }
            catch (JsonException ex)
            {
                return Problem($"Error parsing Riot API response: {ex.Message}", statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("get-Match/{gameName}/{tagLine}/{matchId}")]
        public async Task<IActionResult> GetMatchData(string gameName, string tagLine, string matchId)
        {
            try
            {
                var result = await userService.GetMatchDataAsync(gameName, tagLine, matchId);

            if (result == null)
                return NotFound(new { message = "Match not found." });

            return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return Problem($"Error calling Riot API: {ex.Message}", statusCode: 500);
            }
            catch (SqlException ex)
            {
                return Problem($"Database error: {ex.Message}", statusCode: 500);
            }
            catch (JsonException ex)
            {
                return Problem($"Error parsing Riot API response: {ex.Message}", statusCode: 500);
            }
            catch (Exception ex)
            {
                return Problem($"An unexpected error occurred: {ex.Message}", statusCode: 500);
            }
        }
    }
}
