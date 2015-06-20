using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Implementation;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class PirateTreasure : LotaExhibit
    {
        public PirateTreasure() : base("Pirate Treasure", Coin.Topaz) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.PirateTreasure; } }

        public override bool StaticBeforeCoin
        {
            get
            {
                return false;
            }
        }
        public override void RunExhibit(Player unused)
        {
            if (CheckOfferReread(Player))
            {
                ReadRawText(ExhibitInfo.Text[1]);
            }

            TextArea.PrintLine("Would you like to go");
            TextArea.PrintLine("to the pirate's lair?");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
            {
                ReadRawText(ExhibitInfo.Text[2]);

                for (int i = 0; i < 8; i++)
                {
                    GameControl.Wait(50);
                    TextArea.SetCharacterColor(2, 12 + i, XleColor.Cyan);
                }

                Input.WaitForKey();

                MapChanger.ChangeMap(Player, 2, 0);
            }
        }
    }
}
