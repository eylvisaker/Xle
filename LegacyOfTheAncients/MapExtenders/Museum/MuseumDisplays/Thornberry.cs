﻿using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.MapLoad;
using ERY.Xle.Services.Rendering;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class Thornberry : LotaExhibit
    {
        public Thornberry() : base("Thornberry", Coin.Jade) { }

        public IMapChanger MapChanger { get; set; }
        public IXleRenderer Renderer { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Thornberry; } }
        public override string LongName
        {
            get { return "A typical town of Tarmalon"; }
        }

        public override void RunExhibit()
        {
            if (CheckOfferReread())
            {
                ReadRawText(ExhibitInfo.Text[1]);
            }

            TextArea.PrintLine("Would you like to go");
            TextArea.PrintLine("to thornberry?");
            TextArea.PrintLine();

            if (QuickMenu.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
            {
                ReadRawText(ExhibitInfo.Text[2]);

                int amount = 100;

                if (HasBeenVisited || ExhibitHasBeenVisited(ExhibitIdentifier.Fountain))
                {
                    amount += 200;
                }

                Player.Gold += amount;

                TextArea.PrintLine();
                TextArea.PrintLine("             GOLD:  + " + amount.ToString(), XleColor.Yellow);

                SoundMan.PlaySound(LotaSound.VeryGood);
                Renderer.FlashHPWhileSound(XleColor.Yellow);

                Input.WaitForKey();

                MapChanger.ChangeMap(Player, 11, 0);
                Player.SetReturnLocation(1, 18, 56);
            }

            MarkAsVisited();
        }
    }
}
