using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreExtender : EventExtender
	{
		private bool mRobbed = false;

		public bool Robbed
		{
			get { return mRobbed; }
			set { mRobbed = value; }
		}

		/// <summary>
		/// Stores the player object for use when redrawing.
		/// </summary>
		protected Player player;
		/// <summary>
		/// Bool indicating whether or not we are robbing this store.
		/// </summary>
		protected bool robbing;

		public new Store TheEvent { get { return (Store)base.TheEvent; } }

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

		public override bool Speak(GameState state)
		{
			return StoreNotImplementedMessage();
		}

		protected bool StoreNotImplementedMessage()
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine(TheEvent.ShopName, XleColor.Yellow);
			XleCore.TextArea.PrintLine("");
			XleCore.TextArea.PrintLine("A Sign Says, ");
			XleCore.TextArea.PrintLine("Closed for remodelling.");

			SoundMan.PlaySound(LotaSound.Medium);

			XleCore.TextArea.PrintLine();

			XleCore.Wait(1000);

			return true;
		}

		public override bool Rob(GameState state)
		{
			if (Robbed)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No items within reach here.");
				XleCore.Wait(1000);
				return true;
			}

			int value = RobValue();

			if (value == 0)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("There's nothing to really carry here.");
				XleCore.Wait(1000);
				return true;
			}

			state.Player.Gold += value;
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You get " + value.ToString() + " gold.", XleColor.Yellow);
			XleCore.Wait(1000);
			Robbed = true;

			return true;
		}

		public virtual int RobValue()
		{
			return 0;
		}

		/// <summary>
		/// Method called when the player attempts to rob and should get the 
		/// message "the merchant won't let you rob."
		/// </summary>
		public virtual void RobFail()
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The merchant won't let you rob.");

			XleCore.Wait(1000);
		}

		/// <summary>
		/// Gets whether or not this type of event allows the player
		/// to rob it when the town isn't angry at him.
		/// </summary>
		public virtual bool AllowRobWhenNotAngry { get { return false; } }

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
