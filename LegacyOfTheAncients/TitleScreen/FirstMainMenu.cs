using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.TitleScreen
{
	class FirstMainMenu : MainMenuBase
	{
		public FirstMainMenu()
		{
			Colors.BorderColor = XleColor.Purple;
			Colors.FrameColor = XleColor.Blue;
			Colors.FrameHighlightColor = XleColor.White;
			Colors.BackColor = XleColor.LightBlue;
			Colors.TextColor = XleColor.White;

			MenuItems.Add("Play a game");
			MenuItems.Add("Some simple instructions");
			MenuItems.Add("Scenes from legacy");
			MenuItems.Add("Quit to desktop");

			Instruction.SetColor(XleColor.Blue);
		}

		
		protected override void ExecuteMenuItem(int item)
		{
			if (item == 0)
			{
				NewState = new SecondMainMenu();
			}
			else if (item == 3)
			{
				Wait(500);
				throw new MainWindowClosedException();
			}
		}

	}
}
