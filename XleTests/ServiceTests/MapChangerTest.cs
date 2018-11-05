using Xle;
using Xle.Maps;
using Xle.Maps.XleMapTypes;
using Xle.Commands;
using Xle.Menus;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System;
using Xunit;
using System.Threading.Tasks;

namespace Xle.MapLoad
{
    public class MapChangerTest : XleTest
    {
        private MapChanger changer;
        private Mock<IMapExtender> returnedMap;
        private Mock<IMapExtender> startMap;
        private Mock<IMuseumCoinSale> museumCoinSale;

        public MapChangerTest()
        {
            museumCoinSale = new Mock<IMuseumCoinSale>();

            changer = new MapChanger(GameState,
                Services.Screen.Object,
                Services.Images.Object,
                Services.TextArea.Object,
                Services.CommandList.Object,
                Services.MapLoader.Object,
                museumCoinSale.Object);
        }

        private void SetupMapLoader<TMapData>(Action<Mock<IMapExtender>> mapGenerator = null) where TMapData : XleMap, new()
        {
            Services.MapLoader.Setup(x => x.LoadMap(It.IsAny<int>()))
                .Returns((int mapId) =>
                {
                    returnedMap = InitializeMap<TMapData>(mapId);

                    returnedMap.Setup(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>())).Verifiable();
                    returnedMap.Setup(x => x.OnLoad()).Verifiable();
                    returnedMap.Setup(x => x.SetCommands(Services.CommandList.Object)).Verifiable();
                    returnedMap.Setup(x => x.OnAfterEntry()).Returns(Task.CompletedTask).Verifiable();

                    if (mapGenerator != null)
                        mapGenerator(returnedMap);
                    return returnedMap.Object;
                });
        }

        [Fact]
        public void SetMap()
        {
            var newMap = InitializeMap<CastleMap>(1);

            newMap.Setup(x => x.OnLoad()).Verifiable();
            newMap.Setup(x => x.SetCommands(It.IsAny<ICommandList>())).Verifiable();

            Services.CommandList.Setup(x => x.ResetCurrentCommand()).Verifiable();
            Services.Images.Setup(x => x.LoadTiles("MyTiles")).Verifiable();

            changer.SetMap(newMap.Object);

            newMap.Verify();
        }

        private void SetStartMap(int mapId = 1)
        {
            startMap = InitializeMap<Outside>(mapId);
            changer.SetMap(startMap.Object);
            Player.MapID = startMap.Object.MapID;
        }

        [Fact]
        public void ChangeMapToPoint()
        {
            SetupMapLoader<CastleMap>();

            var startMap = InitializeMap<CastleMap>(1);
            GameState.MapExtender = startMap.Object;

            changer.ChangeMap(2, new Point(4, 4));

            GameState.Map.MapID.Should().Be(2);
            Player.Location.Should().Be(new Point(4, 4));
        }

        [Fact]
        public void PreserveReturnLocation()
        {
            SetupMapLoader<Town>();

            SetStartMap();

            Player.Location = new Point(22, 44);
            Player.FaceDirection = Direction.West;

            changer.ChangeMap(2, new Point(4, 4));

            Player.returnMap.Should().Be(1, "Player's return map ID not set properly.");
            Player.returnX.Should().Be(22);
            Player.returnY.Should().Be(44);
            GameState.Map.MapID.Should().Be(2);
            Player.MapID.Should().Be(2);
            Player.Location.Should().Be(new Point(4, 4));
            Player.returnFacing.Should().Be(Direction.South);
        }

        [Fact]
        public async Task MapEntryPointEvents()
        {
            SetupMapLoader<Town>(m =>
            {
                m.Object.TheMap.EntryPoints.Add(new EntryPoint { Location = new Point(4, 4) });
            });

            SetStartMap();
            await changer.ChangeMap(2, 0);

            returnedMap.Verify(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>()));
            returnedMap.Verify(x => x.OnLoad());
            returnedMap.Verify(x => x.SetCommands(Services.CommandList.Object));
            returnedMap.Verify(x => x.OnAfterEntry());
        }

        [Fact]
        public void MapDirectEntryEvents()
        {
            SetupMapLoader<Town>();

            SetStartMap();
            changer.ChangeMap(2, new Point(5, 5));

            returnedMap.Verify(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>()), Times.Never);
            returnedMap.Verify(x => x.OnLoad());
            returnedMap.Verify(x => x.SetCommands(Services.CommandList.Object));
            returnedMap.Verify(x => x.OnAfterEntry());
        }

        [Fact]
        public void MoveOnSameMap()
        {
            SetupMapLoader<Town>();

            SetStartMap(1);
            startMap.Invocations.Clear();

            changer.ChangeMap(1, new Point(24, 33));

            Player.Location.Should().Be(new Point(24, 33));
            startMap.Verify(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>()), Times.Never());
            startMap.Verify(x => x.OnLoad(), Times.Never());
            startMap.Verify(x => x.OnAfterEntry(), Times.Never);
        }
    }
}
