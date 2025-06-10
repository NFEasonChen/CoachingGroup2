using CoachingProject.Enums;

namespace CoachingProject.Models;

public class Match
{
    public int Id { get; set; }
        
    public required string Scores { get; set; }

    public void HandleEvent(MatchEvent matchEvent)
    {
        switch (matchEvent)
        {
            case MatchEvent.HomeGoal:
                Scores += "H";
                break;
            case MatchEvent.AwayGoal:
                Scores += "A";
                break;
            case MatchEvent.CancelHomeGoal:
                if (Scores.Length > 0 && Scores[^1] == ';')
                {
                    if (Scores.Length > 1 && Scores[^2] == 'A')
                        throw new InvalidOperationException("Cannot cancel home goal when last event is away goal.");
                    else if (Scores.Length > 1 && Scores[^2] == 'H')
                        Scores = Scores.Remove(Scores.Length - 2, 1);
                }
                else if (Scores.Length > 0 && Scores[^1] == 'A')
                {
                    throw new InvalidOperationException("Cannot cancel home goal when last event is away goal.");
                }
                else if (Scores.EndsWith("H"))
                {
                    Scores = Scores[..^1];
                }
                break;
            case MatchEvent.CancelAwayGoal:
                if (Scores.Length > 0 && Scores[^1] == ';')
                {
                    if (Scores.Length > 1 && Scores[^2] == 'H')
                        throw new InvalidOperationException("Cannot cancel away goal when last event is home goal.");
                    else if (Scores.Length > 1 && Scores[^2] == 'A')
                        Scores = Scores.Remove(Scores.Length - 2, 1);
                }
                else if (Scores.Length > 0 && Scores[^1] == 'H')
                {
                    throw new InvalidOperationException("Cannot cancel away goal when last event is home goal.");
                }
                else if (Scores.EndsWith("A"))
                {
                    Scores = Scores[..^1];
                }
                break;
            case MatchEvent.NextPeriod:
                Scores += ";";
                break;
            default:
                break;
        }
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