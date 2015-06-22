using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Services.Implementation;

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
