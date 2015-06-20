using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class HerbOfLife : LotaExhibit
    {
        public HerbOfLife() : base("Herb of life", Coin.Topaz) { }
        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.HerbOfLife; } }
        public override string LongName
        {
            get
            {
                return "The herb of life";
            }
        }

        public override void RunExhibit(Player player)
        {
            ReadRawText(RawText);

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to eat the fruit?");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenu(new MenuItemList("Yes", "No"), 3) == 0)
            {
                TextArea.PrintLine("You feel a tingling sensation.", XleColor.Green);
                SoundMan.PlaySoundSync(LotaSound.Good);

                Story.EatenJutonFruit = true;
            }
        }
    }
}
