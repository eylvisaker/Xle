using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.TitleScreen
{
    public class SecondMainMenu : MainMenuScreen
	{
		public SecondMainMenu()
		{
			Colors.FrameColor = XleColor.Brown;
			Colors.FrameHighlightColor = XleColor.Yellow;
			Colors.BorderColor = XleColor.DarkGray;
			Colors.BackColor = XleColor.Orange;
			Colors.TextColor = XleColor.White;

			MenuItems.Add("Return to first menu");
			MenuItems.Add("Start a new game");
			MenuItems.Add("Restart a game");
			MenuItems.Add("Erase a character");

			Instruction.SetColor(XleColor.Yellow);
			Copyright.SetColor(XleColor.Yellow);
		}

		protected override void ExecuteMenuItem(int item)
		{
			if (item == 0)
				NewState = Factory.CreateFirstMainMenu();
			else if (item == 1)
				NewState = Factory.CreateNewGame();
			else if (item == 2)
				NewState = Factory.CreateLoadGame();
			else if (item == 3)
				NewState = Factory.CreateEraseGame();
		}
	}
}
