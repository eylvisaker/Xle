using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Platform;
using AgateLib.InputLib.Legacy;

using ERY.Xle.Rendering;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle.LotA.TitleScreen
{
    public class LotaTitleScreen : IXleTitleScreen
    {
        TitleState state;

        Surface titleScreenSurface;			// stores the image of the title screen.
        Surface titleHeader1;
        Surface titleHeader2;

        TitleScreenState titleState = TitleScreenState.NoState;

        int titleMenu;				// current menu location in the title screen

        string[] wnd = new string[25];
        Color[][] color = new Color[25][];
        Color bgColor = XleColor.Black;
        Color borderColor = XleColor.DarkGray;

        string tempName;
        string[] files = new string[8];
        int page;
        int maxPages = 0;

        Player player;
        bool titleDone = false;

        double lastTime = 0;
        int waitTime = 0;

        private IXleRenderer renderer;
        private ISoundMan soundMan;

        public LotaTitleScreen(
            IXleRenderer renderer,
            ISoundMan soundMan,
            ILotaTitleScreenFactory factory)
        {
            this.renderer = renderer;
            this.soundMan = soundMan;

            state = factory.CreateSplash();
        }

        void ClearTitleText()
        {
            for (int i = 0; i < 25; i++)
            {
                wnd[i] = "";
                color[i] = new Color[40];

                for (int j = 0; j < 40; j++)
                {
                    color[i][j] = XleColor.White;
                }
            }
        }

        private void UpdateTitleScreen()
        {
            // update cursor position
            if (titleMenu > 0 && (titleState == TitleScreenState.Menu1
                || titleState == TitleScreenState.Menu2))
            {
                int loc = 7 + titleMenu * 2;

                for (int i = 9; i < 16; i += 2)
                {
                    SetText(i, 4, " ");

                    if (i == loc)
                    {
                        SetText(i, 4, "`");
                    }

                }

            }
            else if (titleState == TitleScreenState.LoadGame)
            {
                int loc = 7 + titleMenu;

                if (titleMenu > 0)
                    loc++;
                if (titleMenu == 9)
                    loc++;

                for (int i = 7; i < 19; i++)
                {
                    SetText(i, 5, " ");

                    if (i == loc)
                    {
                        SetText(i, 5, "`");
                    }
                }
            }

            switch (titleState)
            {
                case TitleScreenState.NewGameMusic:

                    if (soundMan.IsPlaying(LotaSound.VeryGood) == false)
                    {
                        SetNewGameText();

                        Wait(100);
                    }

                    break;
            }
        }
        void DisplayTitleScreen()
        {
            renderer.ReplacementDrawMethod = state.Draw;

            state.Update();

            Display.BeginFrame();
            state.Draw();
            Display.EndFrame();
        }

        private void SetText(int y, int x, string text)
        {
            if (wnd[y] == null)
                wnd[y] = "";

            if (wnd[y].Length < x)
            {
                wnd[y] += new string(' ', x - wnd[y].Length) + text;
            }
            else
            {

                string temp = wnd[y].Substring(0, x);

                temp += text;

                if (wnd[y].Length > x + text.Length)
                    temp += wnd[y].Substring(x + text.Length);

                wnd[y] = temp;
            }
        }

        public void Run()
        {
            if (titleScreenSurface == null)
            {
                titleScreenSurface = new Surface("title.png");
                titleHeader1 = new Surface("TitleHeader1.png");
                titleHeader2 = new Surface("TitleHeader2.png");
            }

            player = null;
            titleDone = false;
            titleState = TitleScreenState.NoState;

            Keyboard.KeyDown += new InputEventHandler(Keyboard_KeyDown);

            while (Display.CurrentWindow.IsClosed == false && !titleDone)
            {
                UpdateTitleScreen();
                DisplayTitleScreen();

                if (state.ThePlayer != null)
                {
                    player = state.ThePlayer;
                    break;
                }

                Core.KeepAlive();
            }

            renderer.ReplacementDrawMethod = null;

            Keyboard.KeyDown -= Keyboard_KeyDown;
        }

        void Keyboard_KeyDown(InputEventArgs e)
        {
            KeyCode keyCode = e.KeyCode;

            if (lastTime + waitTime > Timing.TotalMilliseconds)
                return;

            lastTime = Timing.TotalMilliseconds;
            waitTime = 50;

            state.SkipWait = false;
            state.KeyDown(e.KeyCode, e.KeyString);

            if (state.SkipWait)
                waitTime = 0;

            if (state.NewState != null)
                state = state.NewState;
        }

        private void Wait(int milliseconds)
        {
            Keyboard.KeyDown -= Keyboard_KeyDown;
            IStopwatch watch = Timing.CreateStopWatch();

            while (watch.TotalMilliseconds < milliseconds)
            {
                DisplayTitleScreen();
                Core.KeepAlive();
            }

            Keyboard.KeyDown += Keyboard_KeyDown;
        }

        void WriteSlowToString(ref string target, string source)
        {

            int i = 0;
            int length = source.Length;

            while (i <= length)
            {
                target = source.Substring(0, i);

                i++;

                if (Keyboard.AnyKeyPressed == false)
                    Wait(62);
                else
                {
                    i++;
                    Wait(1);
                }
            }

            Wait(1);

        }

        void SetNewGameText()
        {
            titleState = TitleScreenState.NewGameText;
            titleMenu = 0;


            ClearTitleText();
            bgColor = XleColor.DarkGray;

            wnd[0] = "             Start a new game";

            for (int i = 0; i < 25; i++)
            {
                wnd[i] = "  ";
            }

            for (int i = 0; i < 40; i++)
            {
                color[24][i] = XleColor.Yellow;
            }

            {
                int i = 4;

                WriteSlowToString(ref wnd[i++], "  You are only a poor peasant on the");
                WriteSlowToString(ref wnd[i++], "  world of Tarmalon, so it's hardly");
                WriteSlowToString(ref wnd[i++], "  surprising that you've never seen");
                WriteSlowToString(ref wnd[i++], "  a dead man before.  His crumpled");
                WriteSlowToString(ref wnd[i++], "  figure lies forlornly by the side");
                WriteSlowToString(ref wnd[i++], "  of the road.");
                WriteSlowToString(ref wnd[i++], "                                   ");

                i++;

                WriteSlowToString(ref wnd[i++], "  Fighting your fear, you kneel by");
                WriteSlowToString(ref wnd[i++], "  the still-warm corpse.  You see a");
                WriteSlowToString(ref wnd[i++], "  a look of panic on his face, a gold");
                WriteSlowToString(ref wnd[i++], "  band around his wrist, and a large");
                WriteSlowToString(ref wnd[i++], "  leather scroll, clutched tightly to");
                WriteSlowToString(ref wnd[i++], "  his chest.");
            }
            wnd[24] = "    (Press key/button to continue)";

            Keyboard.ReleaseAllKeys();
            while (Keyboard.AnyKeyPressed == false)
            {
                Wait(50);
            }

            ClearTitleText();

            wnd[0] = "             Start a new game";

            for (int i = 0; i < 25; i++)
            {
                wnd[i] = "  ";
            }

            for (int i = 0; i < 40; i++)
            {
                color[24][i] = XleColor.Yellow;
            }
            {
                int i = 4;

                WriteSlowToString(ref wnd[i++], "  You've never been a thief, yet");
                WriteSlowToString(ref wnd[i++], "  something compels you to reach for");
                WriteSlowToString(ref wnd[i++], "  the leather scroll.  Getting the");
                WriteSlowToString(ref wnd[i++], "  armband off is trickier, but you");
                WriteSlowToString(ref wnd[i++], "  manage to snap it around your own");
                WriteSlowToString(ref wnd[i++], "  wrist.  You scoop up two green coins");
                WriteSlowToString(ref wnd[i++], "  lying nearby and hasten on your way.");
                WriteSlowToString(ref wnd[i++], "                                   ");

                i++;

                WriteSlowToString(ref wnd[i++], "  Before you've gone more than a few");
                WriteSlowToString(ref wnd[i++], "  steps, your senses waver and shift.");
                WriteSlowToString(ref wnd[i++], "  Rising from the mists, as though ");
                WriteSlowToString(ref wnd[i++], "  you've never been this way before, ");
                WriteSlowToString(ref wnd[i++], "  is a magnificient structure of ");
                WriteSlowToString(ref wnd[i++], "  polished stone.  A shimmering arch-");
                WriteSlowToString(ref wnd[i++], "  way beckons.");
            }

            wnd[24] = "    (Press key/button to continue)";


            Keyboard.ReleaseAllKeys();
            while (Keyboard.AnyKeyPressed == false)
            {
                Wait(50);
            }

            titleDone = true;
        }

        public Player Player
        {
            get { return player; }
        }

    }
}