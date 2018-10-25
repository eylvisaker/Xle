using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    public class GuardWarning : EventExtender
    {
        public override async Task<bool> StepOn()
        {
            Enabled = false;

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("The guards eye you warily", XleColor.Yellow);

            await SoundMan.PlaySoundWait(LotaSound.VeryBad);

            return true;
        }
    }
}
