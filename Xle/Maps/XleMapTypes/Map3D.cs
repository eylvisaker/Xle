using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.Maps;

namespace ERY.Xle.Maps.XleMapTypes
{
    public abstract class Map3D : XleMap
    {
        public override bool AutoDrawPlayer
        {
            get { return false; }
        }
    }
}
