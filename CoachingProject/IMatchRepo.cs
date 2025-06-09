using System;
using System.Collections.Generic;

namespace CoachingProject
{
    public interface IMatchRepo
    {
        Match GetMatch(int matchId);
        Match UpdateMatch(int matchId, string matchScore);
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

        public string GetScoreResult()
        {
            if (string.IsNullOrEmpty(Scores))
            {
                return "0:0 (First Half)";
            }
            var homeScore = 0;
            var awayScore = 0;
            var isFirstHalf = true;
            foreach (var score in Scores)
            {
                switch (score)
                {
                    case 'H':
                        homeScore++; 
                        break;
                    case 'A':
                        awayScore++;
                        break;
                    case ';':
                        isFirstHalf = false;
                        break;
                }
                
            }
            return $"{homeScore}:{awayScore} ({(isFirstHalf ? "First Half" : "Second Half")})";
            
        }
    }
}