﻿using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreFrontExtender : StoreExtender
	{
		[Obsolete]
		protected string[] theWindow = new string[20];
		[Obsolete]
		protected Color[][] theWindowColor = new Color[20][];

		ColorScheme mColorScheme = new ColorScheme();

		public List<TextWindow> Windows { get; private set; }

		public StoreFrontExtender()
		{
			Windows = new List<TextWindow>();

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
			Windows.Clear();
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

		[Obsolete]
		protected virtual void SetColorScheme(ColorScheme cs)
		{ }

		protected internal void RedrawStore()
		{
			Display.BeginFrame();

			DrawStore();

			Display.EndFrame();
			XleCore.KeepAlive();
		}

		protected void DrawStore()
		{
			string tempString;
			var player = XleCore.GameState.Player;

			var renderer = XleCore.Renderer;

			SetColorScheme(mColorScheme);
			XleCore.SetProjectionAndBackColors(mColorScheme);

			// Draw the borders
			renderer.DrawFrame(mColorScheme.FrameColor);
			renderer.DrawFrameLine(0, 288, 1, 640, mColorScheme.FrameColor);

			renderer.DrawFrameHighlight(mColorScheme.FrameHighlightColor);
			renderer.DrawInnerFrameHighlight(0, 288, 1, 640, mColorScheme.FrameHighlightColor);

			// Draw the title
			if (string.IsNullOrEmpty(Title))
			{
				DrawTitle(theWindow[0]);
			}
			else
			{
				DrawTitle(Title);
			}

			if (Windows.Count == 0)
			{
				for (int i = 1; i < 18; i++)
				{
					if (string.IsNullOrEmpty(theWindow[i]))
						continue;

					renderer.WriteText((LeftOffset + 1) * 16, i * 16, theWindow[i], theWindowColor[i]);
				}
			}
			else
			{
				foreach (var window in Windows)
				{
					window.Draw();
				}
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

		private void DrawTitle(string title)
		{
			Display.FillRect(320 - (title.Length + 2) / 2 * 16, 0,
						 (title.Length + 2) * 16, 16, mColorScheme.BackColor);

			XleCore.Renderer.WriteText(320 - (title.Length / 2) * 16, 0, title, mColorScheme.TitleColor);
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

		public string Title { get; set; }

		public override bool Speak(GameState state)
		{
			try
			{
				XleCore.Renderer.ReplacementDrawMethod = DrawStore;

				return SpeakImpl(state);
			}
			finally
			{
				XleCore.Renderer.ReplacementDrawMethod = null;
			}
		}
		protected virtual bool SpeakImpl(GameState state)
		{
			return StoreNotImplementedMessage();
		}

		public new Store TheEvent { get { return (Store)base.TheEvent; } }

	}
}
