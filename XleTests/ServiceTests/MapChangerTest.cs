﻿using System;
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
        Mock<MapExtender> startMap;

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

        private void SetupMapLoader<TMapData>(Action<Mock<MapExtender>> mapGenerator = null) where TMapData : XleMap, new()
        {
            Services.MapLoader.Setup(x => x.LoadMap(It.IsAny<int>()))
                .Returns((int mapId) =>
                {
                    returnedMap = InitializeMap<TMapData>(mapId);

                    returnedMap.Setup(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>())).Verifiable();
                    returnedMap.Setup(x => x.OnLoad()).Verifiable();
                    returnedMap.Setup(x => x.SetCommands(Services.CommandList.Object)).Verifiable();
                    returnedMap.Setup(x => x.OnAfterEntry()).Verifiable();

                    if (mapGenerator != null)
                        mapGenerator(returnedMap);
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

        private void SetStartMap(int mapId = 1)
        {
            startMap = InitializeMap<Outside>(mapId);
            changer.SetMap(startMap.Object);
            Player.MapID = startMap.Object.MapID;
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

            SetStartMap();

            Player.Location = new Point(22, 44);
            Player.FaceDirection = Direction.West;

            changer.ChangeMap(2, new Point(4, 4));

            Assert.AreEqual(1, Player.returnMap);
            Assert.AreEqual(22, Player.returnX);
            Assert.AreEqual(44, Player.returnY);
            Assert.AreEqual(2, GameState.Map.MapID);
            Assert.AreEqual(2, Player.MapID);
            Assert.AreEqual(new Point(4, 4), Player.Location);
            Assert.AreEqual(Direction.South, Player.returnFacing);
        }

        [TestMethod]
        public void MapEntryPointEvents()
        {
            SetupMapLoader<Town>(m =>
            {
                m.Object.TheMap.EntryPoints.Add(new EntryPoint { Location = new Point(4, 4) });
            });

            SetStartMap();
            changer.ChangeMap(2, 0);

            returnedMap.Verify(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>()));
            returnedMap.Verify(x => x.OnLoad());
            returnedMap.Verify(x => x.SetCommands(Services.CommandList.Object));
            returnedMap.Verify(x => x.OnAfterEntry());
        }

        [TestMethod]
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

        [TestMethod]
        public void MoveOnSameMap()
        {
            SetupMapLoader<Town>();

            SetStartMap(1);
            startMap.ResetCalls();

            changer.ChangeMap(1, new Point(24, 33));

            Assert.AreEqual(new Point(24,33), Player.Location);
            startMap.Verify(x => x.ModifyEntryPoint(It.IsAny<MapEntryParams>()), Times.Never());
            startMap.Verify(x => x.OnLoad(), Times.Never());
            startMap.Verify(x => x.OnAfterEntry(), Times.Never);
        }
    }
}