using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.LotA.TitleScreen;
using ERY.Xle.XleMapTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	public class LotaFactory : XleGameFactory
	{
		public override string GameTitle
		{
			get
			{
				return "Legacy of the Ancients";
			}
		}

		public override void LoadSurfaces()
		{
			Font = FontSurface.BitmapMonospace("font.png", new Size(16, 16));
			Font.StringTransformer = StringTransformer.ToUpper;

			Character = new Surface("character.png");
			Monsters = new Surface("OverworldMonsters.png");

			MuseumBackdrop = new Surface("MuseumBackdrop.png");
			MuseumWall = new Surface("MuseumWall.png");
			MuseumDoor = new Surface("MuseumDoor.png");
			MuseumSidePassage = new Surface("MuseumSidePassage.png");
			MuseumExhibitFrame = new Surface("ExhibitFrame.png");
			MuseumExhibitStatic = new Surface("ExhibitStatic.png");
			MuseumCloseup = new Surface("MuseumCloseup.png");
			MuseumExtras = new Surface("MuseumExtras.png");

			DungeonBlueBackdrop = new Surface("DungeonBackdropBlue.png");
			DungeonBlueSidePassage = new Surface("DungeonSidePassageBlue.png");
			DungeonBlueWall = new Surface("DungeonWallBlue.png");
			DungeonBlueExtras = new Surface("DungeonExtrasBlue.png");

			foreach (var exinfo in XleCore.ExhibitInfo.Values)
			{
				exinfo.LoadImage();
			}
		}

		public override IXleTitleScreen CreateTitleScreen()
		{
			return new LotaTitleScreen();
		}

		public override IDungeonExtender CreateMapExtender(Dungeon theMap) 
		{
			switch (theMap.MapID)
			{
				case 71: return new PirateExtender();
				case 72: return new ArmakExtender();
				case 73: return new FourJewelsExtender();
			}

			return base.CreateMapExtender(theMap);
		}
	}
}
