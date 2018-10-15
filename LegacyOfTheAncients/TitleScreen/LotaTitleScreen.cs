using AgateLib;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.XleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ERY.Xle.LotA.TitleScreen
{
    public interface ILotaTitleScreen : IXleTitleScreen
    {
        void Update(GameTime time);
    }

    [Transient]
    public class LotaTitleScreen : ILotaTitleScreen
    {
        private TitleState state;
        private Texture2D titleScreenTexture2D;         // stores the image of the title screen.
        private Texture2D titleHeader1;
        private Texture2D titleHeader2;
        private TitleScreenState titleState = TitleScreenState.NoState;
        private int titleMenu;              // current menu location in the title screen

        private string[] wnd = new string[25];
        private Color[][] color = new Color[25][];
        private Color bgColor = XleColor.Black;
        private Color borderColor = XleColor.DarkGray;
        private string tempName;
        private string[] files = new string[8];
        private int page;
        private int maxPages = 0;
        private Player player;
        private bool titleDone = false;
        private double lastTime = 0;
        private int waitTime = 0;

        private IXleRenderer renderer;
        private ISoundMan soundMan;
        private readonly IContentProvider content;
        private bool waiting = false;

        public LotaTitleScreen(
            IXleRenderer renderer,
            ISoundMan soundMan,
            IContentProvider content,
            ILotaTitleScreenFactory factory)
        {
            this.renderer = renderer;
            this.soundMan = soundMan;
            this.content = content;
            State = factory.CreateSplash();
        }

        private TitleState State
        {
            get { return state; }
            set
            {
                if (state != null)
                {
                    state.ReleaseAllKeys -= state_ReleaseAllKeys;
                }

                state = value;
                state.ReleaseAllKeys += state_ReleaseAllKeys;
            }
        }

        private void state_ReleaseAllKeys(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //input.Keys.ReleaseAll();
        }

        private void ClearTitleText()
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

        public void Update(GameTime time)
        {
            UpdateTitleScreen();
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

        private void DisplayTitleScreen()
        {
            renderer.ReplacementDrawMethod = State.Draw;

            //State.Update();

            //Display.BeginFrame();
            //State.Draw();
            //Display.EndFrame();
            throw new NotImplementedException();
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
            if (titleScreenTexture2D == null)
            {
                titleScreenTexture2D = content.Load<Texture2D>("Images/title.png");
                titleHeader1 = content.Load<Texture2D>("Images/TitleHeader1.png");
                titleHeader2 = content.Load<Texture2D>("Images/TitleHeader2.png");
            }

            player = null;
            titleDone = false;
            titleState = TitleScreenState.NoState;

            throw new NotImplementedException();

            //using (input = new SimpleInputHandler())
            //{
            //    Input.Handlers.Add(input);
            //    input.KeyDown += Keyboard_KeyDown;

            //    while (AgateApp.IsAlive && !titleDone)
            //    {
            //        UpdateTitleScreen();
            //        DisplayTitleScreen();

            //        if (State.ThePlayer != null)
            //        {
            //            player = State.ThePlayer;
            //            break;
            //        }

            //        AgateApp.KeepAlive();
            //    }

            //    renderer.ReplacementDrawMethod = null;
            //}
        }

        private void Keyboard_KeyDown(object sender, object /*AgateInputEventArgs */e)
        {
            if (waiting)
                return;

            throw new NotImplementedException();
            //KeyCode keyCode = e.KeyCode;

            //if (lastTime + waitTime > Timing.TotalMilliseconds)
            //    return;

            //lastTime = Timing.TotalMilliseconds;
            //waitTime = 50;

            //State.SkipWait = false;
            //State.KeyDown(e.KeyCode, e.KeyString);

            //if (State.SkipWait)
            //    waitTime = 0;

            //if (State.NewState != null)
            //    State = State.NewState;
        }

        private void Wait(int milliseconds)
        {
            //try
            //{
            //    waiting = true;
            //    IStopwatch watch = Timing.CreateStopWatch();

            //    while (watch.TotalMilliseconds < milliseconds)
            //    {
            //        DisplayTitleScreen();
            //        AgateApp.KeepAlive();
            //    }
            //}
            //finally
            //{
            //    waiting = false;
            //}
            throw new NotImplementedException();

        }

        private void WriteSlowToString(ref string target, string source)
        {

            int i = 0;
            int length = source.Length;

            while (i <= length)
            {
                target = source.Substring(0, i);

                i++;

                throw new NotImplementedException();
                //if (input.Keys.Any == false)
                //    Wait(62);
                //else
                {
                    i++;
                    Wait(1);
                }
            }

            Wait(1);

        }

        private void SetNewGameText()
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

            throw new NotImplementedException();
            //input.Keys.ReleaseAll();
            //while (input.Keys.Any == false)
            //{
            //    Wait(50);
            //}

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

            throw new NotImplementedException();
            //input.Keys.ReleaseAll();
            //while (input.Keys.Any == false)
            //{
            //    Wait(50);
            //}

            titleDone = true;
        }

        public Player Player
        {
            get { return player; }
        }

    }
}