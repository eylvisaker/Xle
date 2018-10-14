﻿using ERY.Xle.Services.Game;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        protected void OnReleaseAllKeys()
        {
            ReleaseAllKeys?.Invoke(this, EventArgs.Empty);
        }

        public abstract void KeyDown(Keys Keys, string keyString);

        public bool SkipWait { get; set; }

        public TitleState NewState { get; set; }


        protected void Wait(int time)
        {
            GameControl.Wait(time);
        }

        protected ColorScheme Colors { get; set; }

        public string Title { get; set; }
        public string Prompt { get; set; }

        protected List<TextWindow> Windows { get; set; }

        public Player ThePlayer { get; protected set; }

        public virtual void Update(GameTime time)
        {
        }

        public virtual void Draw()
        {
            //Display.Clear(Colors.BorderColor);
            //Display.FillRect(new Rectangle(0, 0, 640, 400), Colors.BackColor);
            throw new NotImplementedException();

            DrawBackgrounds();
            DrawWindows();
            DrawTitle();
            DrawPrompt();
        }

        protected virtual void DrawTitle()
        {
            if (string.IsNullOrEmpty(Title))
                return;

            DrawCenteredText(0, Title, Colors.BackColor, Colors.TextColor);
        }
        private void DrawPrompt()
        {
            if (string.IsNullOrEmpty(Prompt))
                return;

            DrawCenteredText(24, Prompt, XleColor.Yellow, Colors.BackColor);
        }

        private void DrawCenteredText(int y, string text, Color textColor, Color backColor)
        {
            int destx = 20 - text.Length / 2;

            Display.FillRect(new Rectangle(destx * 16, y * 16, text.Length * 16, 16), backColor);

            TextRenderer.WriteText(destx * 16, y * 16, text, textColor);
        }

        protected virtual void DrawWindows()
        {
            foreach (var wind in Windows)
            {
                Renderer.DrawObject(wind);
            }
        }

        protected virtual void DrawBackgrounds()
        {
            DrawFrame();
            DrawFrameHighlight();
        }

        protected virtual void DrawFrame()
        {
            Renderer.DrawFrame(Colors.FrameColor);
        }
        protected virtual void DrawFrameHighlight()
        {
            Renderer.DrawFrameHighlight(Colors.FrameHighlightColor);
        }

    }
}
