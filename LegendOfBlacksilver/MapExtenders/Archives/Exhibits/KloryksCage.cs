using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class KloryksCage : LobExhibit
    {
        public KloryksCage()
            : base("Kloryk's Cage", Coin.WhiteDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.KloryksCage; }
        }

        public override bool IsClosed
        {
            get { return HasBeenVisited; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            ReturnGem();
        }
    }
}
