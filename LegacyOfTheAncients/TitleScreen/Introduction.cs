using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Rendering;

namespace ERY.Xle.LotA.TitleScreen
{
    public class Introduction : TitleState
    {
        private string enteredName;
        TextWindow window = new TextWindow();
        int page = 0;
        LotaStory story;

        public Introduction(string enteredName, LotaStory story)
        {
            this.enteredName = enteredName;
            Title = " start a new game ";

            Colors.BorderColor = XleColor.Blue;
            Colors.BackColor = XleColor.DarkGray;
            Colors.FrameColor = XleColor.LightGray;
            Colors.FrameHighlightColor = XleColor.Yellow;

            Windows.Add(window);
            window.Location = new Point(2, 4);

            SetFirstWindow();

            this.story = story;
            Prompt = "(Press key/button to continue)";
        }

        private void SetFirstWindow()
        {
            window.Clear();

            window.WriteLine("You are only a poor peasant on the");
            window.WriteLine("world of Tarmalon, so it's hardly");
            window.WriteLine("surprising that you've never seen");
            window.WriteLine("a dead man before.  His crumpled");
            window.WriteLine("figure lies forlornly by the side");
            window.WriteLine("of the road.");
            window.WriteLine("");
            window.WriteLine("");
            window.WriteLine("Fighting your fear, you kneel by");
            window.WriteLine("the still-warm corpse.  You see a");
            window.WriteLine("a look of panic on his face, a gold");
            window.WriteLine("band around his wrist, and a large");
            window.WriteLine("leather scroll, clutched tightly to");
            window.WriteLine("his chest.");
        }

        void SetSecondWindow()
        {
            page = 1;
            window.Clear();

            window.WriteLine("You've never been a thief, yet");
            window.WriteLine("something compels you to reach for");
            window.WriteLine("the leather scroll.  Getting the");
            window.WriteLine("armband off is trickier, but you");
            window.WriteLine("manage to snap it around your own");
            window.WriteLine("wrist.  You scoop up two green coins");
            window.WriteLine("lying nearby and hasten on your way.");
            window.WriteLine("");
            window.WriteLine("");
            window.WriteLine("Before you've gone more than a few");
            window.WriteLine("steps, your senses waver and shift.");
            window.WriteLine("Rising from the mists, as though ");
            window.WriteLine("you've never been this way before, ");
            window.WriteLine("is a magnificient structure of ");
            window.WriteLine("polished stone.  A shimmering arch-");
            window.WriteLine("way beckons.");
        }

        void CreatePlayer()
        {
            var player = new Player(enteredName);
            player.MapID = 5;
            player.Location = new Point(3, 1);
            player.FaceDirection = Direction.West;

            player.returnX = 114;
            player.returnY = 42;
            player.returnMap = 1;

            player.Items[LotaItem.GoldArmband] = 1;
            player.Items[LotaItem.Compendium] = 1;
            player.Items[LotaItem.JadeCoin] = 2;

            player.AddArmor(1, 0);
            player.CurrentArmor = player.Armor[0];
            player.VaultGold = 1500;

            player.StoryData = story;
            player.SavePlayer();

            ThePlayer = player;
        }


        public override void KeyDown(AgateLib.InputLib.KeyCode keyCode, string keyString)
        {
            if (page == 0)
                SetSecondWindow();
            else
                CreatePlayer();
        }
    }
}
