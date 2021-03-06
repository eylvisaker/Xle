﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xle.Serialization;
using Xle.XleEventTypes.Extenders;
using System.ComponentModel;
using Xle.Maps;

namespace Xle.XleEventTypes
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
