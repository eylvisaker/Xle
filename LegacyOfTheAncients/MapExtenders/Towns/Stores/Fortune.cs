using ERY.Xle.Data;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
    public class Fortune : LotaStore
    {
        int timesUsed;

        public IQuickMenu QuickMenu { get; set; }
        public XleData Data { get; set; }

        protected override bool SpeakImpl(GameState state)
        {
            int choice;
            int cost = 5 + (int)Math.Sqrt(state.Player.Gold) / 9;

            TextArea.PrintLine();
            TextArea.PrintLine(TheEvent.ShopName, XleColor.Green);
            TextArea.PrintLine();
            TextArea.PrintLine("Read your fortune for " +
                cost + " gold?");

            choice = QuickMenu.QuickMenuYesNo();

            TextArea.PrintLine();

            if (choice == 1)
                return true;

            if (timesUsed == 3)
            {
                TextArea.PrintLine("\n\nI know no more.");
                return true;
            }

            timesUsed++;

            if (cost > Player.Gold)
            {
                TextArea.PrintLine("You're short on gold.");
                SoundMan.PlaySoundSync(LotaSound.Medium);

                return true;
            }

            TextArea.Clear(true);
            TextArea.PrintLine("\n\n");

            Player.Gold -= cost;

            int index = Story.NextFortune;

            string fortune = Data.Fortunes[index];

            TextArea.PrintLineSlow(fortune);

            Story.NextFortune++;

            GameControl.Wait(1500);

            return true;
        }
    }
}
