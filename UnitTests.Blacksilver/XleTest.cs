using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle;
using Xle.Maps;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;

using Xunit;

using Moq;
using Xle.Services.Commands;

namespace Xle
{
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

        protected virtual Mock<IMapExtender> InitializeMap<TMapData>(int mapId)
            where TMapData : XleMap, new()
        {
            XleMap map = new TMapData();
            map.MapID = mapId;
            map.TileImage = "MyTiles";

            Mock<IMapExtender> newMap = new Mock<IMapExtender>();
            newMap.SetupAllProperties();

            newMap.Setup(x => x.TheMap).Returns(map);
            newMap.Setup(x => x.MapID).Returns(map.MapID);

            return newMap;
        }

        protected virtual void InitializeCommand(Command command)
        {
            command.GameState = GameState;
            command.TextArea = Services.TextArea.Object;
        }
    }
}
