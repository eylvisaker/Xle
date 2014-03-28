using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreExtender : EventExtender
	{
		/// <summary>
		/// Returns true if the loan for the player is over due and stores should not
		/// speak with him and optionally displays a message to the player.  
		/// Returns false if the current map has no lending association
		/// regardless of whether the player has an overdue loan.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="displayMessage">Pass true to display a default message indicating
		/// that the shop keeper will not talk to the player.</param>
		/// <returns>Returns true if the loan for the player is over due and stores should not
		/// speak with him and optionally displays a message to the player.  
		/// Returns false if the current map has no lending association
		/// regardless of whether the player has an overdue loan.</returns>
		public bool IsLoanOverdue(GameState state, bool displayMessage)
		{
			if (state.Map.Events.Any(x => x is StoreLending) == false)
				return false;

			var player = state.Player;

			if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
			{
				if (displayMessage)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Sorry.  I can't talk to you.");
					XleCore.Wait(500);
				}

				return true;
			}
			else
				return false;
		}

		[Obsolete]
		bool robbing = false;

		public virtual void CheckOfferMuseumCoin(Player player)
		{
			if (XleCore.random.Next(1000) < 45 && robbing == false)
			{
				OfferMuseumCoin(player);
			}
		}
		public static void OfferMuseumCoin(Player player)
		{
			int coin = -1;
			MenuItemList menu = new MenuItemList("Yes", "No");

			coin = XleCore.Factory.NextMuseumCoinOffer(XleCore.GameState);

			if (coin == -1)
				return;

			// TODO: only allow player to buy a coin if he has less than Level of that type of coins.
			int amount = 50 + (int)(XleCore.random.NextDouble() * 20 * player.Level);

			if (amount > player.Gold)
				amount /= 2;

			SoundMan.PlaySound(LotaSound.Question);

			XleCore.TextArea.PrintLine("Would you like to buy a ");
			XleCore.Wait(1);

			XleCore.TextArea.PrintLine("museum coin for " + amount.ToString() + " gold?");
			XleCore.Wait(1);

			XleCore.TextArea.PrintLine();
			XleCore.Wait(1);

			int choice = XleCore.QuickMenu(menu, 3, 0);

			if (choice == 0)
			{
				if (player.Spend(amount))
				{
					string coinName = XleCore.Data.ItemList[coin].Name;

					XleCore.TextArea.PrintLine("Use this " + coinName + " well!");

					player.Items[coin] += 1;

					SoundMan.PlaySound(LotaSound.Sale);
				}
				else
				{
					XleCore.TextArea.PrintLine("Not enough gold.");
					SoundMan.PlaySound(LotaSound.Medium);
				}
			}
		}

	}
}
