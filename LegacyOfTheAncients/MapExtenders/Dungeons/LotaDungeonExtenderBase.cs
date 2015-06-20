using AgateLib;
using AgateLib.Platform;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	abstract class LotaDungeonExtenderBase : DungeonExtender
	{
		public LotaDungeonExtenderBase()
		{
			FillDrips();
			ResetDripTime();
		}

        public ICommandFactory CommandFactory { get; set; }

		public override IEnumerable<MagicSpell> ValidMagic
		{
			get
			{
				// everything but seek spell
				return from m in XleCore.Data.MagicSpells
					   where m.Key != 6
					   select m.Value;
			}
		}
		public override bool PrintLevelDuringXamine
		{
			get { return XleCore.Options.EnhancedGameplay; }
		}

		public override void SetCommands(ICommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(CommandFactory.Climb());
			commands.Items.Add(CommandFactory.End());
			commands.Items.Add(CommandFactory.Magic());
			commands.Items.Add(CommandFactory.Open());
		}

		protected abstract int StrengthBoost { get; }
		protected abstract bool IsCompleted { get; set; }

		public override void OnLoad(GameState state)
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

			int monsterID = XleCore.random.Next(6);

			if (state.Player.DungeonLevel >= 4)
				monsterID += 6;

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

			var weapon = state.Player.CurrentWeapon;

			double vd = weapon.ID + 1 + weapon.Quality / 2.8;

			damage *= vd + 4;

			if (state.Player.WeaponEnchantTurnsRemaining > 0)
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
			var armor = state.Player.CurrentArmor;
			double vc = armor.ID + armor.Quality / 3.5;

			double damage = 10 * TheMap.MonsterDamageScale / (vc + 3) * (state.Player.DungeonLevel + 7);

			return (int)((XleCore.random.NextDouble() + 0.5) * damage);
		}

		public override bool RollSpellFizzle(GameState state, MagicSpell magic)
		{
			return XleCore.random.NextDouble() * 45 > state.Player.Attribute[Attributes.intelligence] || XleCore.random.NextDouble() < 0.05;
		}
		public override int RollSpellDamage(GameState state, MagicSpell magic, int distance)
		{
			var dam =  (1.0 / distance + .3) * 45 * (1 + XleCore.random.NextDouble()) * ((magic.ID == 2) ? 2 : 1);

			return (int)dam;
		}

		public override void CastSpell(GameState state, MagicSpell magic)
		{
			XleCore.TextArea.PrintLine("Cast " + magic.Name + ".", XleColor.White);

			if (magic.ID == 3)
				CastBefuddle(state, magic);
			if (magic.ID == 4)
				CastPsychoStrength(state, magic);
			if (magic.ID == 5)
				CastKillFlash(state, magic);
		}

		private void CastKillFlash(GameState state, MagicSpell magic)
		{
			ExecuteKillFlash(state);
		}

		private void CastPsychoStrength(GameState state, MagicSpell magic)
		{
			throw new NotImplementedException();
		}

		private void CastBefuddle(GameState state, MagicSpell magic)
		{
			if (state.Player.HP >= 250 && XleCore.random.NextDouble() < 0.07)
			{
				//Backfire!!!
				Lota.Story.BackfiredBefuddleTurns = XleCore.random.Next(5, 10);
			}
			else
			{
				if (Lota.Story.BefuddleTurns > 0)
					Lota.Story.BefuddleTurns /= 2;

				Lota.Story.BefuddleTurns += XleCore.random.Next(25, 35);

				var monst = MonsterInFrontOfPlayer(state.Player);

				if (monst != null)
				{
					XleCore.TextArea.PrintLine("The " + monst.Name + " looks confused.", XleColor.White);
				}
			}
		}

		public override void UpdateMonsters(GameState state)
		{
			if (Lota.Story.BefuddleTurns > 0)
			{
				Lota.Story.BefuddleTurns--;
			}
			else
				base.UpdateMonsters(state);
		}
	}
}
