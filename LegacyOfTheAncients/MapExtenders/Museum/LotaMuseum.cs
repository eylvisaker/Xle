using AgateLib.Geometry;
using ERY.Xle.LotA.MapExtenders.Museum.MuseumDisplays;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Museums;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands;

namespace ERY.Xle.LotA.MapExtenders.Museum
{
    public class LotaMuseum : MuseumExtender
    {
        Dictionary<int, Exhibit> mExhibits = new Dictionary<int, Exhibit>();

        public LotaMuseum(IExhibitFactory factory)
        {
            mExhibits.Add(0x50, factory.Information());
            mExhibits.Add(0x51, factory.Welcome());
            mExhibits.Add(0x52, factory.Weaponry());
            mExhibits.Add(0x53, factory.Thornberry());
            mExhibits.Add(0x54, factory.Fountain());
            mExhibits.Add(0x55, factory.PirateTreasure());
            mExhibits.Add(0x56, factory.HerbOfLife());
            mExhibits.Add(0x57, factory.NativeCurrency());
            mExhibits.Add(0x58, factory.StonesWisdom());
            mExhibits.Add(0x59, factory.Tapestry());
            mExhibits.Add(0x5A, factory.LostDisplays());
            mExhibits.Add(0x5B, factory.KnightsTest());
            mExhibits.Add(0x5C, factory.FourJewels());
            mExhibits.Add(0x5D, factory.Guardian());
            mExhibits.Add(0x5E, factory.Pegasus());
            mExhibits.Add(0x5F, factory.AncientArtifact());
        }

        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LotaProgram.CommonLotaCommands);

            commands.Items.Add(CommandFactory.Fight("MuseumFight"));
            commands.Items.Add(CommandFactory.Rob("LotaMuseumRob"));
            commands.Items.Add(CommandFactory.Take("MuseumTake"));
            commands.Items.Add(CommandFactory.Speak("MuseumSpeak"));
            commands.Items.Add(CommandFactory.Xamine("MuseumXamine"));
        }

        public override Exhibit GetExhibitByTile(int tile)
        {
            if (mExhibits.ContainsKey(tile) == false)
                return null;

            return mExhibits[tile];
        }

        public override void OnLoad()
        {
            base.OnLoad();
            CheckExhibitStatus();
            MapRenderer.AnimateExhibits = true;
        }

        public override void OnAfterEntry()
        {
            CheckInformationMessage();
        }

        public override void CheckExhibitStatus()
        {
            // lost displays
            if (Story.Museum[0xa] > 0)
            {
                for (int i = 0; i < Map.Width; i++)
                {
                    for (int j = 0; j < Map.Height; j++)
                    {
                        if (Map[i, j] == 0x5a)
                            Map[i, j] = 0x10;
                    }
                }
            }

            // welcome exhibit
            if (Story.Museum[1] == 0)
            {
                Map[4, 1] = 0;
                Map[3, 10] = 0;
            }
            else if (Story.Museum[1] == 1)
            {
                Map[4, 1] = 0;
                Map[3, 10] = 16;
            }
            else
            {
                Map[4, 1] = 16;
                Map[3, 10] = 16;
            }
        }

        private void CheckInformationMessage()
        {
            // check to see if the caretaker wants to see the player
            var info = Information;

            if (info.ShouldLevelUp())
            {
                TextArea.Clear();
                TextArea.PrintLine("The caretaker wants to see you!");

                SoundMan.PlaySoundSync(LotaSound.Good);
            }
        }

        private Information Information
        {
            get { return (Information)GetExhibitByTile(0x50); }
        }

        public override void ModifyEntryPoint(MapEntryParams entryParams)
        {
            if (entryParams.EntryPoint < 3)
                entryParams.EntryPoint = Story.MuseumEntryPoint;
        }

        public override void PlayerUse(int item, ref bool handled)
        {
            // twist gold armband
            if (item == (int)LotaItem.GoldArmband)
            {
                UseGoldArmband();

                handled = true;
                return;
            }
        }
        public override void AfterPlayerStep()
        {
            if (Player.X == 12 && Player.Y == 13)
            {
                if (Story.Museum[1] < 3)
                {
                    var welcome = (Welcome)GetExhibitByTile(0x51);
                    welcome.PlayGoldArmbandMessage();
                    Story.Museum[1] = 3;

                    CheckExhibitStatus();
                }
            }
        }
        private void UseGoldArmband()
        {
            bool facingDoor = IsFacingDoor;

            if (facingDoor)
            {
                GameControl.Wait(1000);

                foreach (var entry in Map.EntryPoints)
                {
                    if (entry.Location == Player.Location)
                    {
                        Story.MuseumEntryPoint = Map.EntryPoints.IndexOf(entry);
                    }
                }

                LeaveMap();
            }
            else
            {
                TextArea.PrintLine("The gold armband hums softly.");
            }
        }

        public override void NeedsCoinMessage(Exhibit ex)
        {
            var lotaex = (LotaExhibit)ex;

            TextArea.PrintLine("You'll need a " + lotaex.Coin + " coin.");
        }

        public override void PrintUseCoinMessage(Exhibit ex)
        {
            var lotaex = (LotaExhibit)ex;

            TextArea.PrintLine();
        }

        public override Map3DSurfaces Surfaces()
        {
            var step = Player.FaceDirection.StepDirection();
            var first = new Point(
                Player.Location.X + step.X,
                Player.Location.Y + step.Y);

            if (Map[Player.Location.X, Player.Location.Y] == 31 ||
                Map[first.X, first.Y] == 31)
                return Lota3DSurfaces.MuseumDark;
            else
                return Lota3DSurfaces.Museum;
        }
    }
}
