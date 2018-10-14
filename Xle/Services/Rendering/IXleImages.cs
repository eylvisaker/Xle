using Microsoft.Xna.Framework.Graphics;

namespace ERY.Xle.Services.Rendering
{
    public interface IXleImages : IXleService
    {
        void LoadTiles(string tileset);

        Texture2D Tiles { get; }
    }
}
