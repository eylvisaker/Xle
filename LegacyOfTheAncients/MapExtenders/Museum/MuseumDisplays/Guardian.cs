using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class Guardian : LotaExhibit
    {
        public Guardian() : base("The guardian", Coin.Turquoise) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Guardian; } }

        public override bool IsClosed
        {
            get { return Story.HasGuardianPassword; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            Story.HasGuardianPassword = true;
        }
    }
}
