using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.TitleScreen
{
	public class EraseGame : FileMenu
	{
		bool inPrompt;
		TextWindow instruction;
		TextWindow cursor;
		string selectedFile;
		int selection = 0;

		public EraseGame()
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
		}

		protected override void UserSelectedCancel()
		{
		    NewState = Factory.CreateSecondMainMenu();
		}

		public override void KeyDown(KeyCode keyCode, string keyString)
		{
			if (inPrompt == false)
			{
				base.KeyDown(keyCode, keyString);
				return;
			}

			if (keyCode == KeyCode.Y)
			{
				selection = 0;
				keyCode = KeyCode.Enter;
			}
			else if (keyCode == KeyCode.N)
			{
				selection = 1;
				keyCode = KeyCode.Enter;
			}
			else if (keyCode == KeyCode.Right)
				selection = 1;
			else if (keyCode == KeyCode.Left)
				selection = 0;

			cursor.Location = new Point(19 + 4 * selection, cursor.Location.Y);

			if (keyCode == KeyCode.Enter)
			{
			    NewState = Factory.CreateSecondMainMenu();

				if (selection == 0)
				{
					File.Delete(selectedFile);
				}

				SoundMan.PlaySound(LotaSound.TitleAccept);
			}

		}
		protected override void UserSelectedFile(string file)
		{
			inPrompt = true;

			SoundMan.PlaySound(LotaSound.TitleErasePrompt);
			instruction.Location = new Point(9, instruction.Location.Y-1);

			instruction.Clear();
			instruction.WriteLine("Erase " + Path.GetFileNameWithoutExtension(file) + "?", XleColor.Yellow);
			instruction.WriteLine("Choose: yes  no", XleColor.Yellow);

			cursor = new TextWindow();
			cursor.Write("`", XleColor.Yellow);
			cursor.Location = new Point(19, instruction.Location.Y+2);

			Windows.Add(cursor);
			selectedFile = file;
		}
	}
}
