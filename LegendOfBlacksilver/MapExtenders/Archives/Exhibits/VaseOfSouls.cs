using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class VaseOfSouls : LobExhibit
    {
        public VaseOfSouls()
            : base("Vase of Souls", Coin.AmethystGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.VaseOfSouls; }
        }

        public override bool IsClosed
        {
            get { return Story.ClosedVaseOfSouls; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            ReturnGem();

            Story.ClosedVaseOfSouls = true;
        }
    }
}
