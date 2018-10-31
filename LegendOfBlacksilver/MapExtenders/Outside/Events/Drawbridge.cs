using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.LoB.MapExtenders.Outside.Events
{
    public class Drawbridge : ChangeMap
    {
        protected override async Task<bool> OnStepOnImpl()
        {
            await TextArea.PrintLine();

            if (Player.Items[LobItem.RopeAndPulley] == 0)
            {
                await TextArea.PrintLine("You're not equipped");
                await TextArea.PrintLine("to storm the citadel.");
                SoundMan.PlaySound(LotaSound.Bad);
            }
            else
            {
                await TextArea.PrintLine("The drawbridge is up.");
                await TextArea.PrintLine("You may wish to lower it.");
                SoundMan.PlaySound(LotaSound.Question);
            }

            await GameControl.WaitAsync(1000);

            return true;
        }

        public override async Task<bool> Use(int item)
        {
            if (item == (int)LobItem.RopeAndPulley)
            {
                await GameControl.WaitAsync(1000);
                await ExecuteMapChange();

                return true;
            }
            else
                return false;
        }
    }
}
