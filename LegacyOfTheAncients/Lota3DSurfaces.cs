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
			Museum.Walls = new Surface("Museum/walls.png");
			Museum.Torches = new Surface("Museum/torches.png");
			Museum.ExhibitOpen = new Surface("Museum/Exhibits/exopen.png");
			Museum.ExhibitClosed = new Surface("Museum/Exhibits/exopen.png");

			MuseumDark.Walls = new Surface("MuseumDark/walls.png");

			DungeonBlue.Traps = new Surface("Dungeon/PiratesLair/traps.png");
			DungeonBlue.Walls = new Surface("Dungeon/PiratesLair/walls.png");

			DungeonBrown.Traps = new Surface("Dungeon/Armak/traps.png");
			DungeonBrown.Walls = new Surface("Dungeon/Armak/walls.png");
		}
	}
}
