using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
    [ServiceName("LotaOutsideMagic")]
    public class LotaOutsideMagic : MagicCommand
    {
        protected override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                yield return Data.MagicSpells[1];
                yield return Data.MagicSpells[2];
                yield return Data.MagicSpells[6];
            }
        }

    }
}
