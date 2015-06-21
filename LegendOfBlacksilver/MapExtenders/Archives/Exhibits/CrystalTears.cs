using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class CrystalTears : LobExhibit
    {
        public CrystalTears()
            : base("Crystal Tears", Coin.BlackOpal)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.CrystalTears; }
        }

        public override void RunExhibit(Player unused)
        {
            base.RunExhibit(Player);

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to borrow them?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                Player.Items[LobItem.DragonTear] += 2;

                TextArea.PrintLine();
                TextArea.PrintLine("You receive two dragon's tears.");

                SoundMan.PlaySoundSync(LotaSound.VeryGood);
            }
            else
            {
                ReturnGem(Player);
            }
        }
    }
}
