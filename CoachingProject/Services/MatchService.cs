using CoachingProject.Enums;
using CoachingProject.Repositories;
using CoachingProject.Models;
using System.Threading.Tasks;

namespace CoachingProject.Services
{
    public class MatchService(IMatchRepository matchRepo):IMatchService
    {
        /// <summary>
        /// Handles updating a match with a new event and returns the updated match.
        /// </summary>
        public async Task<Match> UpdateMatch(int matchId, MatchEvent matchEvent)
        {
            var match = await matchRepo.GetMatchAsync(matchId);
            var score = match.HandleEvent(matchEvent);
            var updatedMatch = await matchRepo.UpdateMatchAsync(matchId, score);
            return updatedMatch;
        }
    }
} 