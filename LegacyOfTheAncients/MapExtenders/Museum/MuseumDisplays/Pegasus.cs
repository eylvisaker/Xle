﻿using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.MapLoad;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class Pegasus : LotaExhibit
    {
        public Pegasus() : base("Pegasus", Coin.Diamond) { }

        public IMapChanger MapChanger { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Pegasus; } }
        public override string LongName
        {
            get { return "A flight of fancy"; }
        }

        public override void RunExhibit()
        {
            base.RunExhibit();

            TextArea.PrintLine();
            TextArea.PrintLine("Do you want to climb on?");
            TextArea.PrintLine();

            if (0 == QuickMenu.QuickMenuYesNo())
            {
                if (Player.Food < 150)
                    Player.Food = 150;

                MapChanger.ChangeMap(Player, 3, 0);
            }
        }

        public override bool StaticBeforeCoin
        {
            get { return false; }
        }
    }
}
