using Xle.Maps;
using Xle.Maps.Castles;
using Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LegacyOfTheAncients
{
    public class MagicIceTest : LotaTest
    {
        CastleMap map;
        CastleExtender mapExtender;

        public MagicIceTest()
        {
            map = new CastleMap();
            map.TileSet = new TileSet();
            map.TileSet.TileGroups = new List<TileGroup>();

            mapExtender = new CastleExtender();
            mapExtender.TheMap = map;

            GameState.MapExtender = mapExtender;

            GameState.Map.TileSet.TileGroups.Add(
                new TileGroup
                {
                    Tiles = new List<int> { 1, 2, 3 },
                    GroupType = GroupType.Water
                });
            GameState.Map.TileSet.TileGroups.Add(
                new TileGroup()
                {
                    Tiles = new List<int> { 4, 5, 6 },
                    GroupType = GroupType.Special1
                });
        }
    }
}
