using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.TitleScreen
{
	public abstract class TitleState
	{
		public TitleState()
		{
			Colors = new ColorScheme();
			Windows = new List<TextWindow>();
		}

		public abstract void KeyDown(KeyCode keyCode, string keyString);

		public bool SkipWait { get; set; }

		public TitleState NewState { get; set; }


		protected void Wait(int time)
		{
			XleCore.Wait(time);
		}

		protected ColorScheme Colors { get; set; }

		public virtual void Update()
		{
		}

		public virtual void Draw()
		{
			XleCore.SetOrthoProjection(Colors.BorderColor);
			Display.FillRect(new Rectangle(0, 0, 640, 400), Colors.BackColor);

			DrawBackgrounds();
			DrawWindows();
			DrawTitle();
			DrawPrompt();
		}

		protected virtual void DrawTitle()
		{
			if (string.IsNullOrEmpty(Title))
				return;

			DrawCenteredText(0, Title, Colors.BackColor, Colors.TextColor);
		}
		private void DrawPrompt()
		{
			if (string.IsNullOrEmpty(Prompt))
				return;

			DrawCenteredText(24, Prompt, XleColor.Yellow, Colors.BackColor);
		}

		private void DrawCenteredText(int y, string text, Color textColor, Color backColor)
		{
			int destx = 20 - text.Length / 2;

			Display.FillRect(new Rectangle(destx * 16, y*16, text.Length * 16, 16), backColor);

			XleCore.Renderer.WriteText(destx * 16, y*16, text, textColor);
		}

		protected virtual void DrawWindows()
		{
			foreach(var wind in Windows)
			{
				wind.Draw();
			}
		}

		public string Title { get; set; }
		public string Prompt { get; set; }

		protected virtual void DrawBackgrounds()
		{
			DrawFrame();
			DrawFrameHighlight();
		}

		protected virtual void DrawFrame()
		{
			XleCore.Renderer.DrawFrame(Colors.FrameColor);			
		}
		protected virtual void DrawFrameHighlight()
		{
			XleCore.Renderer.DrawFrameHighlight(Colors.FrameHighlightColor);
		}

		protected List<TextWindow> Windows { get; set; }

		public Player ThePlayer { get; protected set; }
	}
}
