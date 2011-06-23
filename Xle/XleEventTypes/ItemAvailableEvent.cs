using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;

namespace ERY.Xle.XleEventTypes
{
	public abstract class ItemAvailableEvent : XleEvent
	{
		private bool mClosed = true;
		private bool mContainsItem = false;

		private int mContents = 0;

		public bool ContainsItem
		{
			get { return mContainsItem; }
			set { mContainsItem = value; }
		}
		public int Contents
		{
			get { return mContents; }
			set { mContents = value; }
		}

		public bool Closed
		{
			get { return mClosed; }
			set { mClosed = value; }
		}

		protected bool OpenImpl(Player player, bool isTaking)
		{
			Commands.UpdateCommand("Open Chest");

			if (Closed == false)
			{
				g.AddBottom("");
				g.AddBottom("Chest already open.");

				return true;
			}

			SoundMan.PlaySound(LotaSound.OpenChest);

			XleCore.wait(750);


			for (int j = this.Rectangle.Top; j < this.Rectangle.Bottom; j++)
			{
				for (int i = this.Rectangle.Left; i < this.Rectangle.Right; i++)
				{
					int m = XleCore.Map[i, j];

					if (m % 16 >= 11 && m % 16 < 14 && m / 16 >= 13 && m / 16 < 15)
					{
						XleCore.Map[i, j] = m - 3;
					}

				}
			}

			if (XleCore.Map is IHasGuards)
			{
				(XleCore.Map as IHasGuards).IsAngry = true;
			}

			if (ContainsItem)
			{
				int count = 1;
				string itemName = XleCore.ItemList[Contents].Name;

				//TODO: Loadstring (g.hInstance(), dave.data[1] + 19, tempChars, 40);

				if (Contents == 8)
					count = XleCore.random.Next(3, 6);

				player.ItemCount(Contents, count);

				g.AddBottom("");


				if (isTaking == false)
				{
					g.AddBottom("You find a " + itemName + "!");
					SoundMan.PlaySound(LotaSound.VeryGood);
				}
				else
					g.AddBottom("You take " + count.ToString() + " " + itemName + ".");


			}
			else
			{
				int gd = mContents;

				g.AddBottom("");
				g.AddBottom("You find " + gd.ToString() + " gold.");

				player.Gold += gd;
				SoundMan.PlaySound(LotaSound.Sale);
			}

			mClosed = false;

			XleCore.wait(500 + 200 * player.Gamespeed);

			return true;
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("ContainsItem", mContainsItem);
			info.Write("Contents", mContents);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			mContainsItem = info.ReadBoolean("ContainsItem", false);
			mContents = info.ReadInt32("Contents", 0);
		}
	}

	public class TakeEvent : ItemAvailableEvent
	{
		public override bool Take(Player player)
		{
			return OpenImpl(player, true);
		}
	}
	public class TreasureChestEvent : ItemAvailableEvent
	{
		public override bool Open(Player player)
		{
			return OpenImpl(player, false);
		}
		public override bool Take(Player player)
		{
			g.AddBottom();
			g.AddBottom("You can't \"take\" the whole chest.");

			return true;
		}
	}
}
