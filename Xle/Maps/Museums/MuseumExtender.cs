using System;

using AgateLib.Mathematics.Geometry;

using Xle.Maps.XleMapTypes;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Xle.Maps.Museums
{
    public class MuseumExtender : Map3DExtender
    {
        int doorVal = 2;

        public XleOptions Options { get; set; }

        public Museum Map { get { return (Museum)base.TheMap; } }

        public MuseumRenderState RenderState { get; set; } = new MuseumRenderState();

        public override IXleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.MuseumRenderer(this);
        }

        protected override void PlayPlayerMoveSound()
        {
            SoundMan.PlaySound(LotaSound.WalkMuseum);
        }
        protected override void OnBeforePlayerMove(Direction dir)
        {
            RenderState.DrawCloseup = false;
        }
        protected override void CommandTextForInvalidMovement(ref string command)
        {
            command = "Bump into wall";
        }

        public override Map3DSurfaces Surfaces()
        {
            return null;
        }

        public bool IsFacingDoor
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

        public virtual Task NeedsCoinMessage(Exhibit ex)
        {
            return Task.CompletedTask;
        }

        public virtual Task PrintUseCoinMessage(Exhibit ex)
        {
            return Task.CompletedTask;
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

        public async Task PrintExhibitStopsActionMessage()
        {
            await TextArea.PrintLine("The display case");
            await TextArea.PrintLine("force field stops you.");
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

        public async Task<bool> InteractWithDisplay()
        {
            Point lookingAt = PlayerLookingAt;

            Exhibit ex = ExhibitAt(lookingAt.X, lookingAt.Y);

            if (ex == null)
                return false;

            Input.PromptToContinueOnWait = true;
            RenderState.DrawCloseup = true;
            RenderState.Closeup = ex;
            RenderState.DrawStatic = ex.StaticBeforeCoin;

            await TextArea.PrintLine(ex.IntroductionText);
            await TextArea.PrintLine();
            await TextArea.PrintLineCentered(ex.LongName + " ", ex.TitleColor);

            Input.PromptToContinueOnWait = true;

            if (ex.IsClosed)
            {
                await TextArea.PrintLineCentered(" - Exhibit closed - ", ex.TitleColor);
                await TextArea.PrintLine();
                await Input.WaitForKey();

                return true;
            }

            await TextArea.PrintLineCentered(ex.InsertCoinText + " ", ex.TitleColor);
            await TextArea.PrintLine();
            await GameControl.WaitForKey();

            if (ex.RequiresCoin == false)
            {
                RenderState.DrawStatic = false;
                await RunExhibit(ex);
            }
            else
            {
                if (ex.HasBeenVisited == false)
                    await TextArea.PrintLine("You haven't used this exhibit.");
                else
                    await TextArea.PrintLine();

                if (Options.DisableExhibitsRequireCoins == false && ex.PlayerHasCoin == false)
                {
                    await NeedsCoinMessage(ex);
                    await GameControl.WaitAsync(500);

                    return true;
                }
                else
                {
                    await TextArea.PrintLine(ex.UseCoinMessage);
                    await TextArea.PrintLine();

                    int choice = await QuickMenu.QuickMenu(new MenuItemList("Yes", "no"), 3);

                    if (choice == 1)
                        return true;

                    if (Options.DisableExhibitsRequireCoins == false)
                        ex.UseCoin();

                    RenderState.DrawStatic = false;
                    await RunExhibit(ex);
                }
            }

            return true;
        }

        private async Task RunExhibit(Exhibit ex)
        {
            try
            {
                await ex.RunExhibit();
            }
            catch (Exception exception)
            {
                Debugger.Break();
            }

            CheckExhibitStatus();
        }
    }
}
