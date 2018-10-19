using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    public class GuardWarning : EventExtender
    {
        public override bool StepOn()
        {
            Enabled = false;

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("The guards eye you warily", XleColor.Yellow);

            SoundMan.PlaySoundSync(LotaSound.VeryBad);

            return true;
        }
    }
}
