using AgateLib;
using System.Collections.Generic;
using Xle.Data;
using Xle.Maps.Dungeons.Commands;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
{
    [Transient("LobDungeonMagic")]
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
