using CoachingProject.Models;

namespace CoachingProject.Repositories
{
    public interface IMatchRepository
    {
        Task<Match> GetMatchAsync(int matchId);
        Task UpdateMatchAsync(Match match);
    }
}