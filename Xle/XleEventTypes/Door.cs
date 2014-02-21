using AgateLib.Serialization.Xle;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes
{
	[Serializable]
	public class Door : XleEvent
	{
		DoorExtender mExtender;

		protected override Extenders.IEventExtender CreateExtenderImpl(XleMap map)
		{
			return mExtender = map.CreateEventExtender<DoorExtender>(this);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			RequiredItem = info.ReadInt32("RequiredItem", 0);
			ReplacementTile = info.ReadInt32("ReplacementTile", 0);
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("RequiredItem", RequiredItem);
			info.Write("ReplacementTile", ReplacementTile);
		}

		public int RequiredItem { get; set; }
		public int ReplacementTile { get; set; }

		public override bool Use(GameState state, int item)
		{
			if (XleCore.ItemList.IsKey(item) == false)
				return false;

			bool itemUnlocksDoor = item == RequiredItem;
			mExtender.ItemUnlocksDoor(state, item, ref itemUnlocksDoor);

			if (itemUnlocksDoor)
			{
				UnlockDoor(state, item);
			}
			else
			{
				UnlockFailureText(state, item);
			}

			return true;
		}

		private void UnlockFailureText(GameState state, int item)
		{
			bool handled = false;

			mExtender.PrintUnlockFailureText(state, item, ref handled);
			if (handled)
				return;
				
			g.AddBottom("");
			XleCore.wait(300 + 200 * state.Player.Gamespeed);
			g.AddBottom("This key does nothing here.");
		}

		private void UnlockDoor(GameState state, int item)
		{
			bool handled = false;
			
			mExtender.PrintUnlockText(state, item, ref handled);
			
			if (handled == false)
				g.AddBottom("Unlock door");

			PlayRemoveSound();
			RemoveDoor(state);
		}

		public void PlayRemoveSound()
		{
			bool handled = false;

			mExtender.PlayRemoveSound(ref handled);

			if (handled)
				return;

			SoundMan.PlaySound(LotaSound.UnlockDoor);
			XleCore.wait(250);
		}
		public void RemoveDoor(GameState state)
		{
			bool handled = false;

			mExtender.RemoveDoor(state, ref handled);
			if (handled)
				return;

			for (int j = Rectangle.Y; j < Rectangle.Bottom; j++)
			{
				for (int i = Rectangle.X; i < Rectangle.Right; i++)
				{
					state.Map[i, j] = ReplacementTile;
				}
			}

		}
	}
}
