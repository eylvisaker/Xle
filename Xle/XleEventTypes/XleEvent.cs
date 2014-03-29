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
using ERY.Xle.Maps;

namespace ERY.Xle
{
	[Serializable]
	public abstract class XleEvent : IXleSerializable
	{
		private Rectangle rect;
		EventExtender mExtender;

		public XleEvent()
		{
			Enabled = true;
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
		/// Gets whether or not this type of event allows the player
		/// to rob it when the town isn't angry at him.
		/// </summary>
		[Obsolete]
		public virtual bool AllowRobWhenNotAngry { get { return false; } }

		/// <summary>
		/// Method called when the player attempts to rob and should get the 
		/// message "the merchant won't let you rob."
		/// </summary>
		[Obsolete]
		public virtual void RobFail()
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The merchant won't let you rob.");

			XleCore.Wait(1000);
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

			AfterReadData();
		}

		protected virtual void AfterReadData()
		{
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

		public bool Enabled { get; set; }

		public EventExtender Extender { get { return mExtender; } }
		protected virtual Type ExtenderType { get { return typeof(EventExtender); } }
		public void CreateExtender(XleMap map)
		{
			mExtender = CreateExtenderImpl(map);
			mExtender.TheEvent = this;
		}

		protected virtual EventExtender CreateExtenderImpl(XleMap map)
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
		public bool Speak(GameState state)
		{
			return mExtender.Speak(state);
		}
		/// <summary>
		/// Function called when player executes Rob in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool Rob(GameState state)
		{
			return mExtender.Rob(state);
		}
		/// <summary>
		/// Function called when the player executes the Open command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool Open(GameState state)
		{
			return mExtender.Open(state);
		}
		/// <summary>
		/// Function called when the player executes the Take command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool Take(GameState state)
		{
			return mExtender.Take(state);
		}
		/// <summary>
		/// Function called when the player walks inside
		/// the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool StepOn(GameState state)
		{
			return mExtender.StepOn(state);
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
		public void TryToStepOn(GameState state, int dx, int dy, out bool allowStep)
		{
			mExtender.TryToStepOn(state, dx, dy, out allowStep);
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
		public bool Use(GameState state, int item)
		{
			return mExtender.Use(state, item);
		}
		/// <summary>
		/// Function called when the player eXamines next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool Xamine(GameState state)
		{
			return mExtender.Xamine(state);
		}


		public virtual void BeforeStepOn(GameState state)
		{
			mExtender.BeforeStepOn(state);
		}


		[Obsolete]
		public virtual void OnLoad(GameState state)
		{
			mExtender.OnLoad(state);
		}
	}
}

namespace ERY.Xle.XleEventTypes
{


}