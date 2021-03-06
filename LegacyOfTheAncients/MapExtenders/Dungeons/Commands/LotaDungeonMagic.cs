﻿using AgateLib;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Commands.Implementation;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
{
    [Transient("LotaDungeonMagic")]
    public class LotaDungeonMagic : MagicWithFancyPrompt
    {
        protected override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                // everything but seek spell
                return from m in Data.MagicSpells
                       where m.Key != 6
                       select m.Value;
            }
        }
    }
}
