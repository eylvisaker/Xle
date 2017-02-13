using AgateLib.Mathematics.Geometry;
using ERY.Xle;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Dungeons;
using ERY.Xle.Maps.Dungeons.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.Commands
{
    [TestClass]
    public class DungeonXamineTest : XleTest
    {
        private DungeonXamine command;
        private Mock<IXamineFormatter> formatter;

        public DungeonXamineTest()
        {
            formatter = new Mock<IXamineFormatter>();

            command = new DungeonXamine();

            InitializeCommand(command);

            command.SoundMan = Services.SoundMan.Object;
            command.GameControl = Services.GameControl.Object;
            command.DungeonAdapter = Services.DungeonAdapter.Object;
            command.XamineFormatter = formatter.Object;

            GameState.Player.Location = new Point(2, 2);
            GameState.Player.FaceDirection = Direction.East;
        }

        [TestMethod]
        public void ExamineNothing()
        {
            formatter.Setup(x => x.PrintNothingUnusualInSight()).Verifiable();

            command.Execute();

            formatter.Verify(x => x.PrintNothingUnusualInSight(), Times.Once);
        }

        [TestMethod]
        public void ExamineDistantTrap()
        {
            Services.DungeonAdapter.Setup(x => x.TileAt(It.IsAny<int>(), It.IsAny<int>(), -1))
                .Returns((int x, int y, int level) =>
                {
                    if (x == 4 && y == 2) return DungeonTile.TripWire;
                    return DungeonTile.Empty;
                });

            formatter.Setup(x => x.DescribeTile(DungeonTile.TripWire, 2))
                .Verifiable();

            command.Execute();

            formatter.Verify(x => x.DescribeTile(DungeonTile.TripWire, 2), Times.Once);
        }

        [TestMethod]
        public void RevealTrap()
        {
            Services.DungeonAdapter.Setup(x => x.RevealTrapAt(new Point(4, 2)))
                .Returns(true);

            formatter.Setup(x => x.PrintHiddenObjectsDetected()).Verifiable();

            command.Execute();

            formatter.Verify(x => x.PrintHiddenObjectsDetected(), Times.Once);
        }

        [TestMethod]
        public void DontRevealTrapThruWall()
        {
            Services.DungeonAdapter.Setup(x => x.RevealTrapAt(new Point(4, 2)))
         .Returns(true);
            Services.DungeonAdapter.Setup(x => x.IsWallAt(new Point(3, 2))).Returns(true);

            formatter.Setup(x => x.PrintHiddenObjectsDetected()).Verifiable();

            command.Execute();

            formatter.Verify(x => x.PrintHiddenObjectsDetected(), Times.Never);
        }

        [TestMethod]
        public void ExamineMonster()
        {
            var monster = new DungeonMonster(new Xle.Data.DungeonMonsterData());
            monster.Data.Name = "Slime Wart";

            Services.DungeonAdapter.Setup(x => x.MonsterAt(new Point(4, 2))).Returns(monster);
            formatter.Setup(x => x.DescribeMonster(monster)).Verifiable();

            command.Execute();

            formatter.Verify(x => x.DescribeMonster(monster), Times.Once);
        }
    }
}
