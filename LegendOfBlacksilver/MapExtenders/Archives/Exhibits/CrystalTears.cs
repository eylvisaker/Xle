using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.Game;

namespace Xle.LoB.MapExtenders.Archives.Exhibits
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

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            await TextArea.PrintLine();
            await TextArea.PrintLine("Do you want to borrow them?");
            await TextArea.PrintLine();

            if (0 == await QuickMenu.QuickMenuYesNo())
            {
                Player.Items[LobItem.DragonTear] += 2;

                await TextArea.PrintLine();
                await TextArea.PrintLine("You receive two dragon's tears.");

                await GameControl.PlaySoundSync(LotaSound.VeryGood);
            }
            else
            {
                await ReturnGem();
            }
        }
    }
}
