using CoachingProject.Enums;
using CoachingProject.Repositories;
using CoachingProject.Models;

namespace CoachingProject.Services
{
    public class MatchService(IMatchRepository matchRepository):IMatchService
    {
        /// <summary>
        /// Handles updating a match with a new event and returns the updated match.
        /// </summary>
        public async Task<Match> UpdateMatch(int matchId, MatchEvent matchEvent)
        {
            var match = await matchRepository.GetMatchAsync(matchId);
            
            match.HandleEvent(matchEvent);
            
            await matchRepository.UpdateMatchAsync(matchId, match.Scores);
            
            return match;
        }
    }
} 