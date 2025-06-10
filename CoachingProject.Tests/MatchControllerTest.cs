using CoachingProject.Controller;
using CoachingProject.Enums;
using CoachingProject.Models;
using CoachingProject.Repositories;
using CoachingProject.Services;
using NSubstitute;

namespace CoachingProject.Tests
{
    [TestFixture]
    public class MatchControllerTests
    {
        private const int MatchId = 91;
        private IMatchRepository _matchRepo;
        private IMatchService _matchService;
        private MatchController _controller;

        [SetUp]
        public void SetUp()
        {
            _matchRepo = Substitute.For<IMatchRepository>();
            _matchService = new MatchService(_matchRepo);
            _controller = new MatchController(_matchService);
        }
        
        private void SetupMatch(int id, string scores)
        {
            var match = new Match
            {
                Id = id,
                Scores = scores
            };
            _matchRepo.GetMatchAsync(id).Returns(Task.FromResult(match));
        }

        private void SetupUpdateMatch(int id, string newScores)
        {
            _matchRepo.UpdateMatchAsync(id, newScores).Returns(Task.FromResult(new Match
            {
                Id = id,
                Scores = newScores
            }));
        }

        [Test]
        public async Task UpdateMatch_WhenHomeGoal_FirstTimeInFH_From0to0()
        {
            // Arrange
            SetupMatch(MatchId, string.Empty);
            SetupUpdateMatch(MatchId, "H");
            var matchEvent = MatchEvent.HomeGoal;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("1:0 (First Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, "H");
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [Test]
        public async Task UpdateMatch_WhenAwayGoal_InFH_From1to0()
        {
            // Arrange
            SetupMatch(MatchId, "H");
            SetupUpdateMatch(MatchId, "HA");
            var matchEvent = MatchEvent.AwayGoal;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("1:1 (First Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, "HA");
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [Test]
        public async Task UpdateMatch_WhenCancelHomeGoal_InFH_From1to0()
        {
            // Arrange
            SetupMatch(MatchId, "H");
            SetupUpdateMatch(MatchId, string.Empty);
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (First Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, string.Empty);
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [Test]
        public async Task UpdateMatch_WhenCancelHomeGoal_LastIsAwayGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "A");
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public async Task UpdateMatch_WhenCancelHomeGoal_LastIsSemicolonAndBeforeIsAwayGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "A;");
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public async Task UpdateMatch_WhenCancelHomeGoal_LastIsSemicolonAndBeforeIsHomeGoal_RemovesHomeGoal()
        {
            // Arrange
            SetupMatch(MatchId, "H;");
            SetupUpdateMatch(MatchId, ";");
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (Second Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, ";");
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [Test]
        public async Task UpdateMatch_WhenCancelAwayGoal_InFH_From0to1()
        {
            // Arrange
            SetupMatch(MatchId, "A");
            SetupUpdateMatch(MatchId, string.Empty);
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (First Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, string.Empty);
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [Test]
        public async Task UpdateMatch_WhenCancelAwayGoal_LastIsHomeGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "H");
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public async Task UpdateMatch_WhenCancelAwayGoal_LastIsSemicolonAndBeforeIsHomeGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "H;");
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public async Task UpdateMatch_WhenCancelAwayGoal_LastIsSemicolonAndBeforeIsAwayGoal_RemovesAwayGoal()
        {
            // Arrange
            SetupMatch(MatchId, "A;");
            SetupUpdateMatch(MatchId, ";");
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (Second Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, ";");
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [Test]
        public async Task UpdateMatch_WhenNextPeriod_AppendsSemicolon()
        {
            // Arrange
            SetupMatch(MatchId, "HA");
            SetupUpdateMatch(MatchId, "HA;");
            var matchEvent = MatchEvent.NextPeriod;

            // Act
            var result = await _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("1:1 (Second Half)"));
            await _matchRepo.Received(1).UpdateMatchAsync(MatchId, "HA;");
            await _matchRepo.Received(1).GetMatchAsync(MatchId);
        }

        [TearDown]
        public void TearDown()
        {
            _matchRepo = null;
            _matchService = null;
            _controller = null;
        }
    }
}