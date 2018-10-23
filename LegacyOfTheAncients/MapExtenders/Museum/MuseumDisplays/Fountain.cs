using AgateLib;
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

        public override void RunExhibit()
        {
            if (Player.Items[LotaItem.Tulip] == 0)
            {
                OfferTulipQuest();
            }
            else
            {
                RewardForTulip();
            }
        }

        private void RewardForTulip()
        {
            // remove the tulip from the player, give the reward and shut down the exhibit.
            Player.Items[LotaItem.Tulip] = 0;
            Player.Attribute[Attributes.charm] += 10;
            Story.ReturnedTulip = true;

            ReadRawText(ExhibitInfo.Text[3]);

            TextArea.Clear();
        }

        private void OfferTulipQuest()
        {
            bool firstVisit = HasBeenVisited;

            base.RunExhibit();
            TextArea.PrintLine();

            if (Story.SearchingForTulip == false)
                TextArea.PrintLine("Do you want to help search?");
            else
                TextArea.PrintLine("Do you want to continue searching?");

            TextArea.PrintLine();

            if (QuickMenu.QuickMenuYesNo() == 0)
            {
                ReadRawText(ExhibitInfo.Text[2]);
                int amount = 100;

                if (firstVisit || ExhibitHasBeenVisited(ExhibitIdentifier.Thornberry))
                {
                    amount += 200;
                }

                Player.Gold += amount;

                TextArea.PrintLine();
                TextArea.PrintLine("            Gold:  + " + amount.ToString(), XleColor.Yellow);

                SoundMan.PlaySound(LotaSound.VeryGood);
                StatsDisplay.FlashHPWhileSound(XleColor.Yellow);

                Input.WaitForKey();

                Story.SearchingForTulip = true;
            }
        }
    }
}
