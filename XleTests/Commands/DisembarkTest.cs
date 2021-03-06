﻿using Xle.Maps.Outdoors;
using Xle.Commands.Implementation;
using Xle.ScreenModel;
using Xle.XleSystem;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Moq;
using System.Linq;
using Xunit;
using System.Threading.Tasks;
using Xle.Game;

namespace Xle.Commands
{
    public class DisembarkTest : XleTest
    {
        private Disembark disembark = new Disembark();
        private Mock<IXleScreen> screen = new Mock<IXleScreen>();
        private Mock<ISoundMan> soundMan = new Mock<ISoundMan>();
        private Mock<IXleGameControl> gameControl = new Mock<IXleGameControl>();
        private Mock<IOutsideExtender> outsideExtender = new Mock<IOutsideExtender>();

        public DisembarkTest()
        {
            InitializeCommand(disembark);

            disembark.Screen = screen.Object;
            disembark.SoundMan = soundMan.Object;
            disembark.GameControl = gameControl.Object;

            GameState.MapExtender = outsideExtender.Object;
        }

        private void SetKeys(params Keys[] keys)
        {
            var sequence = gameControl.SetupSequence(
                    x => x.WaitForKey(It.IsAny<bool>()));

            foreach (var key in keys)
            {
                sequence = sequence.ReturnsAsync(key);
            }
        }

        [Fact]
        public async Task Disembark()
        {
            SetKeys(Keys.Right);

            Player.Rafts.Add(new Xle.RaftData(18, 18, 1));
            Player.BoardedRaft = Player.Rafts.First();

            Player.BoardedRaft.Should().NotBeNull("Player is not on a raft.");

            await disembark.Execute();

            Player.BoardedRaft.Should().BeNull("Player did not disembark.");
        }
    }
}
