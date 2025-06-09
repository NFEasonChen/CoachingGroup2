using Microsoft.AspNetCore.Mvc;

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
            
            var matchScore = match.Scores;
            matchScore = matchEvent switch
            {
                MatchEvent.HomeGoal => matchScore + "H",
                MatchEvent.AwayGoal => matchScore + "A",
                _ => matchScore
            };

            // What happen if we have xx dupplicated request
            var updatedMatch = matchRepo.UpdateMatch(matchId, matchScore);
            // 2. logic to put "H" -> "1:0 (First Half)"

            
            return updatedMatch.GetScoreResult();
        }
    }
} 