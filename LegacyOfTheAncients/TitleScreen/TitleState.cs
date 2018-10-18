using ERY.Xle.Services.Game;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Xle;

namespace ERY.Xle.LotA.TitleScreen
{
    public abstract class TitleState
    {
        public TitleState()
        {
            Colors = new ColorScheme();
            Windows = new List<TextWindow>();
        }

        public event EventHandler ReleaseAllKeys;

        public IXleRenderer Renderer { get; set; }
        public ILotaTitleScreenFactory Factory { get; set; }
        public ISoundMan SoundMan { get; set; }
        public IXleGameControl GameControl { get; set; }
        public ITextRenderer TextRenderer { get; set; }
        public IRectangleRenderer rects { get; set; }

        protected void OnReleaseAllKeys()
        {
            ReleaseAllKeys?.Invoke(this, EventArgs.Empty);
        }

        public abstract void KeyPress(Keys Keys, string keyString);

        public bool SkipWait { get; set; }

        public TitleState NewState { get; set; }


        protected void Wait(int time)
        {
            GameControl.Wait(time);
        }

        public ColorScheme Colors { get; protected set; }

        public string Title { get; set; }
        public string Prompt { get; set; }

        protected List<TextWindow> Windows { get; set; }

        public Player ThePlayer { get; protected set; }

        public virtual void Update(GameTime time)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawBackgrounds(spriteBatch);
            DrawWindows(spriteBatch);
            DrawTitle(spriteBatch);
            DrawPrompt(spriteBatch);
        }

        protected virtual void DrawTitle(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Title))
                return;

            DrawCenteredText(spriteBatch, 0, Title, Colors.BackColor, Colors.TextColor);
        }
        private void DrawPrompt(SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Prompt))
                return;

            DrawCenteredText(spriteBatch, 24, Prompt, XleColor.Yellow, Colors.BackColor);
        }

        private void DrawCenteredText(SpriteBatch spriteBatch, int y, string text, Color textColor, Color backColor)
        {
            int destx = 20 - text.Length / 2;

            FillRect(spriteBatch, new Rectangle(destx * 16, y * 16, text.Length * 16, 16), backColor);

            TextRenderer.WriteText(spriteBatch, destx * 16, y * 16, text, textColor);
        }

        private void FillRect(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            rects.Fill(spriteBatch, rectangle, color);
        }

        protected virtual void DrawWindows(SpriteBatch spriteBatch)
        {
            foreach (var wind in Windows)
            {
                Renderer.DrawObject(spriteBatch, wind);
            }
        }

        protected virtual void DrawBackgrounds(SpriteBatch spriteBatch)
        {
            DrawFrame(spriteBatch);
            DrawFrameHighlight(spriteBatch);
        }

        protected virtual void DrawFrame(SpriteBatch spriteBatch)
        {
            Renderer.DrawFrame(spriteBatch, Colors.FrameColor);
        }
        protected virtual void DrawFrameHighlight(SpriteBatch spriteBatch)
        {
            Renderer.DrawFrameHighlight(spriteBatch, Colors.FrameHighlightColor);
        }

    }
}
