using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class Blacksmith : LobExhibit
    {
        public Blacksmith()
            : base("Blacksmith", Coin.WhiteDiamond)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.Blacksmith; }
        }

        public override bool IsClosed
        {
            get { return Story.ProcuredSteelHammer; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to have it?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("It is yours.");

                SoundMan.PlaySoundSync(LotaSound.Good);

                Player.Items[LobItem.SteelHammer] = 1;

                Story.ProcuredSteelHammer = true;
            }
            else
                ReturnGem();
        }
    }
}
