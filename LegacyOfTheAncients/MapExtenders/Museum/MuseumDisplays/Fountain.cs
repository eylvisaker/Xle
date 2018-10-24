using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.ScreenModel;

namespace Xle.Ancients.MapExtenders.Museum.MuseumDisplays
{
    [Transient, InjectProperties]
    public class Fountain : LotaExhibit
    {
        public Fountain() : base("A Fountain", Coin.Jade) { }

        public IStatsDisplay StatsDisplay { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Fountain; } }
        public override string LongName
        {
            get
            {
                return "Enchanted flower fountain";
            }
        }

        public override bool IsClosed
        {
            get { return Story.ReturnedTulip; }
        }

        public override async Task RunExhibit()
        {
            if (Player.Items[LotaItem.Tulip] == 0)
            {
                await OfferTulipQuest();
            }
            else
            {
                await RewardForTulip();
            }
        }

        private async Task RewardForTulip()
        {
            // remove the tulip from the player, give the reward and shut down the exhibit.
            Player.Items[LotaItem.Tulip] = 0;
            Player.Attribute[Attributes.charm] += 10;
            Story.ReturnedTulip = true;

            await ReadRawText(ExhibitInfo.Text[3]);

            TextArea.Clear();
        }

        private async Task OfferTulipQuest()
        {
            bool firstVisit = HasBeenVisited;

            await base.RunExhibit();
            await TextArea.PrintLine();

            if (Story.SearchingForTulip == false)
                await TextArea.PrintLine("Do you want to help search?");
            else
                await TextArea.PrintLine("Do you want to continue searching?");

            await TextArea.PrintLine();

            if (await QuickMenu.QuickMenuYesNo() == 0)
            {
                await ReadRawText(ExhibitInfo.Text[2]);
                int amount = 100;

                if (firstVisit || ExhibitHasBeenVisited(ExhibitIdentifier.Thornberry))
                {
                    amount += 200;
                }

                Player.Gold += amount;

                await TextArea.PrintLine();
                await TextArea.PrintLine("            Gold:  + " + amount.ToString(), XleColor.Yellow);

                SoundMan.PlaySound(LotaSound.VeryGood);
                StatsDisplay.FlashHPWhileSound(XleColor.Yellow);

                Input.WaitForKey();

                Story.SearchingForTulip = true;
            }
        }
    }
}
