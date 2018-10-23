﻿using AgateLib;
using System;
using Xle.Services.ScreenModel;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class NativeCurrency : LotaExhibit
    {
        public NativeCurrency() : base("Native Currency", Coin.Topaz) { }

        public Random Random { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.NativeCurrency; } }

        public override void RunExhibit()
        {
            base.RunExhibit();

            int gold = (int)(350 * (1 + Player.Level)
                * (1 + Random.NextDouble()));

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("             Gold:  + " + gold.ToString(), XleColor.Yellow);
            TextArea.PrintLine();
            TextArea.PrintLine();

            Player.Gold += gold;

            SoundMan.PlaySound(LotaSound.VeryGood);
            StatsDisplay.FlashHPWhileSound(XleColor.Yellow);
        }
    }
}
