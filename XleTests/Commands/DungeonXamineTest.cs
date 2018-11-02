using Xle;
using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Maps.Dungeons.Commands;
using Microsoft.Xna.Framework;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace Xle.Commands
{
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

        [Fact]
        public async Task ExamineNothing()
        {
            formatter.Setup(x => x.PrintNothingUnusualInSight()).Verifiable();

            await command.Execute();

            formatter.Verify(x => x.PrintNothingUnusualInSight(), Times.Once);
        }

        [Fact]
        public async Task ExamineDistantTrap()
        {
            Services.DungeonAdapter.Setup(x => x.TileAt(It.IsAny<int>(), It.IsAny<int>(), -1))
                .Returns((int x, int y, int level) =>
                {
                    if (x == 4 && y == 2) return DungeonTile.TripWire;
                    return DungeonTile.Empty;
                });

            formatter.Setup(x => x.DescribeTile(DungeonTile.TripWire, 2))
                .Verifiable();

            await command.Execute();

            formatter.Verify(x => x.DescribeTile(DungeonTile.TripWire, 2), Times.Once);
        }

        [Fact]
        public async Task RevealTrap()
        {
            Services.DungeonAdapter.Setup(x => x.RevealTrapAt(new Point(4, 2)))
                .Returns(true);

            formatter.Setup(x => x.PrintHiddenObjectsDetected()).Verifiable();

            await command.Execute();

            formatter.Verify(x => x.PrintHiddenObjectsDetected(), Times.Once);
        }

        [Fact]
        public async Task DontRevealTrapThruWall()
        {
            Services.DungeonAdapter.Setup(x => x.RevealTrapAt(new Point(4, 2)))
         .Returns(true);
            Services.DungeonAdapter.Setup(x => x.IsWallAt(new Point(3, 2))).Returns(true);

            formatter.Setup(x => x.PrintHiddenObjectsDetected()).Verifiable();

            await command.Execute();

            formatter.Verify(x => x.PrintHiddenObjectsDetected(), Times.Never);
        }

        [Fact]
        public async Task ExamineMonster()
        {
            var monster = new DungeonMonster(new Xle.Data.DungeonMonsterData());
            monster.Data.Name = "Slime Wart";

            Services.DungeonAdapter.Setup(x => x.MonsterAt(new Point(4, 2))).Returns(monster);
            formatter.Setup(x => x.DescribeMonster(monster)).Verifiable();

            await command.Execute();

            formatter.Verify(x => x.DescribeMonster(monster), Times.Once);
        }
    }
}
