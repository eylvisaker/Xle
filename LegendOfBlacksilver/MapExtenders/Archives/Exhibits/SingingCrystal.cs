using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class SingingCrystal : LobExhibit
    {
        public SingingCrystal()
            : base("Singing Crystal", Coin.BlueGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.SingingCrystal; }
        }

        public override bool IsClosed
        {
            get { return Story.ProcuredSingingCrystal; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.Clear();

            Player.Items[LobItem.SingingCrystal] = 1;
            Story.ProcuredSingingCrystal = true;

            SoundMan.PlaySoundSync(LotaSound.VeryGood);
        }
    }
}
