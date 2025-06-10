using CoachingProject.Models;

namespace CoachingProject.Repositories
{
    public interface IMatchRepository
    {
        Match GetMatch(int matchId);
        Match UpdateMatch(int matchId, string scores);
    }
}