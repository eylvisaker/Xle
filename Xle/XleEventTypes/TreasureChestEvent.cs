﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.Serialization.Xle;
using ERY.Xle.XleEventTypes.Extenders;
using System.ComponentModel;
using ERY.Xle.Maps;

namespace ERY.Xle.XleEventTypes
{
	public class TreasureChestEvent : XleEvent
	{
		protected override void AfterReadData()
		{
			if (string.IsNullOrEmpty(ExtenderName))
				ExtenderName = "TreasureChest";
		}

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

		[Browsable(false)]
		public bool Closed
		{
			get { return mClosed; }
			set { mClosed = value; }
		}

		[Browsable(false)]
		public int ChestID { get; internal set; }

		public new TreasureChestExtender Extender { get; set; }

		protected override Type ExtenderType
		{
			get
			{
				return typeof(TreasureChestExtender);
			}
		}
		protected override EventExtender CreateExtenderImpl(XleMap map)
		{
			Extender = (TreasureChestExtender)base.CreateExtenderImpl(map);

			return Extender;
		}

		public bool OpenImpl(GameState state)
		{
			UpdateCommand();

			XleCore.TextArea.PrintLine();

			if (Closed == false)
			{
				PrintAlreadyOpenMessage();

				return true;
			}
			bool handled = false;
			
			Extender.Open(state, ref handled);
			if (handled)
				return true;

			Extender.PlayOpenChestSound();

			XleCore.Wait(state.GameSpeed.CastleOpenChestSoundTime);

			SetOpenTilesOnMap(state.Map);

			if (Extender.MakesGuardsAngry)
				Extender.SetAngry(state);

			if (ContainsItem)
			{
				int count = 1;
				int item = Contents;

				Extender.BeforeGiveItem(state, ref item, ref count);

				state.Player.Items[item] += count;

				Extender.PrintObtainItemMessage(state, item, count);
				Extender.PlayObtainItemSound(state, item, count);
			}
			else
			{
				int gd = mContents;

				XleCore.TextArea.PrintLine("You find " + gd.ToString() + " gold.");

				state.Player.Gold += gd;
				SoundMan.PlaySound(LotaSound.Sale);
			}

			mClosed = false;

			XleCore.Wait(state.GameSpeed.CastleOpenChestTime);

			Extender.MarkChestAsOpen(state);

			return true;
		}

		public void SetOpenTilesOnMap(XleMap map)
		{
			var firstTile = map[X, Y];
			var chestGroup = map.TileSet.TileGroups.FirstOrDefault(
				x => x.GroupType == Maps.GroupType.Chest && x.Tiles.Contains(firstTile));
			var openChestGroup = (from grp in map.TileSet.TileGroups
								  where grp.GroupType == Maps.GroupType.OpenChest &&
									 grp.Tiles.All(x => x > firstTile)
								  orderby grp.Tiles.Min()
								  select grp).FirstOrDefault();

			Closed = false;

			if (chestGroup == null || openChestGroup == null)
				return;

			for (int j = this.Rectangle.Top; j < this.Rectangle.Bottom; j++)
			{
				for (int i = this.Rectangle.Left; i < this.Rectangle.Right; i++)
				{
					int index = chestGroup.Tiles.IndexOf(map[i, j]);

					if (index == -1 || index >= openChestGroup.Tiles.Count)
						continue;

					map[i, j] = openChestGroup.Tiles[index];
				}
			}
		}

		private static void UpdateCommand()
		{
			XleCore.TextArea.PrintLine(" chest");
		}

		private void PrintAlreadyOpenMessage()
		{
			XleCore.TextArea.PrintLine(Extender.AlreadyOpenMessage);
		}


		public void OpenIfMarked(GameState state)
		{
			Extender.OpenIfMarked(state);
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
}
