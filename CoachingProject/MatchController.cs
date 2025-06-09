using Microsoft.AspNetCore.Mvc;
using System;

namespace CoachingProject
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController(IMatchRepo matchRepo) : ControllerBase
    {
        /// <summary>
        /// Updates a match event by match ID.
        /// </summary>
        [HttpPost]
        public string UpdateMatch(int matchId, MatchEvent matchEvent)
        {
            var match = matchRepo.GetMatch(matchId);

            var score= match.HandleEvent(matchEvent);

            // What happen if we have xx dupplicated request
            var updatedMatch = matchRepo.UpdateMatch(matchId, score);
           
            // 2. logic to put "H" -> "1:0 (First Half)"
            return updatedMatch.GetScoreResult();
        }
    }
}