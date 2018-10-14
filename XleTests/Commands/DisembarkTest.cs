using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.ScreenModel;
using Xunit;
using Moq;
using ERY.Xle.Services.XleSystem;
using AgateLib.InputLib;
using ERY.Xle.Maps.Outdoors;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;

namespace ERY.XleTests.Commands
{
    public class DisembarkTest : XleTest
    {
        Disembark disembark = new Disembark();
        Mock<IXleScreen> screen = new Mock<IXleScreen>();
        Mock<ISoundMan> soundMan = new Mock<ISoundMan>();
        Mock<IXleInput> input = new Mock<IXleInput>();

        Mock<IOutsideExtender> outsideExtender = new Mock<IOutsideExtender>();

        public DisembarkTest()
        {
            InitializeCommand(disembark);

            disembark.Screen = screen.Object;
            disembark.SoundMan = soundMan.Object;
            disembark.Input = input.Object;

            GameState.MapExtender = outsideExtender.Object;
        }

        void SetKeys(params Keys[] keys)
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
