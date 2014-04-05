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
		public static Map3DSurfaces MuseumDark = new Map3DSurfaces();
		public static Map3DSurfaces DungeonBlue = new Map3DSurfaces();
		public static Map3DSurfaces DungeonBrown = new Map3DSurfaces();

		internal static void LoadSurfaces()
		{
			Museum.Door = new Surface("Museum/MuseumDoor.png");
			Museum.MuseumExhibitFrame = new Surface("Museum/ExhibitFrame.png");
			Museum.MuseumExhibitStatic = new Surface("Museum/ExhibitStatic.png");
			Museum.MuseumExhibitCloseup = new Surface("Museum/MuseumCloseup.png");
			Museum.Extras = new Surface("Museum/MuseumExtras.png");
			Museum.Walls = new Surface("Museum/walls.png");
			Museum.Torches = new Surface("Museum/torches.png");

			MuseumDark.Walls = new Surface("MuseumDark/walls.png");

			DungeonBlue.Extras = new Surface("Dungeon/Blue/DungeonExtras.png");
			DungeonBlue.Walls = new Surface("Dungeon/PiratesLair/walls.png");

			DungeonBrown.Extras = new Surface("Dungeon/Blue/DungeonExtras.png");
			DungeonBrown.Walls = new Surface("Dungeon/Armak/walls.png");
		}
	}
}
