using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	abstract class LobDungeonBase : DungeonExtender
	{
		public override void SetCommands(ICommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(new Climb());
			commands.Items.Add(new End());
			commands.Items.Add(new Magic());
			commands.Items.Add(new Open());
			commands.Items.Add(new Speak());
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

		protected abstract int MonsterGroup(int dungeonLevel);

		public override DungeonMonster GetMonsterToSpawn(GameState state)
		{
			if (XleCore.random.NextDouble() > 0.07)
				return null;

			int monsterID = XleCore.random.Next(6);

			monsterID += 6 * MonsterGroup(state.Player.DungeonLevel + 1);

			DungeonMonster monst = new DungeonMonster(
				XleCore.Data.DungeonMonsters[monsterID]);

			monst.HP = (int)
				(TheMap.MonsterHealthScale * (.7 + .6 * XleCore.random.NextDouble()) * (1 + monsterID / 20.0));

			return monst;
		}

		public override void CastSpell(GameState state, MagicSpell magic)
		{
			XleCore.TextArea.PrintLine("Cast " + magic.Name + ".", XleColor.White);

			if (magic.ID == 5)
				ExecuteKillFlash(state);
		}

		public override IEnumerable<MagicSpell> ValidMagic
		{
			get
			{
				return XleCore.Data.MagicSpells.Values;
			}
		}
	}
}
