using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Stores.Extenders
{
	public class StoreFront : StoreExtender
	{
		ColorScheme mColorScheme = new ColorScheme();

		public List<TextWindow> Windows { get; private set; }
		public new Store TheEvent { get { return (Store)base.TheEvent; } }

		public StoreFront()
		{
			Windows = new List<TextWindow>();

			ClearWindow();

			mColorScheme.BackColor = XleColor.Green;
		}

		protected void ClearWindow()
		{
			Windows.Clear();
		}

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
			DrawTitle(Title);

			foreach (var window in Windows)
			{
				window.Draw();
			}

			string goldText;
			if (robbing == false)
			{
				// Draw Gold
				goldText = " Gold: ";
				goldText += player.Gold;
				goldText += " ";
			}
			else
			{
				// don't need gold if we're robbing it!
				goldText = " Robbery in progress ";
			}

			Display.FillRect(320 - (goldText.Length / 2) * 16, 18 * 16, goldText.Length * 16, 14,
				mColorScheme.BackColor);

			renderer.WriteText(320 - (goldText.Length / 2) * 16, 18 * 16, goldText, XleColor.White);

			XleCore.TextArea.Draw();

		}

		private void DrawTitle(string title)
		{
			if (string.IsNullOrEmpty(title))
				return;

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
			if (AllowInteractionWhenLoanOverdue == false)
			{
				if (IsLoanOverdue(state))
				{
					StoreDeclinePlayer(state);
					return true;
				}
			}

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

	}
}
