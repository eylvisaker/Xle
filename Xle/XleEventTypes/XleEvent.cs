using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.Serialization.Xle;

using ERY.Xle.Maps;
using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.XleEventTypes
{
    [Serializable]
    public abstract class XleEvent : IXleSerializable
    {
        private Rectangle rect;

        public XleEvent()
        {
            Enabled = true;
        }

        public string ExtenderName { get; set; }

        [Browsable(false)]
        public Rectangle Rectangle
        {
            get { return rect; }
            set { rect = value; }
        }

        public Point Location { get { return Rectangle.Location; } set { rect.Location = value; } }
        public Size Size { get { return rect.Size; } set { rect.Size = value; } }

        public int X
        {
            get { return rect.X; }
            set { rect.X = value; }
        }
        public int Y
        {
            get { return rect.Y; }
            set { rect.Y = value; }
        }
        public int Width
        {
            get { return rect.Width; }
            set { rect.Width = value; }
        }
        public int Height
        {
            get { return rect.Height; }
            set { rect.Height = value; }
        }
		
        /// <summary>
        /// Gets whether or not this type of event allows the player
        /// to rob it when the town isn't angry at him.
        /// </summary>
        [Obsolete]
        public virtual bool AllowRobWhenNotAngry { get { return false; } }

        /// <summary>
        /// Method called when the player attempts to rob and should get the 
        /// message "the merchant won't let you rob."
        /// </summary>
        [Obsolete]
        public virtual void RobFail()
        {
            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine();
            XleCore.TextArea.PrintLine("The merchant won't let you rob.");

            XleCore.Wait(1000);
        }

        #region IXleSerializable Members

        void IXleSerializable.WriteData(XleSerializationInfo info)
        {
            info.Write("X", rect.X);
            info.Write("Y", rect.Y);
            info.Write("Width", rect.Width);
            info.Write("Height", rect.Height);
            info.Write("ExtenderName", ExtenderName);

            WriteData(info);
        }
        void IXleSerializable.ReadData(XleSerializationInfo info)
        {
            rect.X = info.ReadInt32("X");
            rect.Y = info.ReadInt32("Y");
            rect.Width = info.ReadInt32("Width");
            rect.Height = info.ReadInt32("Height");
            ExtenderName = info.ReadString("ExtenderName", "");

            ReadData(info);

            AfterReadData();
        }

        protected virtual void AfterReadData()
        {
        }

        /// <summary>
        ///  Override this in a derived class to write data to a map file.
        /// </summary>
        /// <param name="info"></param>
        protected virtual void WriteData(XleSerializationInfo info)
        {
        }
        /// <summary>
        ///  Override this in a derived class to read data from a map file.
        /// </summary>
        /// <param name="info"></param>
        protected virtual void ReadData(XleSerializationInfo info)
        {
        }

        #endregion

        public bool Enabled { get; set; }

        public virtual Type ExtenderType { get { return typeof(EventExtender); } }

    }
}