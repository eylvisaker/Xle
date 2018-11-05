using AgateLib;
using System;
using System.Threading.Tasks;
using Xle.ScreenModel;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class NativeCurrency : LotaExhibit
    {
        public NativeCurrency() : base("Native Currency", Coin.Topaz) { }

        public Random Random { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.NativeCurrency; } }

        public override async Task RunExhibit()
        {
            await base.RunExhibit();

            int gold = (int)(350 * (1 + Player.Level)
                * (1 + Random.NextDouble()));

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("             Gold:  + " + gold.ToString(), XleColor.Yellow);
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            Player.Gold += gold;

            SoundMan.PlaySound(LotaSound.VeryGood);
            await GameControl.FlashHPWhileSound(XleColor.Yellow);
        }
    }
}
