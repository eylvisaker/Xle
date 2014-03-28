using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.SecondArea
{
	class Compendium : EventExtender
	{
		private FortressFinal fortressFinal;

		public Compendium(FortressFinal fortressFinal)
		{
			this.fortressFinal = fortressFinal;
		}

		public override void Use(GameState state, int item, ref bool handled)
		{
			if (item != (int)LotaItem.GuardJewel)
				return;
			if (this.fortressFinal.CompendiumAttacking == false)
				return;

			//SoundMan.PlaySound("SonicMagic");
			XleCore.Wait(2000);

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("The attack stops.");

			handled = true;
			this.fortressFinal.CompendiumAttacking = false;
		}

		public override void Take(GameState state, ref bool handled)
		{
			handled = true;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (fortressFinal.CompendiumAttacking)
			{
				XleCore.TextArea.PrintLine("You can't hold it.");
			}
			else
			{
				XleCore.TextArea.PrintLine("You grab the compendium.");
				SoundMan.PlaySound(LotaSound.VeryGood);
				XleCore.Wait(500);

				XleCore.TextArea.FlashLinesWhile(new CountdownTimer(2500).StillRunning, 
					XleColor.Yellow, XleColor.Cyan, 50);

				TheEvent.Enabled = false;

				CloseExit(state);

				fortressFinal.CreateWarlord(state);
			}
		}

		private void CloseExit(GameState state)
		{
			for(int i = 12; i < 15; i++)
			{
				state.Map[i, 36] = 11;
			}
		}
	}
}
