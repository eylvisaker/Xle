﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Towns.Stores
{
    public class EagleHollowHealer : StoreHealer
    {
        protected override void AfterSpeak()
        {
            if (Story.HasGuardianMark == false)
                return;

            if (Story.FoundGuardianLeader == false)
            {
                TextArea.PrintLine("Welcome to our secret society!", XleColor.Yellow);
                TextArea.PrintLine();

                SoundMan.PlaySoundSync(LotaSound.VeryGood);

                TextArea.PrintLineSlow("Four jewels dungeon text goes here.");

                Player.Items[LotaItem.RubyCoin] += 1;
                Story.FoundGuardianLeader = true;
            }
            else
            {
                TextArea.PrintLineSlow("You must get all four jewels.");
            }
        }
    }
}
