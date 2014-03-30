using AgateLib.Geometry;
using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
	class Healer : StoreFront
	{
		bool buyingHerbs = false;

		protected override void SetColorScheme(ColorScheme cs)
		{
			cs.BackColor = buyingHerbs ? XleColor.LightBlue : XleColor.Green;
			cs.FrameColor = XleColor.LightGreen;
			cs.FrameHighlightColor = XleColor.Yellow;
			cs.BorderColor = XleColor.Gray;
		}

		protected override bool SpeakImpl(GameState state)
		{
			var player = state.Player;

			if (TheEvent.IsLoanOverdue(state, true))
				return true;

			buyingHerbs = false;

			int woundPrice = (int)((player.MaxHP - player.HP) * 0.75);
			int herbsPrice = (int)(player.Level * 300 * TheEvent.CostFactor);

			Windows.Clear();
			SetDescriptionText();
			SetOptionsText(woundPrice, herbsPrice);

			var museum = Lota.Story.Museum;

			// display ready message
			if (Lota.Story.EatenJutonFruit && Lota.Story.PurchasedHerbs == false)
			{
				TextWindow wind = new TextWindow();
				wind.Location = new Point(3, 15);

				wind.WriteLine("You're ready for herbs!", XleColor.Blue);

				StoreSound(LotaSound.VeryGood);
			}

			MenuItemList theList = new MenuItemList("0", "1", "2");

			XleCore.TextArea.PrintLine();
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
				if (Lota.Story.EatenJutonFruit == false)
				{
					XleCore.TextArea.PrintLine("You're not ready yet.");
					SoundMan.PlaySound(LotaSound.Medium);
				}
				else
				{
					int max = player.Gold / herbsPrice;
					max = Math.Min(max, 40 - player.Items[LotaItem.HealingHerb]);

					buyingHerbs = true;

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

						player.Items[LotaItem.HealingHerb] += number;

						XleCore.TextArea.PrintLine(number.ToString() + " healing herbs purchased.");
						Lota.Story.PurchasedHerbs = true;

						StoreSound(LotaSound.Sale);
					}
				}
			}

			AfterSpeak(state);

			return true;
		}

		protected virtual void AfterSpeak(GameState state)
		{
			if (Lota.Story.HasGuardianMark == false)
				return;

			XleCore.TextArea.PrintLine("A distant healer awaits you.", XleColor.Yellow);

			SoundMan.PlaySoundSync(LotaSound.Encounter);
		}

		private void SetOptionsText(int woundPrice, int herbsPrice)
		{
			TextWindow window = new TextWindow();
			window.Location = new Point(3, 9);

			window.Write("1. Wound Care  -  ");

			if (woundPrice <= 0)
				window.WriteLine("Not needed", XleColor.Yellow);
			else
				window.WriteLine(woundPrice.ToString() + " gold");

			window.WriteLine();
			window.WriteLine();

			window.WriteLine("2. Healing Herbs -  " + herbsPrice.ToString() + " apiece");

			Windows.Add(window);
		}

		private void SetDescriptionText()
		{
			TextWindow window = new TextWindow();

			window.Location = new AgateLib.Geometry.Point(7, 3);
			window.WriteLine("Our sect offers restorative");
			window.WriteLine("    cures for your wounds.");

			Windows.Add(window);
		}
	}
}
