using NSubstitute;

namespace CoachingProject.Tests
{
    public class MatchControllerTests
    {
        private const int MatchId = 91;
        private IMatchRepo _matchRepo;
        private MatchController _controller;

        [SetUp]
        public void SetUp()
        {
            _matchRepo = Substitute.For<IMatchRepo>();
            _controller = new MatchController(_matchRepo);
        }
        
        private void SetupMatch(int id, string scores)
        {
            var match = new Match
            {
                Id = id,
                Scores = scores
            };
            _matchRepo.GetMatch(id).Returns(match);
        }

        private void SetupUpdateMatch(int id, string newScores)
        {
            _matchRepo.UpdateMatch(id, newScores).Returns(new Match
            {
                Id = id,
                Scores = newScores
            });
        }

        [Test]
        public void UpdateMatch_WhenHomeGoal_FirstTimeInFH_From0to0()
        {
            // Arrange
            SetupMatch(MatchId, string.Empty);
            SetupUpdateMatch(MatchId, "H");
            var matchEvent = MatchEvent.HomeGoal;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("1:0 (First Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, "H");
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [Test]
        public void UpdateMatch_WhenAwayGoal_InFH_From1to0()
        {
            // Arrange
            SetupMatch(MatchId, "H");
            SetupUpdateMatch(MatchId, "HA");
            var matchEvent = MatchEvent.AwayGoal;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("1:1 (First Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, "HA");
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [Test]
        public void UpdateMatch_WhenCancelHomeGoal_InFH_From1to0()
        {
            // Arrange
            SetupMatch(MatchId, "H");
            SetupUpdateMatch(MatchId, string.Empty);
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (First Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, string.Empty);
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [Test]
        public void UpdateMatch_WhenCancelHomeGoal_LastIsAwayGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "A");
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public void UpdateMatch_WhenCancelHomeGoal_LastIsSemicolonAndBeforeIsAwayGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "A;");
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public void UpdateMatch_WhenCancelHomeGoal_LastIsSemicolonAndBeforeIsHomeGoal_RemovesHomeGoal()
        {
            // Arrange
            SetupMatch(MatchId, "H;");
            SetupUpdateMatch(MatchId, ";");
            var matchEvent = MatchEvent.CancelHomeGoal;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (Second Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, ";");
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [Test]
        public void UpdateMatch_WhenCancelAwayGoal_InFH_From0to1()
        {
            // Arrange
            SetupMatch(MatchId, "A");
            SetupUpdateMatch(MatchId, string.Empty);
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (First Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, string.Empty);
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [Test]
        public void UpdateMatch_WhenCancelAwayGoal_LastIsHomeGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "H");
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public void UpdateMatch_WhenCancelAwayGoal_LastIsSemicolonAndBeforeIsHomeGoal_ThrowsException()
        {
            // Arrange
            SetupMatch(MatchId, "H;");
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _controller.UpdateMatch(MatchId, matchEvent));
        }

        [Test]
        public void UpdateMatch_WhenCancelAwayGoal_LastIsSemicolonAndBeforeIsAwayGoal_RemovesAwayGoal()
        {
            // Arrange
            SetupMatch(MatchId, "A;");
            SetupUpdateMatch(MatchId, ";");
            var matchEvent = MatchEvent.CancelAwayGoal;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("0:0 (Second Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, ";");
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [Test]
        public void UpdateMatch_WhenNextPeriod_AppendsSemicolon()
        {
            // Arrange
            SetupMatch(MatchId, "HA");
            SetupUpdateMatch(MatchId, "HA;");
            var matchEvent = MatchEvent.NextPeriod;

            // Act
            var result = _controller.UpdateMatch(MatchId, matchEvent);

            // Assert
            Assert.That(result, Is.EqualTo("1:1 (Second Half)"));
            _matchRepo.Received(1).UpdateMatch(MatchId, "HA;");
            _matchRepo.Received(1).GetMatch(MatchId);
        }

        [TearDown]
        public void TearDown()
        {
            _matchRepo = null;
            _controller = null;
        }
    }
}