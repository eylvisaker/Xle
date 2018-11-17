using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using Xle.Scenes;

namespace Xle.Blacksilver.TitleScreen
{
    [Transient]
    public class LobTitleScene : BufferedScene, ITitleScene
    {
        private readonly KeyboardEvents keyboard;
        private SpriteBatch spriteBatch;
        private Texture2D title;
        private SoundEffect music;
        private SoundEffectInstance musicInstance;
        private readonly GraphicsDevice graphics;
        private readonly IContentProvider content;

        public LobTitleScene(GraphicsDevice graphics, IContentProvider content)
            : base(graphics, 680, 440)
        {
            this.graphics = graphics;
            this.content = content;

            this.keyboard = new KeyboardEvents();
            this.spriteBatch = new SpriteBatch(graphics);

            keyboard.KeyPress += Keyboard_KeyPress;

            title = content.Load<Texture2D>("Images/Title/Blacksilver");
            music = content.Load<SoundEffect>("Audio/Music");

            musicInstance = music.CreateInstance();
            musicInstance.Play();
        }

        private void Keyboard_KeyPress(object sender, KeyPressEventArgs e)
        {
            musicInstance.Stop();


            var player = new Player("Davey");

            player.MapID = 1;
            player.X = 126;
            player.Y = 52;

            player.StoryData = new LobStory();

            player.Items[LobItem.FalconFeather] = 1;

            BeginGame?.Invoke(player);
        }

        public event Action<Player> BeginGame;

        protected override void OnUpdateInput(IInputState input)
        {
            base.OnUpdateInput(input);

            keyboard.Update(input);
        }

        protected override void DrawScene(GameTime time)
        {
            base.DrawScene(time);

            graphics.Clear(XleColor.Black);

            spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(20, 20, 0));

            spriteBatch.Draw(title, new Rectangle(0, 0, 640, 400), Color.White);

            spriteBatch.End();

        }
    }
}
