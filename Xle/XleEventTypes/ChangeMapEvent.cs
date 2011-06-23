using System;
using System.Collections.Generic;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;

namespace ERY.Xle.XleEventTypes
{
	[Serializable]
	public class ChangeMapEvent : XleEvent
	{
		private int mMapID;
		private bool mAsk = true;
		private Point mLocation;
		private string mCommandText = "";

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
		public Point Location
		{
			get { return mLocation; }
			set { mLocation = value; }
		}


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
			info.Write("TargetX", TargetX);
			info.Write("TargetY", TargetY);
			info.Write("CommandText", mCommandText);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			MapID = info.ReadInt32("MapID");
			mAsk = info.ReadBoolean("AskUser");
			TargetX = info.ReadInt32("TargetX");
			TargetY = info.ReadInt32("TargetY");
			mCommandText = info.ReadString("CommandText", "");
		}

		public override bool StepOn(Player player)
		{
			if (player.X < X) return false;
			if (player.Y < Y) return false;
			if (player.X >= X + Width) return false;
			if (player.Y >= Y + Height) return false;

			string line = "Enter ";
			string newTownName;
			int choice = 0;

			MenuItemList theList = new MenuItemList("Yes", "No");

			try
			{
				newTownName = XleCore.GetMapName(mMapID);
			}
			catch
			{
				SoundMan.PlaySound(LotaSound.Medium);

				g.AddBottom("");
				g.AddBottom("Map ID " + mMapID + " not found.");
				g.AddBottom("");

				XleCore.wait(1500);

				return false;
			}
			line += newTownName + "?";

			if (Ask)
			{
				g.AddBottom("");
				g.AddBottom(line);

				SoundMan.PlaySound(LotaSound.Question);

				choice = XleCore.QuickMenu(theList, 3);
			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Leave " + XleCore.Map.MapName);

				XleCore.wait(1000);
			}
			if (string.IsNullOrEmpty(CommandText) == false)
			{
				g.AddBottom("");
				g.AddBottom(string.Format(CommandText, XleCore.Map.MapName, newTownName));

				g.AddBottom("");
				XleCore.wait(500);

				choice = 0;
			}


			if (choice == 0)
			{

				try
				{
					player.SetMap(mMapID, mLocation.X, this.mLocation.Y);

					CheckLoan(player);
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine(e.Message);

					SoundMan.PlaySound(LotaSound.Medium);

					ColorStringBuilder builder = new ColorStringBuilder();

					builder.AddText("Failed to load ", XleColor.White);
					builder.AddText(newTownName, XleColor.Red);
					builder.AddText(".", XleColor.White);

					g.AddBottom(builder);
					g.AddBottom("");

					XleCore.wait(1500);
				}
			}

			return true;
		}

		protected static void CheckLoan(Player player)
		{
			if (XleCore.Map.HasEventType(typeof(StoreLending)))
			{
				if (player.loan > 0 && player.dueDate - player.TimeDays <= 0)
				{
					g.AddBottom("This is your friendly lender.");
					g.AddBottom("You owe me money!");

					XleCore.wait(1000);

				}
			}
		}
	}

}
