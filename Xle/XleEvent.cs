using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace ERY.Xle
{
	[Serializable]
	public abstract class XleEvent : IXleSerializable
	{
		private Rectangle rect;

		[Browsable(false)]
		public Rectangle Rectangle
		{
			get { return rect; }
			set { rect = value; }
		}

		public int X
		{
			get { return rect.X; }
			set { rect.X = value; }
		}
		public int Y
		{
			get { return rect.Y; }
			set { rect.Y = value; }
		}
		public int Width
		{
			get { return rect.Width; }
			set { rect.Width = value; }
		}
		public int Height
		{
			get { return rect.Height; }
			set { rect.Height = value; }
		}

		/// <summary>
		/// Gets whether or not this type of event can be placed on
		/// the specified map type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public virtual bool AllowedOnMapType(XleMap type)
		{
			return true;
		}

		/// <summary>
		/// Gets whether or not this type of event allows the player
		/// to rob it when the town isn't angry at him.
		/// </summary>
		[Browsable(false)]
		public virtual bool AllowRobWhenNotAngry { get { return false; } }

		/// <summary>
		/// Method called when the player attempts to rob and should get the 
		/// message "the merchant won't let you rob."
		/// </summary>
		public virtual void RobFail()
		{
			g.AddBottom();
			g.AddBottom("The merchant won't let you rob.");

			XleCore.wait(1000);
		}
		public XleEvent()
		{
			/*
			data = new byte[g.map.SpecialDataLength() + 1];

			data[0] = 0;

			robbed = 0;
			id = -1;
			*/
		}

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("X", rect.X);
			info.Write("Y", rect.Y);
			info.Write("Width", rect.Width);
			info.Write("Height", rect.Height);

			WriteData(info);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			rect.X = info.ReadInt32("X");
			rect.Y = info.ReadInt32("Y");
			rect.Width = info.ReadInt32("Width");
			rect.Height = info.ReadInt32("Height");

			ReadData(info);
		}

		/// <summary>
		///  Override this in a derived class to write data to a map file.
		/// </summary>
		/// <param name="info"></param>
		protected virtual void WriteData(XleSerializationInfo info)
		{
		}
		/// <summary>
		///  Override this in a derived class to read data from a map file.
		/// </summary>
		/// <param name="info"></param>
		protected virtual void ReadData(XleSerializationInfo info)
		{
		}

		#endregion

		/// <summary>
		/// Function called when player speaks in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Speak(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when player executes Rob in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Rob(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player executes the Open command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Open(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player executes the Take command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Take(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player walks inside
		/// the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool StepOn(Player player)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player tries to walk inside
		/// the LotaEvent.
		/// 
		/// This is before the position is updated.  Returns false to 
		/// block the player from stepping there, and true if the
		/// player can walk there.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public virtual bool TryToStepOn(Player player, int dx, int dy)
		{
			return true;
		}
		/// <summary>
		/// Function called when the player uses an item
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Use(Player player, int item)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player eXamines next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Xamine(Player player)
		{
			return false;
		}

	}
}

namespace ERY.Xle.XleEventTypes
{
	
	[Serializable]
	public abstract class ItemAvailableEvent : XleEvent
	{
		private bool mClosed = true;
		private bool mContainsItem = false;

		private int mContents = 100;

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
	}

	[Serializable]
	public class TakeEvent : ItemAvailableEvent
	{
		public override bool Take(Player player)
		{
			return OpenImpl(player, true);
		}
	}
	[Serializable]
	public class TreasureChestEvent : ItemAvailableEvent
	{
		public override bool Open(Player player)
		{
			return OpenImpl(player, false);
		}
	}

	[Serializable]
	public class Door : XleEvent
	{
		int mItem;

		public int RequiredItem
		{
			get { return mItem; }
			set { mItem = value; }
		}

		public override bool Use(Player player, int item)
		{
			g.AddBottom("");
			g.AddBottom("The door just laughs at you.");

			return true;
		}
	}
	
	[Obsolete]
	enum StoreType
	{
		storeBank = 2,					// 2
		storeWeapon,					// 3
		storeArmor,						// 4
		storeWeaponTraining,			// 5
		storeArmorTraining,				// 6
		storeBlackjack,					// 7
		storeLending,					// 8
		storeRaft,						// 9
		storeHealer,					// 10
		storeJail,						// 11
		storeFortune,					// 12
		storeFlipFlop,					// 13
		storeBuyback,					// 14
		storeFood,						// 15
		storeVault,						// 16
		storeMagic						// 17
	}
}