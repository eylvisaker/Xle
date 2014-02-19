using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	abstract class LotaDungeonExtenderBase : NullDungeonExtender
	{
		int mBoxesOpened = 0;

		protected abstract string CompleteVariable { get; }
		protected abstract int StrengthBoost { get; }

		public override void OnLoad(Player player)
		{
			player.Variables["BeenInDungeon"] = 1;
		}

		public override void OnPlayerExitDungeon(Player player)
		{
			if (player.Variables.ContainsKey(CompleteVariable))
				return;

			if (player.Item(20) > 0 && player.Item(16) > 0)
			{
				player.Variables[CompleteVariable] = 1;

				player.Attribute[Attributes.strength] += StrengthBoost;

				g.AddBottom("Strength + " + StrengthBoost.ToString());
				SoundMan.PlaySoundSync(LotaSound.VeryGood);
			}
		}

		public virtual void OnBeforeGiveItem(Player player, ref int treasure, ref bool handled)
		{
		}

		
		public virtual void OnBeforeOpenBox(Player player, ref bool handled)
		{
			mBoxesOpened++;

			if (mBoxesOpened == 3 && player.Item(11) == 0)
			{
				g.AddBottom("You find a compass!", XleColor.Yellow);
				player.ItemCount(11, 1);

				SoundMan.PlaySound(LotaSound.VeryGood);

				handled = true;
			}

		}


		public int GetOutsideTile(AgateLib.Geometry.Point playerPoint, int x, int y)
		{
			return 0;
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
