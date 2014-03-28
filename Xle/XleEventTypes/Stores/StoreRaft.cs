using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class StoreRaft : Store
	{
		protected override void AfterReadData()
		{
			ExtenderName = "StoreRaft";
		}

		protected override XleEventTypes.Extenders.EventExtender CreateExtenderImpl(XleMap map)
		{
			return map.CreateEventExtender(this, typeof(StoreRaftExtender));
		}

		// map and coords that mark where a purchased raft shows up
		int mBuyRaftMap;
		Point mBuyRaftPt;

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("BuyRaftMap", mBuyRaftMap);
			info.Write("BuyRaftX", mBuyRaftPt.X);
			info.Write("BuyRaftY", mBuyRaftPt.Y);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			mBuyRaftMap = info.ReadInt32("BuyRaftMap", 0);
			mBuyRaftPt.X = info.ReadInt32("BuyRaftX", 0);
			mBuyRaftPt.Y = info.ReadInt32("BuyRaftY", 0);
		}

		public int BuyRaftMap
		{
			get { return mBuyRaftMap; }
			set { mBuyRaftMap = value; }
		}
		public Point BuyRaftPt
		{
			get { return mBuyRaftPt; }
			set { mBuyRaftPt = value; }
		}

	}
}
