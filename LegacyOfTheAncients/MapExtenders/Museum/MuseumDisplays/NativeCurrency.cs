using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services.Rendering;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class NativeCurrency : LotaExhibit
    {
        public NativeCurrency() : base("Native Currency", Coin.Topaz) { }

        public Random Random { get; set; }
        public IXleRenderer Renderer { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.NativeCurrency; } }

        public override void RunExhibit(Player unused)
        {
            base.RunExhibit(Player);

            int gold = (int)(350 * (1 + Player.Level)
                * (1 + Random.NextDouble()));

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("             Gold:  + " + gold.ToString(), XleColor.Yellow);
            TextArea.PrintLine();
            TextArea.PrintLine();

            Player.Gold += gold;

            SoundMan.PlaySound(LotaSound.VeryGood);
            Renderer.FlashHPWhileSound(XleColor.Yellow);
        }
    }
}
