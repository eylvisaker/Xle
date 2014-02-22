using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle
{
	[Serializable]
	public abstract class XleEvent : IXleSerializable
	{
		private Rectangle rect;
		IEventExtender extender;

		public XleEvent()
		{
		}

		public string ExtenderName { get; set; }

		[Browsable(false)]
		public Rectangle Rectangle
		{
			get { return rect; }
			set { rect = value; }
		}

		public Point Location { get { return Rectangle.Location; } set { rect.Location = value; } }
		public Size Size { get { return rect.Size; } set { rect.Size = value; } }

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

		#region IXleSerializable Members

		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("X", rect.X);
			info.Write("Y", rect.Y);
			info.Write("Width", rect.Width);
			info.Write("Height", rect.Height);
			info.Write("ExtenderName", ExtenderName);

			WriteData(info);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			rect.X = info.ReadInt32("X");
			rect.Y = info.ReadInt32("Y");
			rect.Width = info.ReadInt32("Width");
			rect.Height = info.ReadInt32("Height");
			ExtenderName = info.ReadString("ExtenderName", "");

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

		protected virtual Type ExtenderType { get { return typeof(NullEventExtender); } }
		public void CreateExtender(XleMap map)
		{
			extender = CreateExtenderImpl(map);
			extender.TheEvent = this;
		}

		protected virtual IEventExtender CreateExtenderImpl(XleMap map)
		{
			return map.CreateEventExtender(this, ExtenderType);
		}

		/// <summary>
		/// Function called when player speaks in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Speak(GameState state)
		{
			bool handled = false;
			extender.Speak(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when player executes Rob in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Rob(GameState state)
		{
			bool handled = false;
			extender.Rob(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player executes the Open command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Open(GameState state)
		{
			bool handled = false;
			extender.Open(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player executes the Take command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Take(GameState state)
		{
			bool handled = false;
			extender.Take(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player walks inside
		/// the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool StepOn(GameState state)
		{
			bool handled = false;
			extender.StepOn(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player tries to walk inside
		/// the XleEvent.
		/// 
		/// This is before the position is updated.  Returns false to 
		/// block the player from stepping there, and true if the
		/// player can walk there.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public virtual void TryToStepOn(GameState state, int dx, int dy, out bool allowStep)
		{
			allowStep = true;
			extender.TryToStepOn(state, dx, dy, ref allowStep);
		}
		/// <summary>
		/// Function called when the player uses an item
		/// or next to the XleEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Use(GameState state, int item)
		{
			bool handled = false;
			extender.Use(state, item, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player eXamines next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Xamine(GameState state)
		{
			bool handled = false;
			extender.Xamine(state, ref handled);

			return handled;
		}


		public virtual void BeforeStepOn(GameState state)
		{
			extender.BeforeStepOn(state);
		}
	}
}

namespace ERY.Xle.XleEventTypes
{


}