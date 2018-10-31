using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    public class DaisMessage : EventExtender
    {
        bool givenMessage = false;

        public override void BeforeStepOn()
        {
            if (givenMessage)
                return;
            if (Player.Items[LobItem.FalconFeather] == 0)
                return;

            givenMessage = true;

            TextArea.Clear(true);
            TextArea.PrintLine();
            TextArea.PrintLine("   You see the prince on a dais!");

            SoundMan.PlaySound(LotaSound.VeryGood);

            TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
                XleColor.Yellow, XleColor.Cyan, 80, 1);

            TextArea.Clear();
        }
    }
}
