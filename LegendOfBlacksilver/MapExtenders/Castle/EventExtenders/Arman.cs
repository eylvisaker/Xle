using AgateLib;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    [Transient("Arman")]
    public class Arman : LobEvent
    {
        private bool spokeThisTime = false;

        public override async Task<bool> Speak()
        {
            await TextArea.PrintLine();

            if (Story.DefeatedOrcs == false && spokeThisTime == false && Player.Items[LobItem.LifeElixir] <= 1)
            {
                spokeThisTime = true;
                Player.Items[LobItem.LifeElixir] = 2;

                SoundMan.PlaySound(LotaSound.Good);

                await TextArea.PrintLineSlow("\nWelcome traveler, I'm arman, the");
                await TextArea.PrintLineSlow("apprentice to the great wizard");
                await TextArea.PrintLineSlow("Seravol.  He's up to his neck with");
                await TextArea.PrintLineSlow("these orcs, farther along.");
                await TextArea.PrintLineSlow();

                await GameControl.WaitForKey();

                await TextArea.PrintLineSlow("I know no attack spells, so i'd be");
                await TextArea.PrintLineSlow("most thankful if you could wipe out");
                await TextArea.PrintLineSlow("these orcs.  I also suggest that you");
                await TextArea.PrintLineSlow("gather all the orc loot you can.");
                await TextArea.PrintLineSlow();

                await GameControl.WaitForKey();

                SoundMan.PlaySound(LotaSound.VeryGood);
                TextArea.Clear(true);
                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLineSlow("I can reward you with life elixir.");
                await TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood), XleColor.White, XleColor.Yellow, 125);

                await GameControl.WaitForKey();

                await TextArea.PrintLine();
                await TextArea.PrintLine();
                await TextArea.PrintLineSlow("In case you missed it, there's a ");
                await TextArea.PrintLineSlow("chest with an archive gem in it.");
                await TextArea.PrintLineSlow("it's between here and the rockslide.");
                await TextArea.PrintLineSlow();
            }
            else
            {
                await TextArea.PrintLine("\nI'm busy.  Go away now.");
                await TextArea.PrintLine();
            }

            await GameControl.WaitForKey();
            return true;
        }
    }
}
