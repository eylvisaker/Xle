using AgateLib;
using Microsoft.Xna.Framework;
using Xle.Ancients;

namespace ERY.Xle.LotA.TitleScreen
{
    [Transient, InjectProperties]
    public class LoadGame : FileMenu
    {
        private readonly IGamePersistance gamePersistance;

        public LoadGame(IGamePersistance gamePersistance) : base(gamePersistance)
        {
            Colors.FrameColor = XleColor.LightGray;
            Colors.FrameHighlightColor = XleColor.Yellow;
            Colors.BackColor = XleColor.Brown;
            Colors.BorderColor = XleColor.Red;

            Title = " Restart a game ";

            var instruction = new TextWindow();

            instruction.Location = new Point(3, 21);
            instruction.WriteLine("(Select by joystick or number keys)", XleColor.Yellow);

            Windows.Add(instruction);

            var prompt = new TextWindow();

            prompt.Location = new Point(9, 5);
            prompt.WriteLine("Restart which character?");

            Windows.Add(prompt);
            this.gamePersistance = gamePersistance;
        }


        protected override void UserSelectedFile(string file)
        {
            ThePlayer = gamePersistance.LoadPlayer(file);
        }

        protected override void UserSelectedCancel()
        {
            NewState = Factory.CreateSecondMainMenu();
        }
    }
}
