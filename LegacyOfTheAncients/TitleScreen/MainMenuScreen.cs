﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ERY.Xle.LotA.TitleScreen
{
    public abstract class MainMenuScreen : TitleState
    {
        private int titleMenu;
        protected List<string> MenuItems = new List<string>();
        private List<TextWindow> MenuItemWindows = new List<TextWindow>();
        private static Surface titleHeader1;
        private static Surface titleHeader2;

        protected TextWindow Instruction { get; private set; }
        protected TextWindow Copyright { get; private set; }


        public MainMenuScreen()
        {
            if (titleHeader1 == null)
            {
                titleHeader1 = new Surface("Images/TitleHeader1.png");
                titleHeader2 = new Surface("Images/TitleHeader2.png");
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

        public override void KeyDown(Keys keyCode, string keyString)
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
        protected override void DrawBackgrounds()
        {
            base.DrawBackgrounds();

            DrawTitleHeader(Colors.FrameColor, Colors.FrameHighlightColor);
        }
        protected override void DrawFrame()
        {
            base.DrawFrame();
            Renderer.DrawFrameLine(0, 20 * 16, 1,
                XleOptions.myWindowWidth, Colors.FrameColor);
        }
        protected override void DrawFrameHighlight()
        {
            base.DrawFrameHighlight();
            Renderer.DrawInnerFrameHighlight(0, 20 * 16, 1,
                XleOptions.myWindowWidth, Colors.FrameHighlightColor);
        }

        protected override void DrawWindows()
        {
            base.DrawWindows();

            Point pt = new Point(5, 9 + titleMenu * 2);

            TextRenderer.WriteText(pt.X * 16, pt.Y * 16, "`");
        }

        private void DrawTitleHeader(Color frameColor, Color lineColor)
        {
            titleHeader1.InterpolationHint = InterpolationMode.Fastest;
            titleHeader2.InterpolationHint = InterpolationMode.Fastest;

            titleHeader1.Color = frameColor;
            titleHeader2.Color = lineColor;

            titleHeader1.Draw();
            titleHeader2.Draw();
        }

    }
}
