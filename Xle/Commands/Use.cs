using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Use : Command
	{
		public Use()
		{
			ShowItemMenu = true;
		}

		public bool ShowItemMenu { get; set; }

		public override void Execute(GameState state)
		{
			if (ShowItemMenu)
				ChooseHeldItem(state);
			else
				XleCore.TextArea.PrintLine();

			string commandstring = string.Empty;
			bool noEffect = true;

			XleCore.TextArea.PrintLine();

			string action = XleCore.ItemList[state.Player.Hold].Action;
			if (string.IsNullOrEmpty(action))
				action = "Use " + XleCore.ItemList[state.Player.Hold].Name;
			else 
				commandstring = action;

			XleCore.TextArea.PrintLine(commandstring + ".");

			switch (state.Player.Hold)
			{
				case 3:
					noEffect = false;
					EatHealingHerbs(state);
					break;

				default: 
					noEffect = !state.Map.PlayerUse(state.Player, state.Player.Hold);

					break;
			}

			if (noEffect == true)
			{
				XleCore.TextArea.PrintLine();
				XleCore.Wait(400 + 100 * state.Player.Gamespeed);
				XleCore.TextArea.PrintLine("No effect");
			}
		}

		public static void ChooseHeldItem(GameState state)
		{
			XleCore.TextArea.PrintLine("-choose above", XleColor.Cyan);
			MenuItemList theList = new MenuItemList();
			int value = 0;

			theList.Add("Nothing");

			foreach (int i in XleCore.ItemList.Keys)
			{
				if (state.Player.Items[i] > 0)
				{
					string itemName = XleCore.ItemList[i].Name;

					if (itemName.Contains("coin"))
						continue;

					/*
					if (i == 9)			// mail
					{
						itemName = XleCore.GetMapName(state.Player.mailTown) + " " + itemName;
					}*/

					if (i <= state.Player.Hold)
					{
						value++;
					}

					theList.Add(itemName);
				}

			}

			state.Player.HoldMenu(XleCore.SubMenu("Hold Item", value, theList));
		}

		private void EatHealingHerbs(GameState state)
		{
			state.Player.HP += state.Player.MaxHP / 2;
			state.Player.ItemCount(3, -1);
			SoundMan.PlaySound(LotaSound.Good);

			XleCore.FlashHPWhileSound(XleColor.Cyan);
		}

	}
}
