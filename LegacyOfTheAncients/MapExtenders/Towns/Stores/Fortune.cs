using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
	public class Fortune : StoreExtender
	{
		int timesUsed;

		protected override bool SpeakImpl(GameState state)
		{
			int choice;
			int cost = 5 + (int)Math.Sqrt(state.Player.Gold) / 9;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine(TheEvent.ShopName, XleColor.Green);
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Read your fortune for " +
				cost + " gold?");

			choice = XleCore.QuickMenuYesNo();

			XleCore.TextArea.PrintLine();

			if (choice == 1)
				return true;

			if (timesUsed == 3)
			{
				XleCore.TextArea.PrintLine("\n\nI know no more.");
				return true;
			}

			timesUsed++; 

			if (cost > state.Player.Gold)
			{
				XleCore.TextArea.PrintLine("You're short on gold.");
				SoundMan.PlaySoundSync(LotaSound.Medium);

				return true;
			}

			XleCore.TextArea.Clear(true);
			XleCore.TextArea.PrintLine("\n\n");

			state.Player.Gold -= cost;

			int index = Lota.Story.NextFortune;
			
			string fortune = XleCore.Data.Fortunes[index];

			XleCore.TextArea.PrintLineSlow(fortune);
			
			Lota.Story.NextFortune++;

			XleCore.Wait(1500);

			return true;
		}
	}
}
