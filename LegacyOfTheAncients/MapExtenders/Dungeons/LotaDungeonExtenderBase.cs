using ERY.Xle.Maps.XleMapTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
	abstract class LotaDungeonExtenderBase : NullDungeonExtender
	{
		int mBoxesOpened = 0;

		public override void SetCommands(Commands.CommandList commands)
		{
			commands.Items.AddRange(LotaProgram.CommonLotaCommands);

			commands.Items.Add(new Commands.Climb());
			commands.Items.Add(new Commands.End());
			commands.Items.Add(new Commands.Magic());
			commands.Items.Add(new Commands.Open());
		}

		protected abstract int StrengthBoost { get; }
		protected abstract bool IsComplete(Player player);
		protected abstract void SetComplete(Player player);

		public override void OnLoad(Player player)
		{
			player.Story().BeenInDungeon = true;
		}

		public override void OnPlayerExitDungeon(Player player)
		{
			if (IsComplete(player))
				return;

			if (player.Item(20) > 0 && player.Item(16) > 0)
			{
				SetComplete(player);

				player.Attribute[Attributes.strength] += StrengthBoost;

				XleCore.TextArea.PrintLine("Strength + " + StrengthBoost.ToString());
				SoundMan.PlaySoundSync(LotaSound.VeryGood);
			}
		}

		public virtual void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
		}

		
		public override void OnBeforeOpenBox(Player player, ref bool handled)
		{
			mBoxesOpened++;

			if (mBoxesOpened == 3 && player.Items[LotaItem.Compass]  == 0)
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


		public void GetBoxColors(out AgateLib.Geometry.Color boxColor, out AgateLib.Geometry.Color innerColor, out AgateLib.Geometry.Color fontColor, out int vertLine)
		{
			boxColor = XleColor.Gray;
			innerColor = XleColor.LightGreen;
			fontColor = XleColor.Cyan;
			vertLine = 15 * 16;
		}
	}
}
