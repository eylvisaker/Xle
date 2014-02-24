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
		public static Map3DSurfaces DungeonBlue = new Map3DSurfaces();


		internal static void LoadSurfaces()
		{
			Archives.Backdrop = new Surface("Museum/MuseumBackdrop.png");
			Archives.Wall = new Surface("Museum/MuseumWall.png");
			Archives.Door = new Surface("Museum/MuseumDoor.png");
			Archives.SidePassages = new Surface("Museum/MuseumSidePassage.png");
			Archives.MuseumExhibitFrame = new Surface("Museum/ExhibitFrame.png");
			Archives.MuseumExhibitStatic = new Surface("Museum/ExhibitStatic.png");
			Archives.MuseumExhibitCloseup = new Surface("Museum/MuseumCloseup.png");
			Archives.Extras = new Surface("Museum/MuseumExtras.png");

			DungeonBlue.Backdrop = new Surface("Dungeon/Blue/DungeonBackdrop.png");
			DungeonBlue.SidePassages = new Surface("Dungeon/Blue/DungeonSidePassage.png");
			DungeonBlue.Wall = new Surface("Dungeon/Blue/DungeonWall.png");
			DungeonBlue.Extras = new Surface("Dungeon/Blue/DungeonExtras.png");
		}
	}
}
