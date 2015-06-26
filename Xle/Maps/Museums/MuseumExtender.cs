using System;

using AgateLib.Geometry;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Museums
{
    public class MuseumExtender : Map3DExtender
    {
        int doorVal = 2;

        public XleOptions Options { get; set; }

        public new Museum TheMap { get { return (Museum)base.TheMap; } }
        public new MuseumRenderer MapRenderer { get { return (MuseumRenderer)base.MapRenderer; } }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.MuseumRenderer(this);
        }

        protected override void PlayPlayerMoveSound()
        {
            SoundMan.PlaySound(LotaSound.WalkMuseum);
        }
        protected override void OnBeforePlayerMove(GameState state, Direction dir)
        {
            MapRenderer.DrawCloseup = false;
        }
        protected override void CommandTextForInvalidMovement(ref string command)
        {
            command = "Bump into wall";
        }

        public override Map3DSurfaces Surfaces(GameState state)
        {
            return null;
        }

        protected bool IsFacingDoor(GameState state)
        {
            Point faceDir = state.Player.FaceDirection.StepDirection();
            Point test = new Point(state.Player.X + faceDir.X, state.Player.Y + faceDir.Y);
            bool facingDoor = TheMap[test.X, test.Y] == 0x02;

            return facingDoor;
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.DarkGray;
            scheme.FrameHighlightColor = XleColor.Yellow;

            scheme.MapAreaWidth = 23;
        }

        public override void OnLoad(GameState state)
        {
            base.OnLoad(state);

            CheckExhibitStatus(state);
        }
        public virtual Exhibit GetExhibitByTile(int tile)
        {
            throw new NotImplementedException();
        }

        public virtual void CheckExhibitStatus(GameState state)
        {
        }
        public virtual void NeedsCoinMessage(Player player, Exhibit ex)
        {
        }
        public virtual void PrintUseCoinMessage(Player player, Exhibit ex)
        {
        }

        public override bool PlayerXamine(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (InteractWithDisplay(state))
                return true;

            TextArea.PrintLine("You are in an ancient museum.");

            return true;
        }
        public override bool PlayerFight(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            Point lookingAt = state.Player.Location;
            lookingAt.X += state.Player.FaceDirection.StepDirection().X;
            lookingAt.Y += state.Player.FaceDirection.StepDirection().Y;

            if (ExhibitAt(state.Player.Location) != null)
            {
                PrintExhibitStopsActionMessage();
            }
            else if (TheMap[lookingAt] == doorVal)
            {
                SoundMan.PlaySound(LotaSound.PlayerHit);

                TextArea.PrintLine("The door does not budge.");
            }
            else
                TextArea.PrintLine("There is nothing to fight.");

            return true;
        }

        private void PrintExhibitStopsActionMessage()
        {
            TextArea.PrintLine("The display case");
            TextArea.PrintLine("force field stops you.");
        }
        public override bool PlayerRob(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (ExhibitAt(state.Player.Location) != null)
            {
                PrintExhibitStopsActionMessage();
            }
            else
            {
                TextArea.PrintLine("There is nothing to rob.");
            }

            return true;
        }
        protected override bool PlayerSpeakImpl(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("There is no reply.");

            return true;
        }
        public override bool PlayerTake(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine("There is nothing to take.");

            return true;
        }

        public Exhibit ExhibitAt(Point location)
        {
            return ExhibitAt(location.X, location.Y);
        }
        public Exhibit ExhibitAt(int x, int y)
        {
            int tileAt = TheMap[x, y];

            return GetExhibitByTile(tileAt);
        }

        protected bool InteractWithDisplay(GameState state)
        {
            var player = state.Player;
            Point stepDir = player.FaceDirection.StepDirection();

            Exhibit ex = ExhibitAt(player.X + stepDir.X, player.Y + stepDir.Y);

            if (ex == null)
                return false;

            MapRenderer.DrawCloseup = true;
            MapRenderer.mCloseup = ex;
            MapRenderer.mDrawStatic = ex.StaticBeforeCoin;

            TextArea.PrintLine(ex.IntroductionText);
            TextArea.PrintLine();
            TextArea.PrintLineCentered(ex.LongName + " ", ex.TitleColor);

            Input.PromptToContinueOnWait = true;

            if (ex.IsClosed)
            {
                TextArea.PrintLineCentered(" - Exhibit closed - ", ex.TitleColor);
                TextArea.PrintLine();
                Input.WaitForKey();

                return true;
            }

            TextArea.PrintLineCentered(ex.InsertCoinText + " ", ex.TitleColor);
            TextArea.PrintLine();
            Input.WaitForKey();

            if (ex.RequiresCoin == false)
            {
                MapRenderer.mDrawStatic = false;
                RunExhibit(state, ex);
            }
            else
            {
                if (ex.HasBeenVisited == false)
                    TextArea.PrintLine("You haven't used this exhibit.");
                else
                    TextArea.PrintLine();

                if (Options.DisableExhibitsRequireCoins == false && ex.PlayerHasCoin == false)
                {
                    NeedsCoinMessage(player, ex);
                    GameControl.Wait(500);

                    return true;
                }
                else
                {
                    TextArea.PrintLine(ex.UseCoinMessage);
                    TextArea.PrintLine();

                    int choice = QuickMenu.QuickMenu(new MenuItemList("Yes", "no"), 3);

                    if (choice == 1)
                        return true;

                    if (Options.DisableExhibitsRequireCoins == false)
                        ex.UseCoin();

                    MapRenderer.mDrawStatic = false;
                    RunExhibit(state, ex);
                }
            }

            return true;
        }

        private void RunExhibit(GameState state, Exhibit ex)
        {
            ex.RunExhibit();

            CheckExhibitStatus(state);
        }


    }
}
