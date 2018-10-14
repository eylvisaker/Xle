using ERY.Xle.Serialization;
using Microsoft.Xna.Framework;

namespace ERY.Xle.XleEventTypes.Stores
{
    public class StoreRaft : Store
    {

        // map and coords that mark where a purchased raft shows up
        private int mBuyRaftMap;
        private Point mBuyRaftPt;

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

        protected override void AfterReadData()
        {
            ExtenderName = "StoreRaft";
        }
    }
}
