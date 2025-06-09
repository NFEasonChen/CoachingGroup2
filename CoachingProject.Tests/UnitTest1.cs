using NUnit.Framework;
using CoachingProject;

namespace CoachingProject.Tests
{
    public class MatchControllerTests
    {
        [Test]
        public void UpdateMatch_WhenHomeGoal_FirstTimeInFH_From0to0()
        {
            // Arrange
            var controller = new MatchController();

            var matchId = 91;
            var matchEvent = MatchEvent.HomeGoal;

            // Act
            var result = controller.UpdateMatch(matchId, matchEvent); // 1:0 (First Half)

            // Assert
            Assert.Equals(result, "1:0 (First Half)"); // NoContent returns null as string
        }
    }
}