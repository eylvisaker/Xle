using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    [Transient("DaisMessage")]
    public class DaisMessage : EventExtender
    {
        bool givenMessage = false;

        public override async Task BeforeStepOn()
        {
            if (givenMessage)
                return;
            if (Player.Items[LobItem.FalconFeather] == 0)
                return;

            givenMessage = true;

            TextArea.Clear(true);
            await TextArea.PrintLine();
            await TextArea.PrintLine("   You see the prince on a dais!");

            SoundMan.PlaySound(LotaSound.VeryGood);

            await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
                XleColor.Yellow, XleColor.Cyan, 80, 1);

            TextArea.Clear();
        }
    }
}
