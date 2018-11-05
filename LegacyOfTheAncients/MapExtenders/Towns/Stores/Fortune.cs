using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Menus;

namespace Xle.Ancients.MapExtenders.Towns.Stores
{
    public class Fortune : LotaStore
    {
        private int timesUsed;

        public IQuickMenu QuickMenu { get; set; }
        public XleData Data { get; set; }

        protected override async Task<bool> SpeakImplAsync()
        {
            int choice;
            int cost = 5 + (int)Math.Sqrt(Player.Gold) / 9;

            await TextArea.PrintLine();
            await TextArea.PrintLine(TheEvent.ShopName, XleColor.Green);
            await TextArea.PrintLine();
            await TextArea.PrintLine("Read your fortune for " +
                cost + " gold?");

            choice = await QuickMenu.QuickMenuYesNo();

            await TextArea.PrintLine();

            if (choice == 1)
                return true;

            if (timesUsed == 3)
            {
                await TextArea.PrintLine("\n\nI know no more.");
                return true;
            }

            timesUsed++;

            if (cost > Player.Gold)
            {
                await TextArea.PrintLine("You're short on gold.");
                await GameControl.PlaySoundWait(LotaSound.Medium);

                return true;
            }

            TextArea.Clear(true);
            await TextArea.PrintLine("\n\n");

            Player.Gold -= cost;

            int index = Story.NextFortune;

            string fortune = Data.Fortunes[index];

            await TextArea.PrintLineSlow(fortune);

            Story.NextFortune++;

            await Wait(1500);

            return true;
        }
    }
}
