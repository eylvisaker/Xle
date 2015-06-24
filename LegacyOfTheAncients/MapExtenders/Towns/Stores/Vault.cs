using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
	public class Vault : StoreVault
	{
		protected override bool RobImpl(GameState state)
		{
			TextArea.PrintLine();
			TextArea.PrintLine();

			if (Robbed)
			{
				TextArea.PrintLine("The mint is empty.");
				return true;
			}

			int bags = (int)(Player.VaultGold / 99.0 + 1);

			TextArea.PrintLine("You find " + bags.ToString() + " bags of gold!");
			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			Player.Gold += Player.VaultGold;
			Player.VaultGold = (int)(Player.VaultGold * 0.8);

			Robbed = true;

			return true;
		}
	}
}
