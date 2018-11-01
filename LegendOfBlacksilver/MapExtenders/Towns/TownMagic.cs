using AgateLib;
using System.Collections.Generic;
using Xle.Data;
using Xle.Services.Commands.Implementation;

namespace Xle.Blacksilver.MapExtenders.Towns
{
    [Transient("TownMagic")]
    public class TownMagic : MagicCommand
    {
        protected override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                yield return Data.MagicSpells[1];
                yield return Data.MagicSpells[2];
                yield return Data.MagicSpells[3];
                yield return Data.MagicSpells[4];
            }
        }
    }
}
