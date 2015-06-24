using AgateLib.DisplayLib;

namespace ERY.Xle.Services.Rendering
{
    public interface IXleImages : IXleService
    {
        void LoadTiles(string tileset);

        ISurface Tiles { get; }
    }
}
