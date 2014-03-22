using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class DungeonExtender : MapExtender, IDungeonExtender
	{
		public new Dungeon TheMap { get { return (Dungeon)base.TheMap; } }

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

			scheme.FrameColor = XleColor.DarkGray;
			scheme.FrameHighlightColor = XleColor.Green;

			scheme.MapAreaWidth = 23;
		}


		public virtual int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			return 0;
		}


		public virtual bool ShowDirection(Player player)
		{
			return true;
		}


		public virtual void CheckSounds(GameState state)
		{
		}


		public virtual DungeonMonster GetMonsterToSpawn(GameState state)
		{
			return null;
		}


		public virtual bool RollToHitMonster(GameState state, DungeonMonster monster)
		{
			return true;
		}


		public virtual int RollDamageToMonster(GameState state, DungeonMonster monster)
		{
			return 9999;
		}

		int count = 0;
		public virtual bool RollToHitPlayer(GameState state, DungeonMonster monster)
		{
			count++;
			return count % 2 == 1;
		}


		public virtual int RollDamageToPlayer(GameState state, DungeonMonster monster)
		{
			return 4;
		}


		public virtual bool SpawnMonsters(GameState state)
		{
			return true;
		}


		public virtual void UpdateMonsters(GameState state, ref bool handled)
		{
		}


		public virtual void PrintExamineMonsterMessage(DungeonMonster foundMonster, ref bool handled)
		{
		}


		public virtual void PlayerSpeak(GameState state, ref bool handled)
		{
		}


		public virtual bool PrintLevelDuringXamine
		{
			get { return true; }
		}
	}
}
