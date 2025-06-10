using CoachingProject.Models;
using System.Collections.Concurrent;

namespace CoachingProject.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly ConcurrentDictionary<int, Match> _matches = new();

        public Task<Match> GetMatchAsync(int matchId)
        {
            _matches.TryGetValue(matchId, out var match);
            // Return a dummy match if not found for demonstration
            return Task.FromResult(match ?? new Match { Id = matchId, Scores = string.Empty });
        }

        public Task UpdateMatchAsync(int matchId, string scores)
        {
            var match = new Match { Id = matchId, Scores = scores };
            _matches[matchId] = match;
            return Task.CompletedTask;
        }
    }
} 