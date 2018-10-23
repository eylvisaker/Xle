using AgateLib;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.MapLoad;
using Xle.Services.ScreenModel;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class Thornberry : LotaExhibit
    {
        public Thornberry() : base("Thornberry", Coin.Jade) { }

        public IMapChanger MapChanger { get; set; }
        public IStatsDisplay StatsDisplay { get; set; }

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
                StatsDisplay.FlashHPWhileSound(XleColor.Yellow);

                Input.WaitForKey();

                MapChanger.ChangeMap(11, 0);
                Player.SetReturnLocation(1, 18, 56);
            }

            MarkAsVisited();
        }
    }
}
