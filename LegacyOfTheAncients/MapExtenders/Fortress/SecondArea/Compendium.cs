using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.SecondArea
{
	public class Compendium : EventExtender
	{
		FortressFinal fortressFinal { get { return (FortressFinal)GameState.MapExtender; } }

		public override bool Use(int item)
		{
			if (item != (int)LotaItem.GuardJewel)
				return false;
			if (this.fortressFinal.CompendiumAttacking == false)
				return false;

			//SoundMan.PlaySound("SonicMagic");
			GameControl.Wait(2000);

			TextArea.PrintLine();
			TextArea.PrintLine("The attack stops.");

			fortressFinal.CompendiumAttacking = false;

			return true;
		}

		public override bool Take()
		{
			TextArea.PrintLine();
			TextArea.PrintLine();

			if (fortressFinal.CompendiumAttacking)
			{
				TextArea.PrintLine("You can't hold it.");
			}
			else
			{
				TextArea.PrintLine("You grab the compendium.");
				SoundMan.PlaySound(LotaSound.VeryGood);
				GameControl.Wait(500);

				TextArea.FlashLinesWhile(new CountdownTimer(2500).StillRunning, 
					XleColor.Yellow, XleColor.Cyan, 50);

				TheEvent.Enabled = false;

				CloseExit(GameState);

				fortressFinal.CreateWarlord();
			}

			return true;
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
