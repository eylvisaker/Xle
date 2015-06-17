using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
	class Vault : StoreVault
	{
		protected override bool RobImpl(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			if (Robbed)
			{
				XleCore.TextArea.PrintLine("The mint is empty.");
				return true;
			}

			int bags = (int)(state.Player.VaultGold / 99.0 + 1);

			XleCore.TextArea.PrintLine("You find " + bags.ToString() + " bags of gold!");
			SoundMan.PlaySoundSync(LotaSound.VeryGood);

			state.Player.Gold += state.Player.VaultGold;
			state.Player.VaultGold = (int)(state.Player.VaultGold * 0.8);

			Robbed = true;

			return true;
		}
	}
}
