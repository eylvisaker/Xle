using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.TitleScreen
{
    public class LoadGame : FileMenu
	{
		public LoadGame()
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
		}


		protected override void UserSelectedFile(string file)
		{
			ThePlayer = Player.LoadPlayer(file);
		}

		protected override void UserSelectedCancel()
		{
			NewState = Factory.CreateSecondMainMenu();
		}
	}
}
