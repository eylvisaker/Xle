using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class Arman : LobEvent
    {
        bool spokeThisTime = false;

        public override bool Speak()
        {
            TextArea.PrintLine();

            if (Story.DefeatedOrcs == false && spokeThisTime == false && Player.Items[LobItem.LifeElixir] <= 1)
            {
                spokeThisTime = true;
                Player.Items[LobItem.LifeElixir] = 2;

                SoundMan.PlaySound(LotaSound.Good);

                TextArea.PrintLineSlow("\nWelcome traveler, I'm arman, the");
                TextArea.PrintLineSlow("apprentice to the great wizard");
                TextArea.PrintLineSlow("Seravol.  He's up to his neck with");
                TextArea.PrintLineSlow("these orcs, farther along.");
                TextArea.PrintLineSlow();

                Input.WaitForKey();

                TextArea.PrintLineSlow("I know no attack spells, so i'd be");
                TextArea.PrintLineSlow("most thankful if you could wipe out");
                TextArea.PrintLineSlow("these orcs.  I also suggest that you");
                TextArea.PrintLineSlow("gather all the orc loot you can.");
                TextArea.PrintLineSlow();

                Input.WaitForKey();

                SoundMan.PlaySound(LotaSound.VeryGood);
                TextArea.Clear(true);
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLineSlow("I can reward you with life elixir.");
                TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.White, XleColor.Yellow, 125);

                Input.WaitForKey();

                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLineSlow("In case you missed it, there's a ");
                TextArea.PrintLineSlow("chest with an archive gem in it.");
                TextArea.PrintLineSlow("it's between here and the rockslide.");
                TextArea.PrintLineSlow();
            }
            else
            {
                TextArea.PrintLine("\nI'm busy.  Go away now.");
                TextArea.PrintLine();
            }

            Input.WaitForKey();
            return true;
        }
    }
}
