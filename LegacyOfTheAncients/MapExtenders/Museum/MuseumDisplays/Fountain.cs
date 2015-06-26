using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays
{
    public class Fountain : LotaExhibit
    {
        public Fountain() : base("A Fountain", Coin.Jade) { }

        public IXleRenderer Renderer { get; set; }

        public override ExhibitIdentifier ExhibitIdentifier { get { return ExhibitIdentifier.Fountain; } }
        public override string LongName
        {
            get
            {
                return "Enchanted flower fountain";
            }
        }

        public override bool IsClosed(ERY.Xle.Player player)
        {
            return Story.ReturnedTulip;
        }

        public override void RunExhibit(Player player)
        {
            if (player.Items[LotaItem.Tulip] == 0)
            {
                OfferTulipQuest();
            }
            else
            {
                RewardForTulip(player);
            }
        }

        private void RewardForTulip(Player player)
        {
            // remove the tulip from the player, give the reward and shut down the exhibit.
            player.Items[LotaItem.Tulip] = 0;
            player.Attribute[Attributes.charm] += 10;
            Story.ReturnedTulip = true;

            ReadRawText(ExhibitInfo.Text[3]);

            TextArea.Clear();
        }

        private void OfferTulipQuest()
        {
            bool firstVisit = HasBeenVisited(Player);

            base.RunExhibit(Player);
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

                if (firstVisit || HasBeenVisited(Player, ExhibitIdentifier.Thornberry))
                {
                    amount += 200;
                }

                Player.Gold += amount;

                TextArea.PrintLine();
                TextArea.PrintLine("            Gold:  + " + amount.ToString(), XleColor.Yellow);

                SoundMan.PlaySound(LotaSound.VeryGood);
                Renderer.FlashHPWhileSound(XleColor.Yellow);

                Input.WaitForKey();

                Story.SearchingForTulip = true;
            }
        }
    }
}
