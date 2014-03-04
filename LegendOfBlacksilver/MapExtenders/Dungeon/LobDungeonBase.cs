﻿using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class LobDungeonBase : NullDungeonExtender
	{
		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Commands.Climb());
			commands.Items.Add(new Commands.End());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Open());
			commands.Items.Add(new Commands.Speak());
		}

		public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			if (chestID == 1)
				return (int)LobItem.SilverCoin;

			return 0;
		}
				
		public override string TrapName(int val)
		{
			switch (val)
			{
				case 0x11: return "ceiling hole";
				case 0x12: return "floor hole";
				case 0x13: return "poison dart";
				case 0x14: return "tendril colony";
				case 0x15: return "trip bar";
				case 0x16: return "gas vent";
				default: return base.TrapName(val);
			}
		}

	}
}