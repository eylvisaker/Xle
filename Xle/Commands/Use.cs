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

			string action = XleCore.Data.ItemList[state.Player.Hold].Action;

			if (string.IsNullOrEmpty(action))
				action = "Use " + XleCore.Data.ItemList[state.Player.Hold].Name;

			XleCore.TextArea.PrintLine(action + ".");

			if (state.Player.Hold == XleCore.Factory.HealingItemID)
			{
				noEffect = false;
				UseHealingItem(state, state.Player.Hold);
			}
			else
			{
				noEffect = !state.Map.PlayerUse(state.Player, state.Player.Hold);
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

			foreach (int i in XleCore.Data.ItemList.Keys)
			{
				if (state.Player.Items[i] > 0)
				{
					string itemName = XleCore.Data.ItemList[i].Name;

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

		private void UseHealingItem(GameState state, int itemID)
		{
			state.Player.HP += state.Player.MaxHP / 2;
			state.Player.Items[itemID] -= 1;
			SoundMan.PlaySound(LotaSound.Good);

			XleCore.FlashHPWhileSound(XleColor.Cyan);
		}

	}
}
