using Xle.Services;
using Xle.Services.Commands.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xle.Data;

namespace Xle.Ancients.MapExtenders.Dungeons.Commands
{
    [ServiceName("LotaDungeonMagic")]
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
