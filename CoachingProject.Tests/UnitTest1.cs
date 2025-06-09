using System.Reflection.Metadata;
using NUnit.Framework;
using CoachingProject;
using NSubstitute;

namespace CoachingProject.Tests
{
    public class MatchControllerTests
    {
        private const int matchId = 91;
        
        [Test]
        public void UpdateMatch_WhenHomeGoal_FirstTimeInFH_From0to0()
        {
            // Arrange
            var matchEvent = MatchEvent.HomeGoal;
            var matchRepo = Substitute.For<IMatchRepo>();
            var match = new Match
            {
                Id = matchId,
                Scores = string.Empty
            };
            matchRepo.GetMatch(matchId).Returns(match);
            
            matchRepo.UpdateMatch(match.Id, "H").Returns(new Match
            {
                Id = matchId,
                Scores = "H"
            });
            
            var controller = new MatchController(matchRepo);

            // Act
            var result = controller.UpdateMatch(matchId, matchEvent); // 1:0 (First Half)
            
            // Assert
            // Assert.Equals(result, "1:0 (First Half)"); // NoContent returns null as string
            Assert.That(result, Is.EqualTo("1:0 (First Half)"));

            matchRepo.Received(1).UpdateMatch(match.Id, "H");
            matchRepo.Received(1).GetMatch(matchId);
        }
        
        [Test]
        public void UpdateMatch_WhenAwayGoal_InFH_From1to0()
        {
            // Arrange
            var matchEvent = MatchEvent.AwayGoal;
            var matchRepo = Substitute.For<IMatchRepo>();
            var match = new Match
            {
                Id = matchId,
                Scores = "H"
            };
            matchRepo.GetMatch(matchId).Returns(match);
            
            matchRepo.UpdateMatch(match.Id, "HA").Returns(new Match
            {
                Id = matchId,
                Scores = "HA"
            });
            
            var controller = new MatchController(matchRepo);

            // Act
            var result = controller.UpdateMatch(matchId, matchEvent); // 1:0 (First Half)
            
            // Assert
            // Assert.Equals(result, "1:0 (First Half)"); // NoContent returns null as string
            Assert.That(result, Is.EqualTo("1:1 (First Half)"));

            matchRepo.Received(1).UpdateMatch(match.Id, "HA");
            matchRepo.Received(1).GetMatch(matchId);
        }

    }
}