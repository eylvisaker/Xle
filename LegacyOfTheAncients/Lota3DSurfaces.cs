using AgateLib.DisplayLib;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA
{
	public static class Lota3DSurfaces
	{
		public static Map3DSurfaces Museum = new Map3DSurfaces();
		public static Map3DSurfaces DungeonBlue = new Map3DSurfaces();
		public static Map3DSurfaces DungeonRed = new Map3DSurfaces();


		internal static void LoadSurfaces()
		{
			Museum.Backdrop = new Surface("Museum/MuseumBackdrop.png");
			Museum.Wall = new Surface("Museum/MuseumWall.png");
			Museum.Door = new Surface("Museum/MuseumDoor.png");
			Museum.SidePassages = new Surface("Museum/MuseumSidePassage.png");
			Museum.MuseumExhibitFrame = new Surface("Museum/ExhibitFrame.png");
			Museum.MuseumExhibitStatic = new Surface("Museum/ExhibitStatic.png");
			Museum.MuseumExhibitCloseup = new Surface("Museum/MuseumCloseup.png");
			Museum.Extras = new Surface("Museum/MuseumExtras.png");

			DungeonBlue.Backdrop = new Surface("Dungeon/Blue/DungeonBackdrop.png");
			DungeonBlue.SidePassages = new Surface("Dungeon/Blue/DungeonSidePassage.png");
			DungeonBlue.Wall = new Surface("Dungeon/Blue/DungeonWall.png");
			DungeonBlue.Extras = new Surface("Dungeon/Blue/DungeonExtras.png");

			DungeonRed = DungeonBlue;
		}
	}
}
