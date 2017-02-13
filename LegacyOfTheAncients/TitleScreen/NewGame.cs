using AgateLib.Mathematics.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.TitleScreen
{
    public class NewGame : TitleState
	{
		string enteredName = "";
		TextWindow upperWindow = new TextWindow();
		TextWindow lowerWindow = new TextWindow();
		TextWindow entryWindow = new TextWindow();

		public NewGame()
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
		
		public override void KeyDown(KeyCode keyCode, string keyString)
		{
			if ((keyCode >= KeyCode.A && keyCode <= KeyCode.Z) || keyCode == KeyCode.Space ||
				(keyCode >= KeyCode.D0 && keyCode <= KeyCode.D9))
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
			else if (keyCode == KeyCode.BackSpace || keyCode == KeyCode.Delete)
			{
				if (enteredName.Length > 0)
				{
					enteredName = enteredName.Substring(0, enteredName.Length - 1);

					SoundMan.PlaySound(LotaSound.TitleKeypress);
				}
				else
					SoundMan.PlaySound(LotaSound.Invalid);
			}
			else if (keyCode == KeyCode.Escape || keyCode == KeyCode.F1)
			{
				SoundMan.PlaySound(LotaSound.TitleAccept);
				NewState = Factory.CreateSecondMainMenu();
			}
			else if (keyCode == KeyCode.Enter && enteredName.Length > 0)
			{
				if (System.IO.File.Exists(@"Saved\" + enteredName + ".chr"))
				{
					SoundMan.PlaySound(LotaSound.Medium);

					lowerWindow.Clear();
					lowerWindow.Location = new Point(4, 16);

					lowerWindow.Text = enteredName + " has already begun.";

					Wait(2000);

					ResetLowerWindow();
				}
				else
				{
					lowerWindow.Clear();
					lowerWindow.Location = new Point(4, 16);

					lowerWindow.Text = enteredName + "'s adventures begin";

					SoundMan.PlaySoundSync(LotaSound.VeryGood);

					NewState = Factory.CreateIntroduction(enteredName);
				}
			}

			ResetEntryWindow();
		}


		protected override void DrawFrame()
		{
			base.DrawFrame();

			Renderer.DrawFrameLine(11 * 16, 10 * 16, 1, 18 * 16 - 4, Colors.FrameColor);  // top
			Renderer.DrawFrameLine(11 * 16, 12 * 16, 1, 18 * 16 - 4, Colors.FrameColor);  // bottom
			Renderer.DrawFrameLine(11 * 16, 10 * 16, 0, 3 * 16 - 4, Colors.FrameColor);   // left
			Renderer.DrawFrameLine(28 * 16, 10 * 16, 0, 3 * 16 - 4, Colors.FrameColor);   // right
		}
		protected override void DrawFrameHighlight()
		{
			base.DrawFrameHighlight();

			Renderer.DrawInnerFrameHighlight(11 * 16, 10 * 16, 1, 18 * 16 - 4, XleColor.White);
			Renderer.DrawInnerFrameHighlight(11 * 16, 12 * 16, 1, 18 * 16 - 2, XleColor.White);
			Renderer.DrawInnerFrameHighlight(11 * 16, 10 * 16, 0, 3 * 16 - 4, XleColor.White);
			Renderer.DrawInnerFrameHighlight(28 * 16, 10 * 16, 0, 3 * 16 - 4, XleColor.White);
		}
	}
}
