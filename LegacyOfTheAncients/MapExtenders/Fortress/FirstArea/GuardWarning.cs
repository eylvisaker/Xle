using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
    public class GuardWarning : EventExtender
    {
        public override bool StepOn(GameState state)
        {
            TheEvent.Enabled = false;

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("The guards eye you warily", XleColor.Yellow);

            SoundMan.PlaySoundSync(LotaSound.VeryBad);

            return true;
        }
    }
}
