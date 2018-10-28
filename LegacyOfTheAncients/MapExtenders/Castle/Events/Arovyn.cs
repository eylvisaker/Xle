using AgateLib;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
    [Transient("Arovyn")]
    public class Arovyn : LotaEvent
    {
        public override async Task<bool> Speak()
        {
            if (Player.Attribute[Attributes.strength] <= 25)
            {
                await TooWeakMessage();
            }
            else
            {
                await GiveMark();
            }

            return true;
        }

        private async Task TooWeakMessage()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLineSlow("I would like to confide in you, but", XleColor.Yellow);
            await TextArea.PrintLineSlow("you are not strong enough to help.", XleColor.Yellow);
            await TextArea.PrintLineSlow("see me when your strength has grown.", XleColor.Yellow);

            await GameControl.WaitAsync(1500);
        }

        private async Task GiveMark()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLineSlow("My health declines.  You are my last", XleColor.Yellow);
            await TextArea.PrintSlow("hope.  Find the ", XleColor.Yellow);
            await TextArea.PrintLineSlow("guardians of the", XleColor.White);
            await TextArea.PrintSlow("scroll.  ", XleColor.White);
            await TextArea.PrintLineSlow("They are in many towns, but", XleColor.Yellow);
            await TextArea.PrintLineSlow("talk only to those with a special", XleColor.Yellow);
            await TextArea.PrintLineSlow("secret mark.", XleColor.Yellow);

            await GameControl.WaitAsync(3000);

            await TextArea.PrintLineSlow("I've now put this magic mark on your", XleColor.Cyan);
            await TextArea.PrintLineSlow("forearm.  Only guardians can see it.", XleColor.Cyan);

            await GameControl.WaitAsync(4000);

            Story.HasGuardianMark = true;
        }
    }
}
