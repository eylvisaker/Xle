using Xle.Data;
using Xle.Maps.Dungeons.Commands;
using Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Dungeon.Commands
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
