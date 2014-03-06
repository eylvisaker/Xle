﻿using AgateLib.DisplayLib;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.Extenders;
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

		public abstract IXleSerializable CreateStoryData();

		// Surfaces
		public FontSurface Font { get; protected set; }					// returns the handle to the font resource
		public Surface Character { get; protected set; }		// returns the handle to the character resource
		public Surface Monsters { get; protected set; }				// returns the handle to the monsters resource

		public virtual void CheatLevel(Player player, int level)
		{
			throw new NotImplementedException();
		}

		public virtual IDungeonExtender CreateMapExtender(Dungeon theMap)
		{
			return new NullDungeonExtender();
		}

		public virtual ICastleExtender CreateMapExtender(Castle castle)
		{
			return new NullCastleExtender();
		}

		public virtual ITempleExtender CreateMapExtender(Temple town)
		{
			return new NullTempleExtender();
		}

		public virtual ITownExtender CreateMapExtender(Town town)
		{
			return new NullTownExtender();
		}

		public virtual IOutsideExtender CreateMapExtender(Outside outside)
		{
			return new NullOutsideExtender();
		}

		public virtual IMuseumExtender CreateMapExtender(Museum museum)
		{
			return new NullMuseumExtender();
		}

		public abstract Maps.Map3DSurfaces GetMap3DSurfaces(Map3D map3D);

		public virtual void SetGameSpeed(GameState state, int speed)
		{
			state.GameSpeed.CastleOpenChestTime = 500 + 200 * speed;
			state.GameSpeed.AfterSetGamespeedTime = 300 + 200 * speed;
			state.GameSpeed.CastleOpenChestSoundTime = 750;
			state.GameSpeed.DungeonOpenChestSoundTime = 500;

			state.GameSpeed.GeneralStepTime = 150;
			state.GameSpeed.OutsideStepTime = 500;
		}
	}
}
