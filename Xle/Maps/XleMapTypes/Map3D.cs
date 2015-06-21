using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Services.Implementation;

namespace ERY.Xle.Maps.XleMapTypes
{
	public abstract class Map3D : XleMap
	{
		public new Map3DExtender Extender { get { return (Map3DExtender)base.Extender; } }

		enum SidePassageType
		{
			Standard,
			Parallel,
			Wall,
		}

		protected enum ExtraType
		{
			None,
			Chest,
			Box,
			GoUp,
			GoDown,
			Needle,
			Slime,
			TripWire,
			DisplayCaseLeft,
			DisplayCaseRight,
			TorchLeft,
			TorchRight,
			DoorLeft,
			DoorRight,
		}

        readonly Size imageSize = new Size(368, 272);

		public override int WaitTimeAfterStep
		{
			get
			{
				return XleCore.GameState.GameSpeed.DungeonStepTime;
			}
		}

		protected bool IsPassable(int value)
		{
			if (value >= 0x10 && value <= 0x3f)
				return true;
			else
				return false;
		}
		protected bool IsMapSpaceBlocked(int xx, int yy)
		{
			if (this[xx, yy] >= 0x40)
				return true;
			else if ((this[xx, yy] & 0xf0) == 0x00)
				return true;

			return false;
		}

		public override bool AutoDrawPlayer
		{
			get { return false; }
		}
	}
}
