﻿using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Dungeons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	public abstract class LobDungeon : DungeonExtender
    {
        protected LobStory Story { get { return GameState.Story(); } }
        
		public override void SetCommands(ICommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			commands.Items.Add(CommandFactory.Climb("DungeonClimb"));
			commands.Items.Add(CommandFactory.End());
			commands.Items.Add(CommandFactory.Magic());
			commands.Items.Add(CommandFactory.Open("DungeonOpen"));
			commands.Items.Add(CommandFactory.Speak());
		}

		public override int GetTreasure(int dungeonLevel, int chestID)
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

		public override DungeonMonster GetMonsterToSpawn()
		{
			if (Random.NextDouble() > 0.07)
				return null;

			int monsterID = Random.Next(6);

			monsterID += 6 * MonsterGroup(Player.DungeonLevel + 1);

			DungeonMonster monst = new DungeonMonster(
				Data.DungeonMonsters[monsterID]);

			monst.HP = (int)
				(TheMap.MonsterHealthScale * (.7 + .6 * Random.NextDouble()) * (1 + monsterID / 20.0));

			return monst;
		}

		public override void CastSpell(MagicSpell magic)
		{
			TextArea.PrintLine("Cast " + magic.Name + ".", XleColor.White);

			if (magic.ID == 5)
				ExecuteKillFlash();
		}

		public override IEnumerable<MagicSpell> ValidMagic
		{
			get
			{
				return Data.MagicSpells.Values;
			}
		}
	}
}
