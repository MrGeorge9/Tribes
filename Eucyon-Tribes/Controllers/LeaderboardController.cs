using Eucyon_Tribes.Models;
using Eucyon_Tribes.Services;
using Microsoft.AspNetCore.Mvc;

namespace Eucyon_Tribes.Controllers
{
    [Route("api/leaderboard")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly IKingdomService _kingdomService;
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardController(IKingdomService kingdomService, ILeaderboardService leaderboardService)
        {
            this._kingdomService = kingdomService;
            this._leaderboardService = leaderboardService;
        }

        [HttpGet("buildings")]
        public IActionResult GetLeaderboardByBuildings()
        {
            List<Kingdom> kingdoms = _kingdomService.GetAllKingdoms();
            var leaderboard = _leaderboardService.GetBuildingLeaderboard(kingdoms);
            if (leaderboard == null || leaderboard.Leaderboard.Count == 0)
            {
                return StatusCode(500, leaderboard);
            }
            return Ok(leaderboard);
        }

        [HttpGet("soldiers")]
        public IActionResult GetLeaderboardBySoldiers()
        {
            List<Kingdom> kingdoms = _kingdomService.GetAllKingdoms();
            var leaderboard = _leaderboardService.GetSoldierLeaderboard(kingdoms);
            if (leaderboard == null || leaderboard.Leaderboard.Count == 0)
            {
                return StatusCode(500, leaderboard);
            }
            return Ok(leaderboard);
        }
    }
}
