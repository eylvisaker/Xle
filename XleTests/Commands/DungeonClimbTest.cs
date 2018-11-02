using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System.Threading.Tasks;
using Xle.Maps.Dungeons;
using Xle.Maps.XleMapTypes;
using Xunit;

namespace Xle.Commands
{
    public class DungeonClimbTest : XleTest
    {
        private DungeonClimb climb;
        private Player player;
        private GameState gameState;
        private Mock<DungeonExtender> map;
        private Mock<IDungeonAdapter> adapter;
        private Dungeon mapData;

        public DungeonClimbTest()
        {
            adapter = new Mock<IDungeonAdapter>();

            mapData = new Dungeon();
            mapData.SetLevels(8);
            mapData.InitializeMap(16, 16);

            map = new Mock<DungeonExtender>();
            map.SetupAllProperties();
            map.Object.TheMap = mapData;

            player = new Player();
            gameState = new GameState { Player = player, MapExtender = map.Object };

            climb = new DungeonClimb();
            climb.DungeonAdapter = adapter.Object;
            climb.GameControl = Services.GameControl.Object;
            climb.MapChanger = Services.MapChanger.Object;
            climb.TextArea = Services.TextArea.Object;
            climb.GameState = gameState;
        }

        [Fact]
        public async Task ClimbDown()
        {
            player.Location = new Point(4, 4);
            adapter.Setup(x => x.TileAt(4, 4, -1)).Returns(DungeonTile.FloorHole);

            player.DungeonLevel = 0;

            await climb.Execute();

            player.DungeonLevel.Should().Be(1);
        }

        [Fact]
        public async Task ClimbUp()
        {
            player.Location = new Point(4, 4);
            adapter.Setup(x => x.TileAt(4, 4, -1)).Returns(DungeonTile.CeilingHole);

            player.DungeonLevel = 2;

            await climb.Execute();

            player.DungeonLevel.Should().Be(1);
        }

        [Fact]
        public async Task ClimbOut()
        {
            player.Location = new Point(4, 4);
            adapter.Setup(x => x.TileAt(4, 4, -1)).Returns(DungeonTile.CeilingHole);

            player.DungeonLevel = 0;

            Services.MapChanger.Setup(x => x.ReturnToPreviousMap()).Returns(Task.CompletedTask).Verifiable();
            adapter.Setup(x => x.OnPlayerExitDungeon()).Returns(Task.CompletedTask).Verifiable();

            await climb.Execute();

            adapter.Verify();
            Services.MapChanger.Verify();
        }
    }
}
