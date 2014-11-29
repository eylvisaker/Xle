using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon
{
	class PitsOfBlackmire : LobDungeonBase
	{
		public override int GetTreasure(GameState state, int dungeonLevel, int chestID)
		{
			if (chestID == 3)
			{
				switch (dungeonLevel)
				{
					case 2: return -1;
					case 3: return (int)LobItem.RustyKey;
					case 6: return (int)LobItem.SkeletonKey;
					case 10: return (int)LobItem.Blacksilver;
				}
			}

			if (chestID == 2)
			{
				switch (dungeonLevel)
				{
					case 2:
						return (int)LobItem.BlackWand;

					default:
						return (int)LobItem.WhiteDiamond;
				}
			}

			return base.GetTreasure(state, dungeonLevel, chestID);
		}

		public override void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled, ref bool clearBox)
		{
			if (treasure == -1)
			{
				XleCore.TextArea.PrintLine("You need a key.", XleColor.Yellow);

				handled = true;
				clearBox = false;
			}
		}

		public override void PlayerUse(GameState state, int item, ref bool handled)
		{
			if (item == (int)LobItem.RustyKey)
			{
				if (state.Player.DungeonLevel + 1 == 2 && 
					state.Map[state.Player.X, state.Player.Y] == 0x33)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("A hole appears!", XleColor.White);

					state.Map[state.Player.X, state.Player.Y] = 0x12;

					SoundMan.PlaySoundSync(LotaSound.VeryGood);

					handled = true;
				}
			}
			base.PlayerUse(state, item, ref handled);
		}

		public override void PlayerMagic(GameState state)
		{
			base.PlayerMagic(state);

			if (state.Player.DungeonLevel >= 6 && Lob.Story.Illusion == false)
			{
				// turn off the display.
			}
		}
		public override bool PlayerClimb(GameState state)
		{
			var retval = base.PlayerClimb(state);

			if (state.Player.DungeonLevel == 4 && Lob.Story.RotlungContracted == false)
			{
				SoundMan.PlaySound(LotaSound.VeryBad);

				XleCore.TextArea.PrintLineSlow("You have contracted rotlung.");
				XleCore.TextArea.PrintLine("Endurance  - 10");

				Lob.Story.RotlungContracted = true;
				state.Player.Attribute[Attributes.endurance] -= 10;
			}

			if (state.Player.DungeonLevel == 6 && Lob.Story.Illusion == false)
			{
				// turn off the display.
			}

			return retval;
		}
		protected override int MonsterGroup(int dungeonLevel)
		{
			if (dungeonLevel <= 2) return 0;
			if (dungeonLevel <= 6) return 1;

			return 2;
		}

		public override Maps.Map3DSurfaces Surfaces(GameState state)
		{
			return Lob3DSurfaces.PitsOfBlackmire;
		}
	}
}
