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
	abstract class TitleState
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
		}

		protected virtual void DrawTitle()
		{
			if (string.IsNullOrEmpty(Title))
				return;

			int destx= 20 - Title.Length / 2;

			Display.FillRect(new Rectangle(destx*16, 0, Title.Length * 16, 16), XleColor.White);

			XleCore.Renderer.WriteText(destx*16, 0, Title, Colors.BackColor);
		}

		protected virtual void DrawWindows()
		{
			foreach(var wind in Windows)
			{
				wind.Draw();
			}
		}

		public string Title;

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
