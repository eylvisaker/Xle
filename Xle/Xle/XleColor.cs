using System;
using System.Collections.Generic;
using System.Text;
using AgateLib.Geometry;

namespace ERY.Xle
{
    class XleColor
    {
        public static Color Black { get { return Color.FromArgb(0, 0, 0); } }
        public static Color White { get { return Color.FromArgb(255, 255, 255); } }
        public static Color Red { get { return Color.FromArgb(223, 63, 63); } }
        public static Color Cyan { get { return Color.FromArgb(96, 252, 252); } }
        public static Color Purple { get { return Color.FromArgb(223, 95, 223); } }
        public static Color Green { get { return Color.FromArgb(63, 223, 63); } }
        public static Color Blue { get { return Color.FromArgb(63, 63, 223); } }
        public static Color Yellow { get { return Color.FromArgb(255, 255, 63); } }
        public static Color Orange { get { return Color.FromArgb(223, 159, 63); } }
        public static Color Brown { get { return Color.FromArgb(156, 116, 72); } }
        public static Color Pink { get { return Color.FromArgb(255, 159, 159); } }
        public static Color DarkGray { get { return Color.FromArgb(80, 80, 80); } }
        public static Color Gray { get { return Color.FromArgb(128, 128, 128); } }
        public static Color LightGreen { get { return Color.FromArgb(159, 255, 159); } }
        public static Color LightBlue { get { return Color.FromArgb(159, 159, 255); } }
        public static Color LightGray { get { return Color.FromArgb(191, 191, 191); } }
    }
}
