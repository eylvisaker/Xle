using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores
{
	public abstract class StoreFront : Store
	{
		protected string[] theWindow = new string[20];
		protected Color[][] theWindowColor = new Color[20][];

		ColorScheme mColorScheme = new ColorScheme();

		public StoreFront()
		{
			LeftOffset = 2;

			for (int i = 0; i < theWindowColor.Length; i++)
			{
				for (int j = 0; j < 40; j++)
				{
					theWindowColor[i] = new Color[40];
				}
			}

			ClearWindow();

			mColorScheme.BackColor = XleColor.Green;
		}

		protected void ClearWindow()
		{
			for (int i = 0; i < theWindow.Length; i++)
			{
				theWindow[i] = string.Empty;

				for (int j = 0; j < theWindowColor[i].Length; j++)
					theWindowColor[i][j] = XleColor.White;
			}
		}

		protected void SetColor(int rowNumber, Color color)
		{
			SetColor(rowNumber, 0, 40, color);
		}
		protected void SetColor(int rowNumber, int start, int length, Color color)
		{
			for (int i = 0; i < length; i++)
				theWindowColor[rowNumber][start + i] = color;
		}

		protected int LeftOffset { get; set; }

		protected abstract void SetColorScheme(ColorScheme cs);

		protected void RedrawStore()
		{
			SetColorScheme(mColorScheme);

			Display.BeginFrame();
			XleCore.SetProjectionAndBackColors(mColorScheme);

			DrawStore();

			Display.EndFrame();
			Core.KeepAlive();
		}
		protected void DrawStore()
		{
			string tempString;

			var renderer = XleCore.Renderer;

			// Draw the borders
			renderer.DrawFrame(mColorScheme.FrameColor);
			renderer.DrawFrameLine(0, 288, 1, 640, mColorScheme.FrameColor);

			renderer.DrawFrameHighlight(mColorScheme.FrameHighlightColor);
			renderer.DrawInnerFrameHighlight(0, 288, 1, 640, mColorScheme.FrameHighlightColor);

			// Draw the title
			Display.FillRect(320 - (theWindow[0].Length + 2) / 2 * 16, 0,
				(theWindow[0].Length + 2) * 16, 16, mColorScheme.BackColor);

			renderer.WriteText(320 - (theWindow[0].Length / 2) * 16, 0, theWindow[0], mColorScheme.TitleColor);

			for (int i = 1; i < 18; i++)
			{
				if (string.IsNullOrEmpty(theWindow[i]))
					continue;

				renderer.WriteText((LeftOffset + 1) * 16, i * 16, theWindow[i], theWindowColor[i]);
			}

			if (robbing == false)
			{
				// Draw Gold
				tempString = " Gold: ";
				tempString += player.Gold;
				tempString += " ";
			}
			else
			{
				// don't need gold if we're robbing it!
				tempString = " Robbery in progress ";
			}

			Display.FillRect(320 - (tempString.Length / 2) * 16, 18 * 16, tempString.Length * 16, 14,
				mColorScheme.BackColor);

			renderer.WriteText(320 - (tempString.Length / 2) * 16, 18 * 16, tempString, XleColor.White);

			XleCore.TextArea.Draw();

		}

		protected void StoreSound(LotaSound sound)
		{
			SoundMan.PlaySoundSync(RedrawStore, sound);
		}

		protected void Wait(int howLong)
		{
			XleCore.Wait(howLong, RedrawStore);
		}
		protected void WaitForKey(params KeyCode[] keys)
		{
			XleCore.WaitForKey(RedrawStore, keys);
		}

		protected int QuickMenu(MenuItemList menu, int spaces)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces, value);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces, value, clrInit);
		}
		protected int QuickMenu(MenuItemList menu, int spaces, int value, Color clrInit, Color clrChanged)
		{
			return XleCore.QuickMenu(RedrawStore, menu, spaces, value, clrInit, clrChanged);
		}

		protected int ChooseNumber(int max)
		{
			return XleCore.ChooseNumber(RedrawStore, max);
		}

	}
}
