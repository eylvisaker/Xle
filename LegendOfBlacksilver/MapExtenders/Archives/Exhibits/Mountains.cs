using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;

namespace ERY.Xle.LoB.MapExtenders.Archives.Exhibits
{
    public class Mountains : LobExhibit
    {
        public Mountains()
            : base("Mountains", Coin.AmethystGem)
        { }

        public override ExhibitIdentifier ExhibitIdentifier
        {
            get { return ExhibitIdentifier.Mountains; }
        }

        public override bool IsClosed
        {
            get { return Player.Items[LobItem.ClimbingGear] > 0; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want a set?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("Use it carefully.");

                SoundMan.PlaySoundSync(LotaSound.Good);

                Player.Items[LobItem.ClimbingGear] += 1;
            }
            else
                ReturnGem();
        }
    }
}
