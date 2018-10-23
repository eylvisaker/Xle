using System;
using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Xle.Services.Rendering;
using Xle.Services.ScreenModel;
using Xle.Services.XleSystem;

namespace Xle.Scenes
{
    [Transient]
    public class GamePlayScene : Scene
    {
        private readonly GraphicsDevice device;
        private readonly IXleScreen screen;
        private readonly IXleRunner gameRunner;
        private readonly IXleInput xleInput;
        private readonly IRectangleRenderer rects;
        private readonly XleSystemState systemState;
        private readonly XleRenderer renderer;
        private readonly GameState gameState;
        private readonly SpriteBatch spriteBatch;
        private readonly KeyboardEvents keyboard;

        public GamePlayScene(GraphicsDevice device,
                             IXleScreen screen,
                             IXleRunner gameRunner,
                             IXleInput xleInput,
                             IRectangleRenderer rects,
                             XleSystemState systemState,
                             XleRenderer renderer, 
                             GameState gameState)
        {
            this.device = device;
            this.screen = screen;
            this.gameRunner = gameRunner;
            this.xleInput = xleInput;
            this.rects = rects;
            this.systemState = systemState;
            this.renderer = renderer;
            this.gameState = gameState;

            this.spriteBatch = new SpriteBatch(device);

            keyboard = new KeyboardEvents();

            keyboard.KeyPress += (_, e) => xleInput.OnKeyPress(e.Key, e.KeyString);
            keyboard.KeyDown += (_, e) => xleInput.OnKeyDown(e.Key);
            keyboard.KeyUp += (_, e) => xleInput.OnKeyUp(e.Key);
        }

        public Player Player
        {
            get => gameState.Player;
            set => gameState.Player = value;
        }

        public void Run(Player player)
        {
            gameRunner.Run(player);
        }

        protected override void OnUpdateInput(IInputState input)
        {
            keyboard.Update(input);
        }

        protected override void OnUpdate(GameTime time)
        {
            base.OnUpdate(time);

            if (gameState != null && gameState.MapExtender != null)
            {
                gameState.MapExtender.OnUpdate(time);
            }

            screen.OnUpdate();

            if (systemState.ReturnToTitle)
                IsFinished = true;
        }


        protected override void DrawScene(GameTime time)
        {
            device.Clear(renderer.ColorScheme.BorderColor);

            spriteBatch.Begin(
                transformMatrix: Matrix.CreateTranslation(new Vector3(20, 20, 0)));

            rects.Fill(spriteBatch, 
                       new Rectangle(0, 0, 640, 400), 
                       renderer.ColorScheme.BackColor);

            renderer.Draw(time, spriteBatch);

            spriteBatch.End();

            //screen.OnDraw();
        }
    }
}
