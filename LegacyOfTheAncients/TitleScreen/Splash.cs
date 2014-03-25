using AgateLib.DisplayLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.TitleScreen
{
	class Splash : TitleState
	{
		Surface titleScreenSurface;			// stores the image of the title screen.

		public Splash()
		{
		}

		public override void KeyDown(AgateLib.InputLib.KeyCode keyCode, string keyString)
		{
			NewState = new FirstMainMenu();
			titleScreenSurface.Dispose();
		}

		public override void Update()
		{
			if (titleScreenSurface == null)
				titleScreenSurface = new Surface("title.png");
		}

		public override void Draw()
		{
			XleCore.SetOrthoProjection(XleColor.Gray);

			titleScreenSurface.Draw();
		}
	}
}
