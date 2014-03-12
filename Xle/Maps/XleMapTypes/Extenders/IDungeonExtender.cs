using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public interface IDungeonExtender : IMapExtender
	{
		void OnPlayerExitDungeon(Player player);

		void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled, ref bool clearBox);

		void OnBeforeOpenBox(Player player, ref bool handled);

		void OnLoad(Player player);

		string TrapName(int val);

		int GetTreasure(GameState gameState, int CurrentLevel, int val);

		bool ShowDirection(Player player);

		void CheckSounds(GameState gameState);

		DungeonMonster GetMonsterToSpawn(GameState state);

		bool RollToHitMonster(GameState state, DungeonMonster monster);

		int RollDamageToMonster(GameState state, DungeonMonster monster);

		bool RollToHitPlayer(GameState state, DungeonMonster monster);

		int RollDamageToPlayer(GameState state, DungeonMonster monster);
	}
}
