using Xle.Maps.Outdoors;
using Xle.Services.Commands.Implementation;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Moq;
using System.Linq;
using Xunit;

namespace Xle.Commands
{
    public class DisembarkTest : XleTest
    {
        private Disembark disembark = new Disembark();
        private Mock<IXleScreen> screen = new Mock<IXleScreen>();
        private Mock<ISoundMan> soundMan = new Mock<ISoundMan>();
        private Mock<IXleInput> input = new Mock<IXleInput>();
        private Mock<IOutsideExtender> outsideExtender = new Mock<IOutsideExtender>();

        public DisembarkTest()
        {
            InitializeCommand(disembark);

            disembark.Screen = screen.Object;
            disembark.SoundMan = soundMan.Object;
            disembark.Input = input.Object;

            GameState.MapExtender = outsideExtender.Object;
        }

        private void SetKeys(params Keys[] keys)
        {
            var sequence = input.SetupSequence(
                    x => x.WaitForKey(It.IsAny<Keys[]>()));

            foreach (var key in keys)
            {
                sequence = sequence.Returns(key);
            }
        }

        [Fact]
        public void Disembark()
        {
            SetKeys(Keys.Right);

            Player.Rafts.Add(new Xle.RaftData(18, 18, 1));
            Player.BoardedRaft = Player.Rafts.First();

            Player.BoardedRaft.Should().NotBeNull("Player is not on a raft.");

            disembark.Execute();

            Player.BoardedRaft.Should().BeNull("Player did not disembark.");
        }
    }
}
