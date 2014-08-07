using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using AgateLib.InputLib.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.TitleScreen
{
	abstract class MainMenuBase : TitleState
	{
		int titleMenu;
		protected List<string> MenuItems = new List<string>();
		List<TextWindow> MenuItemWindows = new List<TextWindow>();

		static Surface titleHeader1;
		static Surface titleHeader2;

		protected TextWindow Instruction { get; private set; }
		protected TextWindow Copyright { get; private set; }


		public MainMenuBase()
		{
			if (titleHeader1 == null)
			{
				titleHeader1 = new Surface("TitleHeader1.png");
				titleHeader2 = new Surface("TitleHeader2.png");
			}

			Instruction = new TextWindow();
			Instruction.Location = new Point(2, 18);
			Instruction.Write("(Pick option by keyboard or joystick)");

			Copyright = new TextWindow();
			Copyright.Location = new Point(2, 22);
			Copyright.Write("Copyright 1987 - Quest Software, Inc.");

			Windows.Add(Instruction);
			Windows.Add(Copyright);
		}

		public override void KeyDown(KeyCode keyCode, string keyString)
		{
			if (keyCode == KeyCode.Down)
			{
				titleMenu++;

				if (titleMenu > 3)
					titleMenu = 3;

				SoundMan.PlaySound(LotaSound.TitleCursor);
			}
			else if (keyCode == KeyCode.Up)
			{
				titleMenu--;

				if (titleMenu < 0)
					titleMenu = 0;

				SoundMan.PlaySound(LotaSound.TitleCursor);
			}
			else if (keyCode >= KeyCode.D1 && keyCode <= KeyCode.D4)
			{
				titleMenu = keyCode - KeyCode.D1;

				keyCode = KeyCode.Return;
			}

			if (keyCode == KeyCode.Return)
			{
				SkipWait = true; 
				SoundMan.PlaySound(LotaSound.TitleAccept);

				Keyboard.ReleaseAllKeys();

				ExecuteMenuItem(titleMenu);
			}
		}

		protected abstract void ExecuteMenuItem(int item);

		public override void Update()
		{
			if (MenuItemWindows.Count == 0)
			{
				for (int i = 0; i < 4; i++)
				{
					MenuItemWindows.Add(new TextWindow());
					MenuItemWindows[i].Location = new Point(7, 9 + i * 2);
				}

				Windows.AddRange(MenuItemWindows);
			}

			for (int i = 0; i < 4; i++)
			{
				var wind = MenuItemWindows[i];

				wind.Text = (i+1).ToString() + ".  " + MenuItems[i];
			}
		}
		protected override void DrawBackgrounds()
		{
			base.DrawBackgrounds();

			DrawTitleHeader(Colors.FrameColor, Colors.FrameHighlightColor);
		}
		protected override void DrawFrame()
		{
			base.DrawFrame();
			XleCore.Renderer.DrawFrameLine(0, 20 * 16, 1, XleCore.myWindowWidth, Colors.FrameColor);
		}
		protected override void DrawFrameHighlight()
		{
			base.DrawFrameHighlight();
			XleCore.Renderer.DrawInnerFrameHighlight(0, 20 * 16, 1, XleCore.myWindowWidth, Colors.FrameHighlightColor);
		}

		protected override void DrawWindows()
		{
			base.DrawWindows();

			Point pt = new Point(5, 9 + titleMenu * 2);

			XleCore.Renderer.WriteText(pt.X * 16, pt.Y * 16, "`");
		}

		private void DrawTitleHeader(Color frameColor, Color lineColor)
		{
			titleHeader1.InterpolationHint = InterpolationMode.Fastest;
			titleHeader2.InterpolationHint = InterpolationMode.Fastest;

			titleHeader1.Color = frameColor;
			titleHeader2.Color = lineColor;

			titleHeader1.Draw();
			titleHeader2.Draw();
		}

	}
}
