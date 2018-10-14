using ERY.Xle.Serialization;
using ERY.Xle.XleEventTypes.Extenders;
using Microsoft.Xna.Framework;
using System;

namespace ERY.Xle.XleEventTypes
{
    [Serializable]
    public class ChangeMapEvent : XleEvent
    {
        private int mMapID;
        private bool mAsk = true;
        private Point mLocation;
        private string mCommandText = "";

        public override Type ExtenderType
        {
            get
            {
                return typeof(ChangeMap);
            }
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

        [Obsolete("Call the extender instead.", true)]
        public void ExecuteMapChange(Player player)
        {
        }
    }
}
