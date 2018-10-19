using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Outside.Events
{
    public class Drawbridge : ChangeMap
    {
        protected override bool OnStepOnImpl(ref bool cancel)
        {
            TextArea.PrintLine();

            if (Player.Items[LobItem.RopeAndPulley] == 0)
            {
                TextArea.PrintLine("You're not equipped");
                TextArea.PrintLine("to storm the citadel.");
                SoundMan.PlaySound(LotaSound.Bad);
            }
            else
            {
                TextArea.PrintLine("The drawbridge is up.");
                TextArea.PrintLine("You may wish to lower it.");
                SoundMan.PlaySound(LotaSound.Question);
            }

            GameControl.Wait(1000);

            return true;
        }

        public override bool Use(int item)
        {
            if (item == (int)LobItem.RopeAndPulley)
            {
                GameControl.Wait(1000);
                ExecuteMapChange();

                return true;
            }
            else
                return false;
        }
    }
}
