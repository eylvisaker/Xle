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

	}
}
