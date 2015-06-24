using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public override bool IsClosed(Player unused)
        {
            if (Story.ProcuredSingingCrystal)
                return true;

            return base.IsClosed(Player);
        }
        public override void RunExhibit(Player unused)
        {
            base.RunExhibit(Player);

            TextArea.Clear();

            Player.Items[LobItem.SingingCrystal] = 1;
            Story.ProcuredSingingCrystal = true;

            SoundMan.PlaySoundSync(LotaSound.VeryGood);
        }
    }
}
