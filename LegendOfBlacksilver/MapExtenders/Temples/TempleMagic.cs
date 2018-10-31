using Xle.Data;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Temples
{
    [ServiceName("TempleMagic")]
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
