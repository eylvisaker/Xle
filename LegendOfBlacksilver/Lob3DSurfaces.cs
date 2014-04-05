using AgateLib.DisplayLib;
using ERY.Xle.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB
{
	public static class Lob3DSurfaces
	{
		public static Map3DSurfaces Archives = new Map3DSurfaces();
		public static Map3DSurfaces IslandCaverns = new Map3DSurfaces();
		public static Map3DSurfaces TaragasMines = new Map3DSurfaces();
		public static Map3DSurfaces MarthbaneTunnels = new Map3DSurfaces();
		public static Map3DSurfaces PitsOfBlackmire = new Map3DSurfaces();
		public static Map3DSurfaces DeathspireChasm = new Map3DSurfaces();


		internal static void LoadSurfaces()
		{
			Archives.Door = new Surface("Museum/MuseumDoor.png");
			Archives.MuseumExhibitFrame = new Surface("Museum/ExhibitFrame.png");
			Archives.MuseumExhibitStatic = new Surface("Museum/ExhibitStatic.png");
			Archives.MuseumExhibitCloseup = new Surface("Museum/MuseumCloseup.png");
			Archives.Extras = new Surface("Museum/MuseumExtras.png");
			Archives.Walls = new Surface("Museum/walls.png");
			Archives.Torches = new Surface("Museum/torches.png");

			IslandCaverns.Extras = new Surface("Dungeon/Blue/DungeonExtras.png");
			IslandCaverns.Walls = new Surface("Dungeon/IslandCavern/walls.png");

			TaragasMines.Extras = IslandCaverns.Extras;
			MarthbaneTunnels.Extras = IslandCaverns.Extras;
			PitsOfBlackmire.Extras = IslandCaverns.Extras;
			DeathspireChasm.Extras = IslandCaverns.Extras;

			TaragasMines.Walls = new Surface("Dungeon/TaragasMines/walls.png");
			MarthbaneTunnels.Walls = new Surface("Dungeon/Marthbane/walls.png");
			PitsOfBlackmire.Walls = new Surface("Dungeon/Blackmire/walls.png");
			DeathspireChasm.Walls = new Surface("Dungeon/Deathspire/walls.png");
		}
	}
}
