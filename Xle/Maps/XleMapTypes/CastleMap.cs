using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.Serialization;

namespace ERY.Xle.Maps.XleMapTypes
{
    public class CastleMap : Town
    {
        public override IEnumerable<string> AvailableTileImages
        {
            get
            {
                yield return "CastleTiles.png";
            }
        }

    }
}
