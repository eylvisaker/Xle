using Xle.Data;
using Xle;
using Xle.Commands.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Temples
{
    [Transient("TempleMagic")]
    public class TempleMagic : MagicCommand
    {
        protected override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                yield return Data.MagicSpells[3];
                yield return Data.MagicSpells[4];
            }
        }
    }
}
