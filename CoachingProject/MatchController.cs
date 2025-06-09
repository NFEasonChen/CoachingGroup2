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
            
            var matchScore = match.Scores;
            
            matchScore = matchEvent switch
            {
                MatchEvent.HomeGoal => matchScore + "H",
                MatchEvent.AwayGoal => matchScore + "A",
                MatchEvent.CancelHomeGoal =>
                    matchScore.Length > 0 && matchScore[^1] == ';'
                        ? (matchScore.Length > 1 && matchScore[^2] == 'A'
                            ? throw new InvalidOperationException("Cannot cancel home goal when last event is away goal.")
                            : (matchScore.Length > 1 && matchScore[^2] == 'H'
                                ? matchScore.Remove(matchScore.Length - 2, 1)
                                : matchScore))
                        : (matchScore.Length > 0 && matchScore[^1] == 'A'
                            ? throw new InvalidOperationException("Cannot cancel home goal when last event is away goal.")
                            : (matchScore.EndsWith("H") ? matchScore[..^1] : matchScore)),
                _ => matchScore
            };

            // What happen if we have xx dupplicated request
            var updatedMatch = matchRepo.UpdateMatch(matchId, matchScore);
            // 2. logic to put "H" -> "1:0 (First Half)"

            
            return updatedMatch.GetScoreResult();
        }
    }
} 