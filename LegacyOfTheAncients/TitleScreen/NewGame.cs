using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using Xle.Ancients;

namespace Xle.Ancients.TitleScreen
{
    [Transient, InjectProperties]
    public class NewGame : TitleState
    {
        private string enteredName = "";
        private TextWindow upperWindow = new TextWindow();
        private TextWindow lowerWindow = new TextWindow();
        private TextWindow entryWindow = new TextWindow();
        private readonly IGamePersistance gamePersistance;

        public NewGame(IGamePersistance gamePersistance)
        {
            Colors.BackColor = XleColor.Green;
            Colors.FrameColor = XleColor.LightGray;
            Colors.FrameHighlightColor = XleColor.Yellow;
            Colors.BorderColor = XleColor.LightGreen;

            Title = " Start a new game ";

            ResetUpperWindow();
            ResetLowerWindow();
            ResetEntryWindow();

            Windows.Add(upperWindow);
            Windows.Add(lowerWindow);
            Windows.Add(entryWindow);
            this.gamePersistance = gamePersistance;
        }

        private void ResetEntryWindow()
        {
            entryWindow.Location = new Point(13, 11);
            entryWindow.Text = enteredName + "_";
        }

        private void ResetUpperWindow()
        {
            upperWindow.Location = new Point(2, 4);
            upperWindow.Clear();
            upperWindow.WriteLine(" Type in your new character's name.");
            upperWindow.WriteLine();
            upperWindow.WriteLine("  It may be up to 14 letters long.");
        }

        private void ResetLowerWindow()
        {
            lowerWindow.Location = new Point(3, 17);
            lowerWindow.Clear();

            lowerWindow.WriteLine("` Press return key when finished `");
            lowerWindow.WriteLine();
            lowerWindow.WriteLine();
            lowerWindow.WriteLine(" - Press 'del' key to backspace -");
            lowerWindow.WriteLine();
            lowerWindow.WriteLine("- Press 'F1' or Escape to cancel -");
        }

        public override async Task KeyPress(Keys keyCode, string keyString)
        {
            if ((keyCode >= Keys.A && keyCode <= Keys.Z) || keyCode == Keys.Space ||
                (keyCode >= Keys.D0 && keyCode <= Keys.D9))
            {
                if (enteredName.Length < 14)
                {
                    enteredName += keyString;
                    SoundMan.PlaySound(LotaSound.TitleKeypress);
                }
                else
                {
                    SoundMan.PlaySound(LotaSound.Invalid);
                }
            }
            else if (keyCode == Keys.Back || keyCode == Keys.Delete)
            {
                if (enteredName.Length > 0)
                {
                    enteredName = enteredName.Substring(0, enteredName.Length - 1);

                    SoundMan.PlaySound(LotaSound.TitleKeypress);
                }
                else
                    SoundMan.PlaySound(LotaSound.Invalid);
            }
            else if (keyCode == Keys.Escape || keyCode == Keys.F1)
            {
                SoundMan.PlaySound(LotaSound.TitleAccept);
                NewState = Factory.CreateSecondMainMenu();
            }
            else if (keyCode == Keys.Enter && enteredName.Length > 0)
            {
                if (gamePersistance.GameExists(enteredName))
                {
                    SoundMan.PlaySound(LotaSound.Medium);

                    lowerWindow.Clear();
                    lowerWindow.Location = new Point(4, 16);

                    lowerWindow.Text = enteredName + " has already begun.";

                    await Wait(2000);

                    ResetLowerWindow();
                }
                else
                {
                    lowerWindow.Clear();
                    lowerWindow.Location = new Point(4, 16);

                    lowerWindow.Text = enteredName + "'s adventures begin";

                    await SoundMan.PlaySoundWait(LotaSound.VeryGood);

                    NewState = Factory.CreateIntroduction(enteredName);
                }
            }

            ResetEntryWindow();
        }


        protected override void DrawFrame(SpriteBatch spriteBatch)
        {
            base.DrawFrame(spriteBatch);

            Renderer.DrawFrameLine(spriteBatch, 11 * 16, 10 * 16, 1, 18 * 16 - 4, Colors.FrameColor);  // top
            Renderer.DrawFrameLine(spriteBatch, 11 * 16, 12 * 16, 1, 18 * 16 - 4, Colors.FrameColor);  // bottom
            Renderer.DrawFrameLine(spriteBatch, 11 * 16, 10 * 16, 0, 3 * 16 - 4, Colors.FrameColor);   // left
            Renderer.DrawFrameLine(spriteBatch, 28 * 16, 10 * 16, 0, 3 * 16 - 4, Colors.FrameColor);   // right
        }
        protected override void DrawFrameHighlight(SpriteBatch spriteBatch)
        {
            base.DrawFrameHighlight(spriteBatch);

            Renderer.DrawInnerFrameHighlight(spriteBatch, 11 * 16, 10 * 16, 1, 18 * 16 - 4, XleColor.White);
            Renderer.DrawInnerFrameHighlight(spriteBatch, 11 * 16, 12 * 16, 1, 18 * 16 - 2, XleColor.White);
            Renderer.DrawInnerFrameHighlight(spriteBatch, 11 * 16, 10 * 16, 0, 3 * 16 - 4, XleColor.White);
            Renderer.DrawInnerFrameHighlight(spriteBatch, 28 * 16, 10 * 16, 0, 3 * 16 - 4, XleColor.White);
        }
    }
}
