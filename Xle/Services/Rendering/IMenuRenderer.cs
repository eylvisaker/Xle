using Microsoft.Xna.Framework.Graphics;

namespace Xle.Services.Rendering
{
    public interface IMenuRenderer : IXleService
    {
        void DrawMenu(SpriteBatch spriteBatch, SubMenu menu);
    }
}
