using AgateLib;
using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	abstract class LotaDungeonExtenderBase : NullDungeonExtender
	{
		public LotaDungeonExtenderBase()
		{
			FillDrips();
			ResetDripTime();
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Climb());
			commands.Items.Add(new Commands.End());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Open());
		}

		protected abstract int StrengthBoost { get; }
		protected abstract bool IsCompleted { get; set; }

		public override void OnLoad(Player player)
		{
			Lota.Story.BeenInDungeon = true;
		}

		protected void GivePermanentStrengthBoost(Player player)
		{
			player.Attribute[Attributes.strength] += StrengthBoost;

			XleCore.TextArea.PrintLine("Strength + " + StrengthBoost.ToString());
			SoundMan.PlaySoundSync(LotaSound.VeryGood);
		}

		public virtual void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
		}

		
		public override void OnBeforeOpenBox(Player player, ref bool handled)
		{
			if (player.DungeonLevel == 0)
				return;
			if (player.Items[LotaItem.Compass] > 0)
				return;
			
			if (XleCore.random.NextDouble() < .6)
			{
				XleCore.TextArea.PrintLine("You find a compass!", XleColor.Yellow);
				player.Items[LotaItem.Compass] += 1;

				SoundMan.PlaySound(LotaSound.VeryGood);

				handled = true;
			}

		}

		public override bool ShowDirection(Player player)
		{
			// check for compass.
			return player.Items[LotaItem.Compass] > 0;
		}

		double nextSound;
		LotaSound[] drips;

		private void FillDrips()
		{
			drips = new LotaSound[2];
			drips[0] = LotaSound.Drip0;
			drips[1] = LotaSound.Drip1;
		}

		public override void CheckSounds(GameState state)
		{
			if (Timing.TotalSeconds > nextSound)
			{
				ResetDripTime();

				SoundMan.PlaySound(drips[XleCore.random.Next(drips.Length)]);
			}
		}


		private void ResetDripTime()
		{
			double time = XleCore.random.NextDouble() * 10 + 2;

			nextSound = Timing.TotalSeconds + time;
		}


		public override DungeonMonster GetMonsterToSpawn(GameState state)
		{
			if (XleCore.random.NextDouble() > 0.07) 
				return null;

			int monsterID = 0;

			DungeonMonster monst = new DungeonMonster(
				XleCore.Data.DungeonMonsters[monsterID]);

			monst.HP = (int)
				((monsterID + 15 + 15 * XleCore.random.NextDouble()) * 2.4 * TheMap.MonsterHealthScale);

			return monst;
		}

		public override bool RollToHitMonster(GameState state, DungeonMonster monster)
		{
			return XleCore.random.NextDouble() * 70 < state.Player.Attribute[Attributes.dexterity] + 30;
		}
		public override int RollDamageToMonster(GameState state, DungeonMonster monster)
		{
			double damage = state.Player.Attribute[Attributes.strength] + 30;
			damage /= 45;

			double vd = state.Player.CurrentWeaponType + 1 + state.Player.CurrentWeaponQuality / 2.8;

			damage *= vd + 4;

			if (state.Player.WeaponEnchantTurnsLeft > 0)
			{
				damage *= 2.5;
			}
			else
				damage *= 1.5;

			return (int)(damage * (0.5 + XleCore.random.NextDouble()));
		}

		public override bool RollToHitPlayer(GameState state, DungeonMonster monster)
		{
			if (XleCore.random.NextDouble() * 70 > state.Player.Attribute[Attributes.dexterity])
			{
				return true;
			}
			else
				return false;
		}

		public override int RollDamageToPlayer(GameState state, DungeonMonster monster)
		{
			double vc = state.Player.CurrentArmorType + state.Player.CurrentArmorQuality / 3.5;

			double damage = 10 * TheMap.MonsterDamageScale / (vc + 3) * (state.Player.DungeonLevel + 7);

			return (int)((XleCore.random.NextDouble() + 0.5) * damage);
		}
	}
}
