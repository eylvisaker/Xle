using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using ERY.Xle.Services.Commands;

namespace ERY.XleTests
{
    [TestClass]
    public class XleTest
    {
        protected Player Player { get; private set; }
        protected GameState GameState { get; private set; }
        protected XleServices Services { get; private set; }

        public XleTest()
        {
            Player = new Player();
            GameState = new GameState { Player = Player };

            Services = new XleServices();

        }

        protected virtual void InitializeEvent(EventExtender evt)
        {
            evt.TheEvent = new Script();

            evt.TextArea = Services.TextArea.Object;
            evt.GameState = GameState;
            evt.GameControl = Services.GameControl.Object;
            evt.SoundMan = Services.SoundMan.Object;

        }

        protected virtual Mock<MapExtender> InitializeMap<TMapData>(int mapId)
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

        protected virtual void InitializeCommand(Command command)
        {
            command.GameState = GameState;
            command.TextArea = Services.TextArea.Object;
        }
    }
}
