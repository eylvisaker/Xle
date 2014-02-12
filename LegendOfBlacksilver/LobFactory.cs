using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.LoB.MapExtenders;
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

		public override XleMapTypes.Extenders.IDungeonExtender CreateMapExtender(XleMapTypes.Dungeon theMap)
		{
			return base.CreateMapExtender(theMap);
		}
		public override XleMapTypes.Extenders.ICastleExtender CreateMapExtender(XleMapTypes.Castle castle)
		{
			if (castle.ExtenderName.ToLowerInvariant() == "citadel1")
				return new CitadelGround();
			if (castle.ExtenderName.ToLowerInvariant() == "citadel2")
				return new CitadelUpper();

			return base.CreateMapExtender(castle);
		}
	}
}
