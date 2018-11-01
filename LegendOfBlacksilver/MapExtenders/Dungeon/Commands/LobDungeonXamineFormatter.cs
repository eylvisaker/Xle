using Xle.Maps.Dungeons;
using Xle.Maps.Dungeons.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
{
    [Singleton]
    public class LobDungeonXamineFormatter : XamineFormatter
    {
        protected override string TileName(DungeonTile val)
        {
            if (val == DungeonTile.Box)
                return "case";

            return base.TileName(val);
        }
    }
}

