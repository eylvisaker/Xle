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
			Museum.Walls = new Surface("Images/Museum/walls.png");
			Museum.Torches = new Surface("Images/Museum/torches.png");
			Museum.ExhibitOpen = new Surface("Images/Museum/Exhibits/exopen.png");
			Museum.ExhibitClosed = new Surface("Images/Museum/Exhibits/exopen.png");

			MuseumDark.Walls = new Surface("Images/MuseumDark/walls.png");

			DungeonBlue.Traps = new Surface("Images/Dungeon/PiratesLair/traps.png");
			DungeonBlue.Walls = new Surface("Images/Dungeon/PiratesLair/walls.png");

			DungeonBrown.Traps = new Surface("Images/Dungeon/Armak/traps.png");
			DungeonBrown.Walls = new Surface("Images/Dungeon/Armak/walls.png");
		}
	}
}
