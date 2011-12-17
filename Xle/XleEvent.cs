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
	public class Door : XleEvent
	{
		int mItem;

		protected override void ReadData(XleSerializationInfo info)
		{
			mItem = info.ReadInt32("RequiredItem", 0);
		}
		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("RequiredItem", mItem);
		}

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
}