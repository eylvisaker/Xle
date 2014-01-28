using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.LoB.TitleScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB
{
	public class LobFactory : XleGameFactory
	{
		public override string GameTitle
		{
			get
			{
				return "Legend of Blacksilver";
			}
		}

		public override void LoadSurfaces()
		{
			Font = FontSurface.BitmapMonospace("font.png", new Size(16, 16));
			Font.StringTransformer = StringTransformer.ToUpper;

			Character = new Surface("character.png");
			
		}

		public override IXleTitleScreen CreateTitleScreen()
		{
			return new LobTitleScreen();
		}
	}
}
