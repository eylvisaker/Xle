using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.ScreenModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ERY.Xle.Services.XleSystem;
using AgateLib.InputLib;
using ERY.Xle.Maps.Outdoors;

namespace ERY.XleTests.Commands
{
    [TestClass]
    public class DisembarkTest : XleTest
    {
        Disembark disembark = new Disembark();
        Mock<IXleScreen> screen = new Mock<IXleScreen>();
        Mock<ISoundMan> soundMan = new Mock<ISoundMan>();
        Mock<IXleInput> input = new Mock<IXleInput>();

        Mock<IOutsideExtender> outsideExtender = new Mock<IOutsideExtender>();

        [TestInitialize]
        public void Init()
        {
            InitializeCommand(disembark);

            disembark.Screen = screen.Object;
            disembark.SoundMan = soundMan.Object;
            disembark.Input = input.Object;

            GameState.MapExtender = outsideExtender.Object;
        }

        void SetKeys(params KeyCode[] keys)
        {
            var sequence = input.SetupSequence(
                    x => x.WaitForKey(It.IsAny<KeyCode[]>()));

            foreach (var key in keys)
            {
                sequence = sequence.Returns(key);
            }
        }

        [TestMethod]
        public void Disembark()
        {
            SetKeys(KeyCode.Right);

            Player.Rafts.Add(new Xle.RaftData(18, 18, 1));
            Player.BoardedRaft = Player.Rafts.First();

            Assert.IsNotNull(Player.BoardedRaft, "Player is not on a raft.");

            disembark.Execute();

            Assert.IsNull(Player.BoardedRaft, "Player did not disembark.");
        }
    }
}
