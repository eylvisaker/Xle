using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class MetalWork : LobExhibit
    {
        public MetalWork()
            : base("Metalwork", Coin.BlueGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.MetalWork; }
        }
    }
}
