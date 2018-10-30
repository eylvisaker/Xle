using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;

namespace Xle.Ancients.TitleScreen
{
    [Transient, InjectProperties]
    public class Splash : TitleState
    {
        private Texture2D titleScreenSurface;         // stores the image of the title screen.
        private int frame;
        private double frameTime;
        private const int animTime = 75;
        private readonly IContentProvider content;
        private double timeUntilMusicRestarts;

        public Splash(IContentProvider content)
        {
            this.content = content;
            titleScreenSurface = content.Load<Texture2D>("Images/title");
        }

        public override Task KeyPress(Keys keyCode, string keyString)
        {
            NewState = Factory.CreateFirstMainMenu();
            titleScreenSurface.Dispose();

            return Task.CompletedTask;
        }


        public override void Update(GameTime time)
        {
            if (SoundMan.IsPlaying(LotaSound.Music) == false)
            {
                timeUntilMusicRestarts -= time.ElapsedGameTime.TotalMilliseconds;

                if (timeUntilMusicRestarts < 0)
                {
                    StartMusic();
                }
            }

            frameTime += time.ElapsedGameTime.TotalMilliseconds;

            if (frameTime > animTime)
            {
                frameTime -= animTime;
                frame++;
            }
        }

        private void StartMusic()
        {
            timeUntilMusicRestarts = 2000;
            SoundMan.PlaySound(LotaSound.Music);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle srcRect = new Rectangle(0, 0, 320, 200);
            Rectangle destRect = new Rectangle(0, 0, 640, 400);

            srcRect.Y = (frame % 8) * 200;

            spriteBatch.Draw(titleScreenSurface, destRect, srcRect, Color.White);
        }
    }
}
