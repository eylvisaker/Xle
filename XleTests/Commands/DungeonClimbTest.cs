using AgateLib.Mathematics.Geometry;
using ERY.Xle;
using ERY.Xle.Maps.Dungeons;
using ERY.Xle.Maps.XleMapTypes;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ERY.XleTests.Commands
{
	public class DungeonClimbTest : XleTest
	{
		DungeonClimb climb;
		Player player;
		GameState gameState;
		Mock<DungeonExtender> map;
		Mock<IDungeonAdapter> adapter;
		Dungeon mapData;

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
		public void ClimbDown()
		{
			player.Location = new Point(4, 4);
			adapter.Setup(x => x.TileAt(4, 4, -1)).Returns(DungeonTile.FloorHole);

			player.DungeonLevel = 0;

			climb.Execute();

			player.DungeonLevel.Should().Be(1);
		}

		[Fact]
		public void ClimbUp()
		{
			player.Location = new Point(4, 4);
			adapter.Setup(x => x.TileAt(4, 4, -1)).Returns(DungeonTile.CeilingHole);

			player.DungeonLevel = 2;

			climb.Execute();

			player.DungeonLevel.Should().Be(1);
		}

		[Fact]
		public void ClimbOut()
		{
			player.Location = new Point(4, 4);
			adapter.Setup(x => x.TileAt(4, 4, -1)).Returns(DungeonTile.CeilingHole);

			player.DungeonLevel = 0;

			Services.MapChanger.Setup(x => x.ReturnToPreviousMap()).Verifiable();
			adapter.Setup(x => x.OnPlayerExitDungeon()).Verifiable();

			climb.Execute();

			adapter.Verify();
			Services.MapChanger.Verify();
		}
	}
}
