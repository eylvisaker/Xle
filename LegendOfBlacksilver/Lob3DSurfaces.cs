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
			Archives.ExhibitOpen = new Surface("Museum/Exhibits/exopen.png");
			Archives.ExhibitClosed = new Surface("Museum/Exhibits/exclosed.png");
			Archives.Walls = new Surface("Museum/walls.png");
			Archives.Torches = new Surface("Museum/torches.png");

			SetDungeon(IslandCaverns, "IslandCavern");
			SetDungeon(TaragasMines, "TaragasMines");
			SetDungeon(MarthbaneTunnels, "Marthbane");
			SetDungeon(PitsOfBlackmire, "Blackmire");
			SetDungeon(DeathspireChasm, "Deathspire");
		}

		private static void SetDungeon(Map3DSurfaces surfs, string name)
		{
			surfs.Walls = new Surface("Dungeon/" + name + "/walls.png");
			surfs.Traps = new Surface("Dungeon/" + name + "/traps.png");
		}
	}
}
