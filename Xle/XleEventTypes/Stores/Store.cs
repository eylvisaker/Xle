using ERY.Xle.Serialization;

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

        public double CostFactor
        {
            get { return mCostFactor; }
            set { mCostFactor = value; }
        }
    }

}
