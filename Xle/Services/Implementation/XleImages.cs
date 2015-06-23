using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.DisplayLib;

namespace ERY.Xle.Services.Implementation
{
    public class XleImages : IXleImages
    {
        public ISurface Tiles { get; private set; }

        public void LoadTiles(string tileset)
        {
            if (tileset.EndsWith(".png") == false)
                tileset += ".png";

            Tiles = new Surface(tileset) { InterpolationHint = InterpolationMode.Fastest };
        }
    }
}
