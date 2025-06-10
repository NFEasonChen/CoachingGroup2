using CoachingProject.Enums;
using CoachingProject.Models;

namespace CoachingProject.Services;

public interface IMatchService
{
    Task<Match> UpdateMatch(int matchId, MatchEvent matchEvent);
}