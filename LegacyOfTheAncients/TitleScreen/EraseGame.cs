using AgateLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading.Tasks;
using Xle.Ancients;
using Xle.Serialization;

namespace Xle.Ancients.TitleScreen
{
    [Transient, InjectProperties]
    public class EraseGame : FileMenu
    {
        private bool inPrompt;
        private TextWindow instruction;
        private TextWindow cursor;
        private string selectedFile;
        private int selection = 0;
        private readonly IGamePersistance gamePersistance;

        public EraseGame(IGamePersistance gamePersistance) : base(gamePersistance)
        {
            Colors.FrameColor = XleColor.Red;
            Colors.FrameHighlightColor = XleColor.Yellow;
            Colors.BackColor = XleColor.Gray;
            Colors.BorderColor = XleColor.Purple;

            Title = " Erase a character ";

            instruction = new TextWindow();

            instruction.Location = new Point(3, 21);
            instruction.WriteLine("(Select by joystick or number keys)", XleColor.Yellow);

            Windows.Add(instruction);

            var prompt = new TextWindow();

            prompt.Location = new Point(9, 5);
            prompt.WriteLine("Erase which character?");

            Windows.Add(prompt);
            this.gamePersistance = gamePersistance;
        }

        protected override void UserSelectedCancel()
        {
            NewState = Factory.CreateSecondMainMenu();
        }

        public override Task KeyPress(Keys keyCode, string keyString)
        {
            if (inPrompt == false)
            {
                return base.KeyPress(keyCode, keyString);
            }

            if (keyCode == Keys.Y)
            {
                selection = 0;
                keyCode = Keys.Enter;
            }
            else if (keyCode == Keys.N)
            {
                selection = 1;
                keyCode = Keys.Enter;
            }
            else if (keyCode == Keys.Right)
                selection = 1;
            else if (keyCode == Keys.Left)
                selection = 0;

            cursor.Location = new Point(19 + 4 * selection, cursor.Location.Y);

            if (keyCode == Keys.Enter)
            {
                NewState = Factory.CreateSecondMainMenu();

                if (selection == 0)
                {
                    gamePersistance.Delete(selectedFile);
                }

                SoundMan.PlaySound(LotaSound.TitleAccept);
            }

            return Task.CompletedTask;
        }

        protected override void UserSelectedFile(string name)
        {
            inPrompt = true;

            SoundMan.PlaySound(LotaSound.TitleErasePrompt);
            instruction.Location = new Point(9, instruction.Location.Y - 1);

            instruction.Clear();
            instruction.WriteLine("Erase " + name + "?", XleColor.Yellow);
            instruction.WriteLine("Choose: yes  no", XleColor.Yellow);

            cursor = new TextWindow();
            cursor.Write("`", XleColor.Yellow);
            cursor.Location = new Point(19, instruction.Location.Y + 2);

            Windows.Add(cursor);
            selectedFile = name;
        }
    }
}
