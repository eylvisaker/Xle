using ERY.Xle.Data;
using ERY.Xle.Maps.Dungeons.Commands;
using ERY.Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
    [ServiceName("LobDungeonMagic")]
    public class LobDungeonMagic : DungeonMagic
    {
        protected override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                return Data.MagicSpells.Values;
            }
        }
    }
}
