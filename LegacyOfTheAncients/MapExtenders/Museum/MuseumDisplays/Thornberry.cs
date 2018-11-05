using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.MapLoad;
using Xle.ScreenModel;

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

        public override async Task RunExhibit()
        {
            if (await CheckOfferReread())
            {
                await ReadRawText(ExhibitInfo.Text[1]);
            }

            await TextArea.PrintLine("Would you like to go");
            await TextArea.PrintLine("to thornberry?");
            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenu(new MenuItemList("Yes", "no"), 3) == 0)
            {
                await ReadRawText(ExhibitInfo.Text[2]);

                int amount = 100;

                if (HasBeenVisited || ExhibitHasBeenVisited(ExhibitIdentifier.Fountain))
                {
                    amount += 200;
                }

                Player.Gold += amount;

                await TextArea.PrintLine();
                await TextArea.PrintLine("             GOLD:  + " + amount.ToString(), XleColor.Yellow);

                SoundMan.PlaySound(LotaSound.VeryGood);
                await GameControl.FlashHPWhileSound(XleColor.Yellow);

                await GameControl.WaitForKey();

                await MapChanger.ChangeMap(11, 0);
                Player.SetReturnLocation(1, 18, 56);
            }

            MarkAsVisited();
        }
    }
}
