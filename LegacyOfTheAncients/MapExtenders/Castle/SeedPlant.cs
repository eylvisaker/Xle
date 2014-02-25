﻿using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class SeedPlant : TreasureChestExtender
	{
		public override void BeforeGiveItem(GameState state, ref int item, ref int count)
		{
			if (item == 8)
				count = XleCore.random.Next(3, 6);
		}

		public override bool MakesGuardsAngry
		{
			get { return false; }
		}

		public override void PrintObtainItemMessage(GameState state, int item, int count)
		{
			XleCore.TextArea.PrintLine("You take " + count.ToString() + " " + XleCore.ItemList[item].Name + "s.");
		}
		public override void PlayObtainItemSound(GameState state, int item, int count)
		{
		}
		public override void PlayOpenChestSound()
		{
		}

		public override void Take(GameState state, ref bool handled)
		{
			TheEvent.OpenImpl(state);
			handled = true;
		}

		public override string AlreadyOpenMessage
		{
			get
			{
				return "Nothing to take.";
			}
		}

		public override void MarkChestAsOpen(GameState state)
		{
			// do nothing, as this is supposed to refresh every time the map is reloaded.
		}
	}
}
