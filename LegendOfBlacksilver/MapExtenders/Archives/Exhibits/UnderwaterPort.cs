using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class UnderwaterPort : LobExhibit
    {
        public UnderwaterPort()
            : base("Underwater Port", Coin.YellowDiamond)
        { }

        public override bool IsClosed
        {
            get { return HasBeenVisited; }
        }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.UnderwaterPort; }
        }
    }
}
