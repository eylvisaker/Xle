using AgateLib.DisplayLib;

namespace ERY.Xle.Services.Rendering.Implementation
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
