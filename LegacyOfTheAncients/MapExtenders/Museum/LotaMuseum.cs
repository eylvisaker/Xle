using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Xle.Ancients.MapExtenders.Museum.MuseumDisplays;
using Xle.Maps;
using Xle.Maps.Museums;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.Commands;
using Xle.Services.Game;

namespace Xle.Ancients.MapExtenders.Museum
{
    [Transient("LotaMuseum")]
    public class LotaMuseum : MuseumExtender
    {
        private Dictionary<int, Exhibit> mExhibits = new Dictionary<int, Exhibit>();

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
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Hold());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

            commands.Items.Add(CommandFactory.Fight("MuseumFight"));
            commands.Items.Add(CommandFactory.Rob("LotaMuseumRob"));
            commands.Items.Add(CommandFactory.Take("MuseumTake"));
            commands.Items.Add(CommandFactory.Speak("MuseumSpeak"));
            commands.Items.Add(CommandFactory.Use("LotaMuseumUse"));
            commands.Items.Add(CommandFactory.Xamine("LotaMuseumXamine"));
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

        public override async Task OnAfterEntry()
        {
            await CheckInformationMessage();
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

        private async Task CheckInformationMessage()
        {
            // check to see if the caretaker wants to see the player
            var info = Information;

            if (info.ShouldLevelUp())
            {
                TextArea.Clear();
                await TextArea.PrintLine("The caretaker wants to see you!");

                await GameControl.PlaySoundSync(LotaSound.Good);
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

        public override async Task AfterPlayerStep()
        {
            if (Player.X == 12 && Player.Y == 13)
            {
                if (Story.Museum[1] < 3)
                {
                    var welcome = (Welcome)GetExhibitByTile(0x51);

                    try
                    {
                        await welcome.PlayGoldArmbandMessage();
                    }
                    catch (Exception ex)
                    {
                        Debugger.Break();
                    }

                    Story.Museum[1] = 3;

                    CheckExhibitStatus();
                }
            }
        }

        private async Task UseGoldArmband()
        {
            bool facingDoor = IsFacingDoor;

            if (facingDoor)
            {
                await GameControl.WaitAsync(1000);

                foreach (var entry in Map.EntryPoints)
                {
                    if (entry.Location == Player.Location)
                    {
                        Story.MuseumEntryPoint = Map.EntryPoints.IndexOf(entry);
                    }
                }

                await LeaveMap();
            }
            else
            {
                await TextArea.PrintLine("The gold armband hums softly.");
            }
        }

        public override async Task NeedsCoinMessage(Exhibit ex)
        {
            var lotaex = (LotaExhibit)ex;

            await TextArea.PrintLine("You'll need a " + lotaex.Coin + " coin.");
        }

        public override async Task PrintUseCoinMessage(Exhibit ex)
        {
            var lotaex = (LotaExhibit)ex;

            await TextArea.PrintLine();
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
