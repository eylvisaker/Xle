using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Outside
{
	class TarmalonExtender : OutsideExtender
	{
		int banditAmbush;

		public override void OnLoad(GameState state)
		{
			base.OnLoad(state);

			Lota.Story.Invisible = false;

			XleCore.Renderer.PlayerColor = XleColor.White;

			SetBanditAmbushTime(state);
		}

		public int WaterAnimLevel
		{
			get { return MapRenderer.WaterAnimLevel; }
			set { MapRenderer.WaterAnimLevel = value; }
		}

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Disembark());
			commands.Items.Add(new Commands.End());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Speak());
		}

		public override void SetColorScheme(ColorScheme scheme)
		{
			base.SetColorScheme(scheme);

			scheme.MapAreaWidth = 23;
		}

		public override void UpdateEncounterState(GameState state, ref bool handled)
		{
			handled = BanditAmbush(state);
		}

		bool AllowBanditAmbush(GameState state)
		{
			// make sure the player has the compendium
			if (state.Player.Items[LotaItem.Compendium] == 0) return false;

			// if the player has the guard jewels we bail.
			if (state.Player.Items[LotaItem.GuardJewel] == 4) return false;

			return true;
		}

		/// <summary>
		/// Check to see if we should have bandits ambush the player. If so,
		/// sets banditAmbush variable.
		/// </summary>
		/// <param name="player"></param>
		private void SetBanditAmbushTime(GameState state)
		{
			if (AllowBanditAmbush(state) == false)
				return;

			int pastTime = (int)(state.Player.TimeDays - 100);
			if (pastTime < 0) pastTime = 0;

			int min = 40 - (int)(pastTime / 2);
			if (min < 3) min = 3;

			int max = 100 - (int)(pastTime / 5);
			if (max < 12) max = 12;

			int time = XleCore.random.Next(min, max);

			if (time > state.Player.Food - 2)
			{
				time = (int)state.Player.Food - 2;
				if (time < 0)
					time = 1;
			}

			banditAmbush = (int)(state.Player.TimeDays) + time;
		}
		private bool BanditAmbush(GameState state)
		{
			if (AllowBanditAmbush(state) == false)
				return false;

			if (banditAmbush <= 0)
				SetBanditAmbushTime(state);

			if (state.Player.TimeDays <= banditAmbush)
				return false;

			// set a random position for the appearance of the bandits.
			SetMonsterImagePosition(state.Player);

			// bandit icon is number 4.
			TheMap.displayMonst = 4;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You are ambushed by bandits!", XleColor.Cyan);

			SoundMan.PlaySound(LotaSound.Encounter);
			XleCore.Wait(500);

			int maxDamage = state.Player.HP / 15;
			int minDamage = Math.Min(5, maxDamage / 2);

			for (int i = 0; i < 8; i++)
			{
				state.Player.HP -= XleCore.random.Next(minDamage, maxDamage + 1);

				SoundMan.PlaySound(LotaSound.EnemyHit);
				XleCore.Wait(250);
			}

			XleCore.TextArea.PrintLine("You fall unconsious.", XleColor.Yellow);

			XleCore.Wait(1000);
			TheMap.displayMonst = -1;
			XleCore.Wait(3000, RedrawUnconscious);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You awake.  The compendium is gone.");
			XleCore.TextArea.PrintLine();

			state.Player.Items[LotaItem.Compendium] = 0;

			SoundMan.PlaySoundSync(LotaSound.VeryBad);

			XleCore.TextArea.PrintLine("You hear a voice...");

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLineSlow("Do not be discouraged.  It was\ninevitable.  Keep to your quest.");

			XleCore.Wait(3000);
			banditAmbush = 0;

			return true;
		}

		void RedrawUnconscious()
		{
			AgateLib.DisplayLib.Display.BeginFrame();
			AgateLib.DisplayLib.Display.Clear(XleColor.Gray);
			AgateLib.DisplayLib.Display.EndFrame();
		}

		/// <summary>
		/// Returns true if the player drowns.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		private bool CheckStormy(Player player)
		{
			int wasStormy = WaterAnimLevel;


			if (player.X < -45 || player.X > TheMap.Width + 45 ||
				player.Y < -45 || player.Y > TheMap.Height + 45)
			{
				WaterAnimLevel = 3;
			}
			else if (player.X < -30 || player.X > TheMap.Width + 30 ||
				player.Y < -30 || player.Y > TheMap.Height + 30)
			{
				WaterAnimLevel = 2;
			}
			else if (player.X < -15 || player.X > TheMap.Width + 15 ||
				player.Y < -15 || player.Y > TheMap.Height + 15)
			{
				WaterAnimLevel = 1;
			}
			else
			{
				WaterAnimLevel = 0;
			}

			if (WaterAnimLevel != wasStormy || WaterAnimLevel >= 2)
			{
				if (WaterAnimLevel == 1 && wasStormy == 0)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("You are sailing into stormy water.", XleColor.Yellow);
				}
				else if (WaterAnimLevel == 2 || WaterAnimLevel == 3)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("The water is now very rough.", XleColor.White);
					XleCore.TextArea.PrintLine("It will soon swamp your raft.", XleColor.Yellow);
				}
				else if (WaterAnimLevel == 1 && wasStormy == 2)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("You are out of immediate danger.", XleColor.Yellow);
				}
				else if (WaterAnimLevel == 0 && wasStormy == 1)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("You leave the storm behind.", XleColor.Cyan);
				}

				if (WaterAnimLevel == 3)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("Your raft sinks.", XleColor.Yellow);
					XleCore.TextArea.PrintLine();
				}

				XleCore.Wait(1000);

				if (WaterAnimLevel == 3)
				{
					player.HP = 0;
					return true;
				}

			}
			return false;
		}

		public override IEnumerable<MagicSpell> ValidMagic
		{
			get
			{
				yield return XleCore.Data.MagicSpells[1];
				yield return XleCore.Data.MagicSpells[2];
				yield return XleCore.Data.MagicSpells[6];
			}
		}

		public override void CastSpell(GameState state, MagicSpell magic)
		{
			if (magic.ID == 6)
			{
				CastSeekSpell(state);
			}
		}

		private void CastSeekSpell(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Cast seek spell.");

			if (state.Player.IsOnRaft)
			{
				XleCore.TextArea.PrintLine("The water mutes the spell.");
			}
			else if (TheMap.MapID != 1)
			{
				XleCore.TextArea.PrintLine("You're too far away.");
			}
			else
			{
				state.Player.FaceDirection = Direction.West;
				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				XleCore.ChangeMap(state.Player, 1, 0);
				EncounterState = 0;
			}
		}

	}
}
