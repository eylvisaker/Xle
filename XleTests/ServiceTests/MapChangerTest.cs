using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Geometry;

using ERY.Xle;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Towns;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.MapLoad.Implementation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using ERY.Xle.Services.Commands;

namespace ERY.XleTests.ServiceTests
{
    [TestClass]
    public class MapChangerTest : XleTest
    {
        MapChanger changer;
        Mock<MapExtender> returnedMap;

        [TestInitialize]
        public void Initialize()
        {
            changer = new MapChanger(GameState,
                Services.Screen.Object,
                Services.Images.Object,
                Services.TextArea.Object,
                Services.CommandList.Object,
                Services.MapLoader.Object);

        }

        private void SetupMapLoader<TMapData>() where TMapData : XleMap, new()
        {
            Services.MapLoader.Setup(x => x.LoadMap(It.IsAny<int>()))
                .Returns((int mapId) =>
                {
                    returnedMap = InitializeMap<TMapData>(mapId);
                    return returnedMap.Object;
                });
        }

        [TestMethod]
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

        private Mock<MapExtender> InitializeMap<TMapData>(int mapId)
            where TMapData : XleMap, new()
        {
            XleMap map = new TMapData();
            map.MapID = mapId;
            map.TileImage = "MyTiles";

            Mock<MapExtender> newMap = new Mock<MapExtender>();
            newMap.SetupAllProperties();

            newMap.Object.TheMap = map;

            return newMap;
        }

        [TestMethod]
        public void ChangeMapToPoint()
        {
            SetupMapLoader<CastleMap>();

            var startMap = InitializeMap<CastleMap>(1);
            GameState.MapExtender = startMap.Object;

            changer.ChangeMap(2, new Point(4, 4));

            Assert.AreEqual(2, GameState.Map.MapID);
            Assert.AreEqual(new Point(4, 4), Player.Location);
        }

        [TestMethod]
        public void PreserveReturnLocation()
        {
            SetupMapLoader<Town>();

            var startMap = InitializeMap<Outside>(1);
            changer.SetMap(startMap.Object);
            Player.MapID = startMap.Object.MapID;

            Player.Location = new Point(22, 44);
            Player.FaceDirection = Direction.West;

            changer.ChangeMap(2, new Point(4, 4));

            Assert.AreEqual(1, Player.returnMap);
            Assert.AreEqual(22, Player.returnX);
            Assert.AreEqual(44, Player.returnY);
            Assert.AreEqual(Direction.South, Player.returnFacing);
        }
    }
}
