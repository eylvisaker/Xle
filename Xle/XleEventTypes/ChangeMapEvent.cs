﻿using System;
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

		protected override void AfterReadData()
		{
			if (string.IsNullOrEmpty(ExtenderName))
				ExtenderName = "ChangeMap";
		}

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
		/// <summary>
		/// What point on the new map to go to
		/// </summary>
		[Browsable(false)]
		public Point TargetLocation
		{
			get { return mLocation; }
			set { mLocation = value; }
		}

		public int TargetEntryPoint { get; set; }

		public int TargetX
		{
			get { return mLocation.X; }
			set { mLocation.X = value; }
		}
		public int TargetY
		{
			get { return mLocation.Y; }
			set { mLocation.Y = value; }
		}

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

		public string GetMapName()
		{
			return XleCore.Data.MapList[mMapID].Name;
		}

		private bool VerifyMapExistence()
		{
			try
			{
				string mapName = GetMapName();
			}
			catch
			{
				SoundMan.PlaySound(LotaSound.Medium);

				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("Map ID " + mMapID + " not found.");
				XleCore.TextArea.PrintLine();

				XleCore.Wait(1500);

				return false;
			}

			return true;
		}

	}

}
