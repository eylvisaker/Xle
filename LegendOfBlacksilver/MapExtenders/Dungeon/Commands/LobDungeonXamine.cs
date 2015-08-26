using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Maps.Dungeons;
using ERY.Xle.Maps.Dungeons.Commands;
using ERY.Xle.Services;

namespace ERY.Xle.LoB.MapExtenders.Dungeon.Commands
{
    [ServiceName("LobDungeonXamine")]
    public class LobDungeonXamine : DungeonXamine
    {
        protected override void PrintExamineMonsterMessage(DungeonMonster foundMonster)
        {
            if (foundMonster.Data.Name == "king")
            {
                TextArea.PrintLine("You see a king!", XleColor.White);
                return;
            }

            base.PrintExamineMonsterMessage(foundMonster);
        }

        protected override string TileName(DungeonTile val)
        {
            if (val == DungeonTile.Box)
                return "case";

            return base.TileName(val);
        }
    }
}
