using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{

	public class StoreHealer : Store
	{
		bool buyHerbs = false;

		protected override void GetColors(out Color backColor, out Color borderColor,
			out Color lineColor, out Color fontColor, out Color titleColor)
		{
			backColor = XleColor.Green;
			borderColor = XleColor.LightGreen;
			lineColor = XleColor.Yellow;
			fontColor = XleColor.White;
			titleColor = XleColor.White;

			if (buyHerbs)
				backColor = XleColor.LightBlue;
		}

		public override bool Speak(GameState state)
		{
			var player = state.Player;

			if (CheckLoan(player, true))
				return true;

			buyHerbs = false;
			int i = 0;
			this.player = player;
			int woundPrice = (int)((player.MaxHP - player.HP) * 0.75);
			int herbsPrice = (int)(player.Level * 300 * CostFactor);

			ClearWindow();

			theWindow[i++] = ShopName;
			theWindow[i++] = "";
			theWindow[i++] = "";
			theWindow[i++] = "    Our sect offers restorative";
			theWindow[i++] = "        cures for your wounds.";

			i += 4;

			string woundString = woundPrice.ToString() + " gold";

			if (woundPrice <= 0)
			{
				woundString = "Not needed";
				SetColor(i, 18, 12, XleColor.Yellow);
			}

			theWindow[i++] = "1. Wound Care  -  " + woundString;
			i += 2;

			theWindow[i++] = "2. Healing Herbs -  " + herbsPrice.ToString() + " apiece";

			throw new NotImplementedException();
			/*
			// display ready message
			if (player.museum[6] == 3)
			{
				i += 2;

				// TODO: make it blue!
				theWindow[i++] = "You're ready for herbs!";

				SoundMan.PlaySound(LotaSound.VeryGood);
				while (SoundMan.IsPlaying(LotaSound.VeryGood))
				{
					XleCore.Wait(20, RedrawStore);
				}
			}

			MenuItemList theList = new MenuItemList("0", "1", "2");

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Make choice (hit 0 to cancel)");
			XleCore.TextArea.PrintLine();

			int choice = QuickMenu(theList, 2, 0);

			if (choice == 0)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Nothing purchased");
				XleCore.TextArea.PrintLine();

				StoreSound(LotaSound.Medium);
			}
			else if (choice == 1)
			{
				XleCore.TextArea.PrintLine("You are cured.");
				player.HP = player.MaxHP;

				StoreSound(LotaSound.VeryGood);
			}
			else if (choice == 2)
			{
				if (player.museum[6] <= 1)
				{
					XleCore.TextArea.PrintLine("You're not ready yet.");
					SoundMan.PlaySound(LotaSound.Medium);
				}
				else
				{
					int max = player.Gold / herbsPrice;
					max = Math.Min(max, 40 - player.Item(3));

					buyHerbs = true;

					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Purchase how many healing herbs?");

					int number = ChooseNumber(max);

					if (number == 0)
					{
						XleCore.TextArea.PrintLine("Nothing purchased.");
						SoundMan.PlaySound(LotaSound.Medium);
					}
					else
					{
						if (player.Spend(number * herbsPrice) == false)
						{
							throw new Exception("Not enough money!");
						}

						player.ItemCount(3, number);

						XleCore.TextArea.PrintLine(number.ToString() + " healing herbs purchased.");
						player.museum[6] |= 0x04;

						StoreSound(LotaSound.Sale);
					}
				}
			}

			return true;
			 * */
		}
	}
}
