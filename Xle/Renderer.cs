using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xle.Maps;
using Xle.Services.Rendering;

namespace Xle
{
    /// <summary>
    /// Interface for an object which renders the entire screen.
    /// </summary>
    public interface IRenderer
    {
        ColorScheme ColorScheme { get; }

        void Draw(SpriteBatch spriteBatch);

        void Update(GameTime time);
    }

    [InjectProperties]
    public abstract class Renderer : IRenderer
    {
        public Renderer()
        {
            ColorScheme.BorderColor = XleColor.Gray;
        }

        public IXleRenderer XleRenderer { get; set; }

        public GameState GameState { get; set; }

        public XleMap TheMap => GameState.Map;

        public ColorScheme ColorScheme { get; set; }

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Update(GameTime time)
        {
        }
    }
}
