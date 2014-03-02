﻿using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes
{
	public class StoreBank : Store
	{
		public override int RobValue()
		{
			return XleCore.random.Next(180, 231);
		}
		public override bool AllowRobWhenNotAngry
		{
			get
			{
				return true;
			}
		}
		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.DarkGray;
			borderColor = XleColor.Green;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.Yellow;
		}
		public override bool Speak(GameState state)
		{
			var player = state.Player;

			int i = 0;
			int choice;
			int amount;

			this.player = player;
			robbing = false;

			theWindow[i++] = "Convenience Bank";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "Our services   ";
			theWindow[i++] = "---------------";
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "1.  Deposit Funds   ";
			theWindow[i++] = "";
			theWindow[i++] = "2.  Withdraw Funds  ";
			theWindow[i++] = "";
			theWindow[i++] = "3.  Balance Inquiry  ";

			g.AddBottom("");
			g.AddBottom("Make choice (Hit 0 to cancel)");
			g.AddBottom("");

			MenuItemList theList = new MenuItemList("0", "1", "2", "3");
			choice = QuickMenu(theList, 2, 0);

			switch (choice)
			{

				case 1:
					g.AddBottom("");
					g.AddBottom("Deposit how much?");
					amount = ChooseNumber(player.Gold);

					player.Spend(amount);
					player.GoldInBank += amount;

					break;
				case 2:
					if (player.GoldInBank > 0)
					{

						g.AddBottom("");
						g.AddBottom("Withdraw how much?");
						amount = ChooseNumber(player.GoldInBank);

						player.Gold += amount;
						player.GoldInBank -= amount;
					}
					else
					{
						g.ClearBottom();
						g.AddBottom("Nothing to withdraw");

						StoreSound(LotaSound.Medium);
						choice = 0;

					}
					break;
			}

			if (choice > 0)
			{
				g.AddBottom("Current balance: " + player.GoldInBank + " gold.");

				if (choice != 3)
				{
					StoreSound(LotaSound.Sale);
				}

			}

			return true;
		}
	}
}
