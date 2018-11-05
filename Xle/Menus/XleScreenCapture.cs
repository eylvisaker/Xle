using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Xle.Game;
using Xle.Rendering;

namespace Xle.Menus.Implementation
{
    public interface IXleScreenCapture
    {
        event Action<KeyPressEventArgs> KeyPress;
        event Action<GameTime> Update;
        event Action<SpriteBatch> Draw;

        void Begin();
        void End();
    }

    [Transient]
    public class XleScreenCapture : IXleScreenCapture
    {
        private readonly IXleGameControl gameControl;
        private readonly GameState gameState;
        private readonly SpriteBatch spriteBatch;
        private readonly GraphicsDevice graphics;
        private readonly IXleRenderer renderer;
        private readonly ISceneStack sceneStack;
        private SubMenu menu;
        private Scene scene;
        private KeyboardEvents keyboard = new KeyboardEvents();

        public XleScreenCapture(
            GraphicsDevice graphics,
            IXleRenderer renderer,
            ISceneStack sceneStack,
            IXleGameControl gameControl,
            GameState gameState)
        {
            this.graphics = graphics;
            this.renderer = renderer;
            this.sceneStack = sceneStack;
            this.gameControl = gameControl;
            this.gameState = gameState;

            this.spriteBatch = new SpriteBatch(graphics);

            keyboard.KeyPress += Keyboard_KeyPress;
        }

        public event Action<KeyPressEventArgs> KeyPress;
        public event Action<GameTime> Update;
        public event Action<SpriteBatch> Draw;
        
        public void Begin()
        {
            this.scene = new Scene { DrawBelow = true };

            scene.Update += (sender, gameTime ) => Update?.Invoke(gameTime);
            scene.Draw += Scene_Draw;
            scene.UpdateInput += Scene_UpdateInput;

            sceneStack.Add(scene);
        }

        public void End()
        {
            sceneStack.Remove(scene);
        }

        private void Scene_UpdateInput(object sender, IInputState e)
        {
            keyboard.Update(e);
        }

        private void Scene_Draw(object sender, GameTime e)
        {
            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(new Vector3(20, 20, 0)));

            Draw?.Invoke(spriteBatch);

            spriteBatch.End();
        }

        private void Keyboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            KeyPress?.Invoke(e);
        }

    }
}
