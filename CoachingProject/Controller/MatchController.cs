using CoachingProject.Enums;
using Microsoft.AspNetCore.Mvc;
using CoachingProject.Services;

namespace CoachingProject.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController(IMatchService matchService) : ControllerBase
    {
        /// <summary>
        /// Updates a match event by match ID.
        /// </summary>
        [HttpPost]
        public async Task<string> UpdateMatch(int matchId, MatchEvent matchEvent)
        {
            var updatedMatch = await matchService.UpdateMatch(matchId, matchEvent);
            return updatedMatch.GetScoreResult();
        }
    }
}