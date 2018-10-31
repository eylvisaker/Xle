using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Maps.Dungeons.Commands;
using Xle.Services;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
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
    }
}
