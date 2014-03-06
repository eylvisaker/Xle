using AgateLib.Geometry;
using ERY.Xle.Maps.XleMapTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreFood : StoreFront
	{
		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = XleColor.DarkGray;
			cs.FrameColor = XleColor.Green;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.TextColor = XleColor.Yellow;
		}

		bool skipMailOffer = false;

		public override bool Speak(GameState state)
		{
			var player = state.Player;

			if (IsLoanOverdue(player, true))
				return true;

			string tempString;
			double cost = 15 / player.Attribute[Attributes.charm];
			int choice;
			int max = (int)(player.Gold / cost);

			SetTitle();

			this.player = player;
			this.robbing = false;

			Wait(1);

			XleCore.TextArea.PrintLine();

			if (player.mailTown == state.Map.MapID)
			{
				PayForMail(player);
				skipMailOffer = true;
			}
			else
			{
				SetWindow(cost);

				tempString = "      Maximum purchase:  ";
				tempString += max;
				tempString += " days";

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine(tempString, XleColor.Cyan);

				choice = ChooseNumber(max);

				if (choice > 0)
				{
					player.Spend((int)(choice * cost));
					player.Food += choice;

					XleCore.TextArea.PrintLine(choice + " days of food bought.");

					StoreSound(LotaSound.Sale);

					if (skipMailOffer == false)
						OfferMail(state);

					return true;
				}
				else
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Nothing Purchased");

					StoreSound(LotaSound.Medium);
				}
			}

			CheckOfferMuseumCoin(player);

			return true;

		}

		private void OfferMail(GameState state)
		{
			var player = state.Player;

			Town twn = state.Map as Town;

			if (player.Item(9) > 0) return;
			if (twn == null) return;
			if (twn.Mail.Count == 0) return;

			int mMap = XleCore.random.Next(twn.Mail.Count);
			int target;
			int count = 0;
			bool valid = false;

			// search for a valid map
			do
			{
				target = twn.Mail[mMap];

				if (XleCore.GetMapName(target) != "")
					valid = true;
				else
				{
					mMap++;
					if (mMap == twn.Mail.Count) mMap = 0;
				}

				count++;

			} while (count < 6 && valid == false);

			if (valid == false)
				return;

			SoundMan.PlaySound(LotaSound.Question);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Would you like to earn some gold?");

			MenuItemList menu = new MenuItemList("Yes", "No");

			int choice = QuickMenu(menu, 2);

			if (choice == 0)
			{
				player.ItemCount(9, 1);
				player.mailTown = target;

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Here's some mail to");
				XleCore.TextArea.PrintLine("deliver to " + XleCore.GetMapName(target) + ".");
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("        Press Key to Continue");

				WaitForKey();
			}
		}
		private void SetWindow(double cost)
		{
			LeftOffset = 9;

			int i = 1;
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "    Food & water";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "We sell food for travel.";
			theWindow[i++] = "Each 'day' of food will ";
			theWindow[i++] = "keep you fed for one day";
			theWindow[i++] = "of travel (on foot).    ";
			theWindow[i++] = "";
			theWindow[i++] = "";

			if (robbing == false)
			{
				theWindow[i] = "Cost is ";
				theWindow[i] += cost;
				theWindow[i++] += " gold per 'day'";
			}
			else
				theWindow[i] = "Robbery in progress";

			for (i = 1; i < theWindowColor.Length; i++)
				SetColor(i, XleColor.Yellow);

		}

		private int SetTitle()
		{
			int i = 0;
			theWindow[0] = ShopName;
			return i;
		}
		private void PayForMail(Player player)
		{
			int gold = XleCore.random.Next(1, 4);

			switch (gold)
			{
				case 1: gold = 95; break;
				case 2: gold = 110; break;
				case 3: gold = 125; break;
			}

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Thanks for the delivery. ");
			XleCore.TextArea.PrintLine("Here's " + gold.ToString() + " gold.");
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			StoreSound(LotaSound.Good);
			g.UpdateBottom("        Press Key to Continue");
			WaitForKey();

			player.Gold += gold;
			player.ItemCount(9, -1);
			player.mailTown = 0;
		}

		int robCount;

		public override bool Rob(GameState state)
		{
			this.player = state.Player;

			SetTitle();
			Wait(1);
			SetWindow(0);

			g.ClearBottom();

			if (robCount < 4)
			{
				robCount++;
				robbing = true;

				int choice = XleCore.random.Next(1, 16) + XleCore.random.Next(20, 36);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Stole " + choice.ToString() + " days of food.", XleColor.Yellow);

				player.Food += choice;
				SoundMan.PlaySound(LotaSound.Sale);

				if (XleCore.random.NextDouble() < 0.25)
					robCount = 4;

			}
			else
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("No items within reach now.", XleColor.Yellow);

				SoundMan.PlaySound(LotaSound.Medium);
			}

			XleCore.TextArea.PrintLine();
			Wait(2000);

			return true;
		}
	}
}
