using AgateLib;
using AgateLib.Foundation;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Xle.XleSystem;

namespace Xle.Ancients
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class LegacyOfTheAncientsGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private XleProgram xle;
        private Plumbing plumbing;
        private SpriteBatch spriteBatch;

        public LegacyOfTheAncientsGame(Config config)
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = config.FullScreen;
            graphics.HardwareModeSwitch = false;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Window.Title = "Legacy of the Ancients";

            plumbing = new Plumbing();

            plumbing.Register(GraphicsDevice);
            plumbing.Register(new SceneStack());
            plumbing.Register(new ContentProvider(Content));

            plumbing.Complete();

            var initializer = plumbing.Resolve<IInitializer>();
            initializer.Initialize();

            xle = plumbing.Resolve<XleProgram>();

            this.Window.AllowUserResizing = true;
            this.Window.ClientSizeChanged += Window_ClientSizeChanged;

            void Window_ClientSizeChanged(object sender, EventArgs e)
            {
                graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                graphics.ApplyChanges();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            xle.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            xle.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
