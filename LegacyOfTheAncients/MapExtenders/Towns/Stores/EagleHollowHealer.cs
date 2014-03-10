using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
	class EagleHollowHealer : Healer
	{
		protected override void AfterSpeak(GameState state)
		{
			if (Lota.Story.HasGuardianMark == false)
				return;

			if (Lota.Story.FoundGuardianLeader == false)
			{
				XleCore.TextArea.PrintLine("Welcome to our secret society!", XleColor.Yellow);
				XleCore.TextArea.PrintLine();

				SoundMan.PlaySoundSync(LotaSound.VeryGood);

				XleCore.TextArea.PrintLineSlow("Four jewels dungeon text goes here.");

				state.Player.Items[LotaItem.RubyCoin] += 1;
				Lota.Story.FoundGuardianLeader = true;
			}
			else
			{
				XleCore.TextArea.PrintLineSlow("You must get all four jewels.");
			}
		}
	}
}
