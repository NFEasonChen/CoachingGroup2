using System;
using System.Collections.Generic;

namespace CoachingProject
{
    public interface IMatchRepo
    {
        Match GetMatch(int matchId);
        Match UpdateMatch(int matchId, MatchEvent matchEvent);
    }

    public enum MatchEvent
    {
        HomeGoal,
        AwayGoal,
        CancelHomeGoal,
        CancelAwayGoal,
        NextPeriod
    }

    // Simple Match model for demonstration
    public class Match
    {
        public int Id { get; set; }
        
        public string Scores { get; set; }
    }
}