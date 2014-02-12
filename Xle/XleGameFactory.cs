using AgateLib.DisplayLib;
using ERY.Xle.XleMapTypes;
using ERY.Xle.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public abstract class XleGameFactory
	{
		public abstract string GameTitle { get; }

		public abstract void LoadSurfaces();

		public abstract IXleTitleScreen CreateTitleScreen();

		// Surfaces
		public FontSurface Font { get; protected set; }					// returns the handle to the font resource
		public Surface Character { get; protected set; }		// returns the handle to the character resource
		public Surface Monsters { get; protected set; }				// returns the handle to the monsters resource

		public Surface MuseumBackdrop { get; protected set; }
		public Surface MuseumWall { get; protected set; }			// stores the pointer to the wall texture
		public Surface MuseumSidePassage { get; protected set; }
		public Surface MuseumDoor { get; protected set; }
		public Surface MuseumExhibitFrame { get; protected set; }
		public Surface MuseumExhibit{ get; protected set; }
		public Surface MuseumExhibitStatic { get; protected set; }
		public Surface MuseumExtras { get; protected set; }
		public Surface MuseumCloseup { get; protected set; }

		public Surface DungeonBlueBackdrop { get; protected set; }
		public Surface DungeonBlueWall { get; protected set; }
		public Surface DungeonBlueSidePassage { get; protected set; }
		public Surface DungeonBlueExtras { get; protected set; }


		public virtual IDungeonExtender CreateMapExtender(Dungeon theMap)
		{
			return new NullDungeonExtender();
		}

		public virtual ICastleExtender CreateMapExtender(Castle castle)
		{
			return new NullCastleExtender();
		}
	}
}
