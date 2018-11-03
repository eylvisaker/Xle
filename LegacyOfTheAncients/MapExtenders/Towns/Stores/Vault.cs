using AgateLib;
using System.Threading.Tasks;
using Xle.XleEventTypes.Stores.Extenders;

namespace Xle.Ancients.MapExtenders.Towns.Stores
{
    [Transient("StoreVault")]
    public class Vault : StoreVault
    {
        protected override async Task<bool> RobCore()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Robbed)
            {
                await TextArea.PrintLine("The mint is empty.");
                return true;
            }

            int bags = (int)(Player.VaultGold / 99.0 + 1);

            await TextArea.PrintLine("You find " + bags.ToString() + " bags of gold!");
            await GameControl.PlaySoundWait(LotaSound.VeryGood);

            Player.Gold += Player.VaultGold;
            Player.VaultGold = (int)(Player.VaultGold * 0.8);

            Robbed = true;

            return true;
        }
    }
}
