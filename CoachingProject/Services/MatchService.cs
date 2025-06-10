using CoachingProject.Enums;
using CoachingProject.Repositories;
using CoachingProject.Models;

namespace CoachingProject.Services
{
    public class MatchService(IMatchRepository matchRepo):IMatchService
    {
        /// <summary>
        /// Handles updating a match with a new event and returns the updated match.
        /// </summary>
        public Match UpdateMatch(int matchId, MatchEvent matchEvent)
        {
            var match = matchRepo.GetMatch(matchId);
            var score = match.HandleEvent(matchEvent);
            var updatedMatch = matchRepo.UpdateMatch(matchId, score);
            return updatedMatch;
        }
    }
} 