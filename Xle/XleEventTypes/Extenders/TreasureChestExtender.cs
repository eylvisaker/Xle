using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class TreasureChestExtender : NullEventExtender
	{
		public new TreasureChestEvent TheEvent { get { return (TreasureChestEvent)base.TheEvent; } }

		public virtual void SetAngry(GameState state)
		{
			state.Map.Guards.IsAngry = true;
		}

		public virtual bool MakesGuardsAngry { get { return true; } }

		public virtual void BeforeGiveItem(GameState state, ref int item, ref int count)
		{
		}

		public virtual void PrintObtainItemMessage(GameState state, int item, int count)
		{
			var itemName = XleCore.Data.ItemList[item].Name;
			var space = "aeiou".Contains(itemName.ToLowerInvariant()[0]) ? "n " : " ";

			XleCore.TextArea.PrintLine("You find a" + space + itemName + "!");
		}

		public virtual void PlayObtainItemSound(GameState state, int item, int count)
		{
			SoundMan.PlaySound(LotaSound.VeryGood);
		}

		public virtual string AlreadyOpenMessage { get { return "Chest already open."; } }

		public virtual void PlayOpenChestSound()
		{
			SoundMan.PlaySound(LotaSound.OpenChest);
		}

		public virtual string TakeFailMessage { get { return "You can't \"take\" the whole chest."; } }

		public virtual void MarkChestAsOpen(GameState state)
		{
		}

		public virtual void OpenIfMarked(GameState state)
		{
		}
	}
}
