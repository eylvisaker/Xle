using Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Services;
using Xle.Services.MapLoad;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
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
        public override void RunExhibit()
        {
            base.RunExhibit();

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

                MapChanger.ChangeMap(2, 0);
            }
        }
    }
}
