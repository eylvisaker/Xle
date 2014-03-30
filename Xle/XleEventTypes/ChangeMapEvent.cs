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

namespace ERY.Xle.XleEventTypes
{
	[Serializable]
	public class ChangeMapEvent : XleEvent
	{
		private int mMapID;
		private bool mAsk = true;
		private Point mLocation;
		private string mCommandText = "";
		ChangeMapExtender mExtender;

		protected override Type ExtenderType
		{
			get
			{
				return typeof(ChangeMapExtender);
			}
		}
		protected override EventExtender CreateExtenderImpl(XleMap map)
		{
			mExtender = (ChangeMapExtender)base.CreateExtenderImpl(map);

			return mExtender;
		}

		/// <summary>
		/// Text used as a command.
		/// Use {0} to indicate town we are in, {1} to indicate town we are 
		/// going to.
		/// </summary>
		public string CommandText
		{
			get { return mCommandText; }
			set { mCommandText = value; }
		}

		/// <summary>
		/// Whether or not to ask the player to change maps
		/// </summary>
		public bool Ask
		{
			get { return mAsk; }
			set { mAsk = value; }
		}

		public int TargetEntryPoint { get; set; }

		/// <summary>
		/// What map ID to go to.
		/// </summary>
		public int MapID
		{
			get { return mMapID; }
			set { mMapID = value; }
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("MapID", MapID);
			info.Write("AskUser", mAsk);
			info.Write("TargetEntryPoint", TargetEntryPoint);
			info.Write("CommandText", mCommandText);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			MapID = info.ReadInt32("MapID");
			mAsk = info.ReadBoolean("AskUser");
			TargetEntryPoint = info.ReadInt32("TargetEntryPoint");
			mCommandText = info.ReadString("CommandText", "");
		}
		public void ExecuteMapChange(Player player)
		{
			mExtender.ExecuteMapChange(player);
		}
	}
}
