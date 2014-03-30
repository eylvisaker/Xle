using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;
using System.ComponentModel;
using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using ERY.Xle.Maps;

namespace ERY.Xle.XleEventTypes.Stores
{
	public class Store : XleEvent
	{
		private double mCostFactor = 1.0;
		private bool mRobbed = false;
		private string mShopName;

		public Store()
		{
		}

		public string ShopName
		{
			get { return mShopName; }
			set { mShopName = value; }
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("ShopName", mShopName);
			info.Write("CostFactor", mCostFactor);
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			mShopName = info.ReadString("ShopName");
			mCostFactor = info.ReadDouble("CostFactor");
		}

		protected override XleEventTypes.Extenders.EventExtender CreateExtenderImpl(XleMap map)
		{
			return map.CreateEventExtender<StoreExtender>(this);
		}

		public double CostFactor
		{
			get { return mCostFactor; }
			set { mCostFactor = value; }
		}

		public new StoreExtender Extender { get { return (StoreExtender)base.Extender; } }
	}

}
