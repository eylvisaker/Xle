using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class NullDungeonExtender : NullMapExtender, IDungeonExtender
	{
		public virtual void OnPlayerExitDungeon(Player player)
		{
		}

		public virtual void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled, ref bool clearBox)
		{
		}

		public virtual void OnBeforeOpenBox(Player player, ref bool handled)
		{
		}

		public virtual void OnLoad(Player player)
		{
		}

		public virtual string TrapName(int val)
		{
			switch (val)
			{
				case 0x11: return "ceiling hole";
				case 0x12: return "floor hole";
				case 0x13: return "poison gas vent";
				case 0x14: return "slime splotch";
				case 0x15: return "trip wire";
				case 0x16: return "gas vent";
				default: throw new ArgumentException();
			}
		}


		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.Cyan;

			scheme.FrameColor = XleColor.Gray;
			scheme.FrameHighlightColor = XleColor.LightGreen;

			scheme.VerticalLinePosition = 15 * 16;
		}


		public virtual int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			return 0;
		}


		public virtual bool ShowDirection(Player player)
		{
			return true;
		}
	}
}
