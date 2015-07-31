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

        public new Museum Map { get { return (Museum)base.TheMap; } }
        public new MuseumRenderer MapRenderer { get { return (MuseumRenderer)base.MapRenderer; } }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.MuseumRenderer(this);
        }

        protected override void PlayPlayerMoveSound()
        {
            SoundMan.PlaySound(LotaSound.WalkMuseum);
        }
        protected override void OnBeforePlayerMove(Direction dir)
        {
            MapRenderer.DrawCloseup = false;
        }
        protected override void CommandTextForInvalidMovement(ref string command)
        {
            command = "Bump into wall";
        }

        public override Map3DSurfaces Surfaces()
        {
            return null;
        }

        protected bool IsFacingDoor
        {
            get
            {
                Point faceDir = Player.FaceDirection.StepDirection();
                Point test = new Point(Player.X + faceDir.X, Player.Y + faceDir.Y);
                bool facingDoor = Map[test.X, test.Y] == 0x02;

                return facingDoor;
            }
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

        public override void OnLoad()
        {
            base.OnLoad();

            CheckExhibitStatus();
        }
        public virtual Exhibit GetExhibitByTile(int tile)
        {
            throw new NotImplementedException();
        }

        public virtual void CheckExhibitStatus()
        {
        }
        public virtual void NeedsCoinMessage(Exhibit ex)
        {
        }
        public virtual void PrintUseCoinMessage(Exhibit ex)
        {
        }

        public override bool PlayerFight()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            var lookingAt = PlayerLookingAt;

            if (ExhibitAt(Player.Location) != null)
            {
                PrintExhibitStopsActionMessage();
            }
            else if (Map[lookingAt] == doorVal)
            {
                SoundMan.PlaySound(LotaSound.PlayerHit);

                TextArea.PrintLine("The door does not budge.");
            }
            else
                TextArea.PrintLine("There is nothing to fight.");

            return true;
        }

        public Point PlayerLookingAt
        {
            get
            {
                Point lookingAt = Player.Location;

                lookingAt.X += Player.FaceDirection.StepDirection().X;
                lookingAt.Y += Player.FaceDirection.StepDirection().Y;

                return lookingAt;
            }
        }

        public override bool PlayerTake()
        {
            TextArea.PrintLine();
            TextArea.PrintLine("There is nothing to take.");

            return true;
        }

        private void PrintExhibitStopsActionMessage()
        {
            TextArea.PrintLine("The display case");
            TextArea.PrintLine("force field stops you.");
        }

        public Exhibit ExhibitAt(Point location)
        {
            return ExhibitAt(location.X, location.Y);
        }
        public Exhibit ExhibitAt(int x, int y)
        {
            int tileAt = Map[x, y];

            return GetExhibitByTile(tileAt);
        }

        public bool InteractWithDisplay()
        {
            Point lookingAt = PlayerLookingAt;

            Exhibit ex = ExhibitAt(lookingAt.X, lookingAt.Y);

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
                RunExhibit(ex);
            }
            else
            {
                if (ex.HasBeenVisited == false)
                    TextArea.PrintLine("You haven't used this exhibit.");
                else
                    TextArea.PrintLine();

                if (Options.DisableExhibitsRequireCoins == false && ex.PlayerHasCoin == false)
                {
                    NeedsCoinMessage(ex);
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
                    RunExhibit(ex);
                }
            }

            return true;
        }

        private void RunExhibit(Exhibit ex)
        {
            ex.RunExhibit();

            CheckExhibitStatus();
        }


    }
}
