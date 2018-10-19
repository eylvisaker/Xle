using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xle.Ancients.TitleScreen
{
    public abstract class MainMenuScreen : TitleState
    {
        private int titleMenu;
        protected List<string> MenuItems = new List<string>();
        private List<TextWindow> MenuItemWindows = new List<TextWindow>();
        private static Texture2D titleHeader1;
        private static Texture2D titleHeader2;

        protected TextWindow Instruction { get; private set; }
        protected TextWindow Copyright { get; private set; }


        public MainMenuScreen(IContentProvider content)
        {
            if (titleHeader1 == null)
            {
                titleHeader1 = content.Load<Texture2D>("Images/TitleHeader1");
                titleHeader2 = content.Load<Texture2D>("Images/TitleHeader2");
            }

            Instruction = new TextWindow();
            Instruction.Location = new Point(2, 18);
            Instruction.Write("(Pick option by keyboard or joystick)");

            Copyright = new TextWindow();
            Copyright.Location = new Point(2, 22);
            Copyright.Write("Copyright 1987 - Quest Software, Inc.");

            Windows.Add(Instruction);
            Windows.Add(Copyright);
        }

        public override Task KeyPress(Keys keyCode, string keyString)
        {
            if (keyCode == Keys.Down)
            {
                titleMenu++;

                if (titleMenu > 3)
                    titleMenu = 3;

                SoundMan.PlaySound(LotaSound.TitleCursor);
            }
            else if (keyCode == Keys.Up)
            {
                titleMenu--;

                if (titleMenu < 0)
                    titleMenu = 0;

                SoundMan.PlaySound(LotaSound.TitleCursor);
            }
            else if (keyCode >= Keys.D1 && keyCode <= Keys.D4)
            {
                titleMenu = keyCode - Keys.D1;

                keyCode = Keys.Enter;
            }

            if (keyCode == Keys.Enter)
            {
                SkipWait = true;
                SoundMan.PlaySound(LotaSound.TitleAccept);

                OnReleaseAllKeys();

                ExecuteMenuItem(titleMenu);
            }

            return Task.CompletedTask;
        }

        protected abstract void ExecuteMenuItem(int item);

        public override void Update(GameTime time)
        {
            if (MenuItemWindows.Count == 0)
            {
                for (int i = 0; i < 4; i++)
                {
                    MenuItemWindows.Add(new TextWindow());
                    MenuItemWindows[i].Location = new Point(7, 9 + i * 2);
                }

                Windows.AddRange(MenuItemWindows);
            }

            for (int i = 0; i < 4; i++)
            {
                var wind = MenuItemWindows[i];

                wind.Text = (i + 1).ToString() + ".  " + MenuItems[i];
            }
        }

        protected override void DrawBackgrounds(SpriteBatch spriteBatch)
        {
            base.DrawBackgrounds(spriteBatch);

            DrawTitleHeader(spriteBatch, Colors.FrameColor, Colors.FrameHighlightColor);
        }

        protected override void DrawFrame(SpriteBatch spriteBatch)
        {
            base.DrawFrame(spriteBatch);
            Renderer.DrawFrameLine(spriteBatch, 0, 20 * 16, 1,
                XleOptions.myWindowWidth, Colors.FrameColor);
        }

        protected override void DrawFrameHighlight(SpriteBatch spriteBatch)
        {
            base.DrawFrameHighlight(spriteBatch);
            Renderer.DrawInnerFrameHighlight(spriteBatch,0, 20 * 16, 1,
                XleOptions.myWindowWidth, Colors.FrameHighlightColor);
        }

        protected override void DrawWindows(SpriteBatch spriteBatch)
        {
            base.DrawWindows(spriteBatch);

            Point pt = new Point(5, 9 + titleMenu * 2);

            TextRenderer.WriteText(spriteBatch, pt.X * 16, pt.Y * 16, "`");
        }

        private void DrawTitleHeader(SpriteBatch spriteBatch, Color frameColor, Color lineColor)
        {
            //titleHeader1.InterpolationHint = InterpolationMode.Fastest;
            //titleHeader2.InterpolationHint = InterpolationMode.Fastest;

            //titleHeader1.Color = frameColor;
            //titleHeader2.Color = lineColor;

            //titleHeader1.Draw();
            //titleHeader2.Draw();

            spriteBatch.Draw(titleHeader1, new Rectangle(0, 0, titleHeader1.Width, titleHeader1.Height), frameColor);
            spriteBatch.Draw(titleHeader2, new Rectangle(0, 0, titleHeader2.Width, titleHeader2.Height), lineColor);
        }

    }
}
