using System;
using System.Collections.Generic;

namespace CoachingProject
{
    public interface IMatchRepo
    {
        Match GetMatch(int matchId);
        Match UpdateMatch(int matchId, string Scores);
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

        public string HandleEvent(MatchEvent matchEvent)
        {
             return matchEvent switch
            {
                MatchEvent.HomeGoal => Scores + "H",
                MatchEvent.AwayGoal => Scores + "A",
                MatchEvent.CancelHomeGoal =>
                    Scores.Length > 0 && Scores[^1] == ';'
                        ? (Scores.Length > 1 && Scores[^2] == 'A'
                            ? throw new InvalidOperationException("Cannot cancel home goal when last event is away goal.")
                            : (Scores.Length > 1 && Scores[^2] == 'H'
                                ? Scores.Remove(Scores.Length - 2, 1)
                                : Scores))
                        : (Scores.Length > 0 && Scores[^1] == 'A'
                            ? throw new InvalidOperationException("Cannot cancel home goal when last event is away goal.")
                            : (Scores.EndsWith("H") ? Scores[..^1] : Scores)),
                MatchEvent.CancelAwayGoal =>
                    Scores.Length > 0 && Scores[^1] == ';'
                        ? (Scores.Length > 1 && Scores[^2] == 'H'
                            ? throw new InvalidOperationException("Cannot cancel away goal when last event is home goal.")
                            : (Scores.Length > 1 && Scores[^2] == 'A'
                                ? Scores.Remove(Scores.Length - 2, 1)
                                : Scores))
                        : (Scores.Length > 0 && Scores[^1] == 'H'
                            ? throw new InvalidOperationException("Cannot cancel away goal when last event is home goal.")
                            : (Scores.EndsWith("A") ? Scores[..^1] : Scores)),
                _ => Scores
            };
        }

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