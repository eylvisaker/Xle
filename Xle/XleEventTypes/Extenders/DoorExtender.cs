using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class DoorExtender : EventExtender
	{
		public new Door TheEvent { get { return (Door)base.TheEvent; } }

		[Obsolete]
		public virtual void PlayRemoveSound(ref bool handled)
		{
		}

		[Obsolete]
		public virtual void RemoveDoor(GameState state, ref bool handled)
		{
		}

		public virtual bool ItemUnlocksDoor(GameState state, int item)
		{
			return item == TheEvent.RequiredItem;
		}

		public virtual void PrintUnlockText(GameState state, int item, ref bool handled)
		{
			XleCore.TextArea.PrintLine("Unlock door.");
		}

		public virtual void PrintUnlockFailureText(GameState state, int item, ref bool handled)
		{
			XleCore.TextArea.PrintLine();
			XleCore.Wait(300 + 200 * state.Player.Gamespeed);
			XleCore.TextArea.PrintLine("It doesn't fit this door.");
		}

		public override bool Use(GameState state, int item)
		{
			if (XleCore.Data.ItemList.IsKey(item) == false)
				return false;

			bool itemUnlocksDoor = ItemUnlocksDoor(state, item);

			if (itemUnlocksDoor)
			{
				UnlockDoor(state, item);
			}
			else
			{
				bool unused = false;
				PrintUnlockFailureText(state, item, ref unused);
			}

			return true;
		}

		protected virtual void UnlockDoor(GameState state, int item)
		{
			bool handled = false;

			PrintUnlockText(state, item, ref handled);

			PlayRemoveSound();
			RemoveDoor(state);
		}

		public virtual void PlayRemoveSound()
		{
			SoundMan.PlaySound(LotaSound.UnlockDoor);
			XleCore.Wait(250);
		}
		public virtual void RemoveDoor(GameState state)
		{
			for (int j = TheEvent.Rectangle.Y; j < TheEvent.Rectangle.Bottom; j++)
			{
				for (int i = TheEvent.Rectangle.X; i < TheEvent.Rectangle.Right; i++)
				{
					state.Map[i, j] = TheEvent.ReplacementTile;
				}
			}

		}
	}
}
