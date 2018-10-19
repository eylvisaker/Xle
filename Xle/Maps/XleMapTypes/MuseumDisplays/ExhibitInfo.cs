using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Xle.Maps.XleMapTypes.MuseumDisplays
{
    public class ExhibitInfo
    {
        private int animFrame;
        private double animTime;

        public ExhibitInfo()
        {
            Text = new Dictionary<int, string>();
            FrameTime = 100;
        }

        public Dictionary<int, string> Text { get; private set; }

        public string ImageFile { get; set; }
        private Texture2D Image { get; set; }

        public void LoadImage(IContentProvider content)
        {
            if (string.IsNullOrEmpty(ImageFile))
                return;

            Image = content.Load<Texture2D>("Images/Museum/Exhibits/" + ImageFile);
        }

        public int FrameTime { get; set; }
        public int Frames { get { return Image.Width / 240; } }

        public void DrawImage(GameTime time, SpriteBatch spriteBatch, Rectangle destRect, int id)
        {
            animTime += time.ElapsedGameTime.TotalMilliseconds;
            if (animTime > FrameTime)
            {
                animTime %= FrameTime;
                animFrame++;
                if (animFrame >= Frames)
                    animFrame = 0;
            }

            Rectangle srcRect = new Rectangle(animFrame * 240, 128 * id, 240, 128);

            if (Image != null)
            {
                spriteBatch.Draw(Image, destRect, srcRect, Color.White);
            }
            else
            {
                System.Diagnostics.Debug.Print("Null image in exhibit: " + Text);
            }
        }
    }
}
