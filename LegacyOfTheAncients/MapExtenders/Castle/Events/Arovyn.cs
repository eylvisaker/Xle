using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class Arovyn : LotaEvent
    {
        public override bool Speak(GameState state)
        {
            if (state.Player.Attribute[Attributes.strength] <= 25)
            {
                TooWeakMessage(state);
            }
            else
            {
                GiveMark(state);
            }

            return true;
        }

        private void TooWeakMessage(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLineSlow("I would like to confide in you, but", XleColor.Yellow);
            TextArea.PrintLineSlow("you are not strong enough to help.", XleColor.Yellow);
            TextArea.PrintLineSlow("see me when your strength has grown.", XleColor.Yellow);

            GameControl.Wait(1500);
        }

        private void GiveMark(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLineSlow("My health declines.  You are my last", XleColor.Yellow);
            TextArea.PrintSlow("hope.  Find the ", XleColor.Yellow);
            TextArea.PrintLineSlow("guardians of the", XleColor.White);
            TextArea.PrintSlow("scroll.  ", XleColor.White);
            TextArea.PrintLineSlow("They are in many towns, but", XleColor.Yellow);
            TextArea.PrintLineSlow("talk only to those with a special", XleColor.Yellow);
            TextArea.PrintLineSlow("secret mark.", XleColor.Yellow);

            GameControl.Wait(3000);

            TextArea.PrintLineSlow("I've now put this magic mark on your", XleColor.Cyan);
            TextArea.PrintLineSlow("forearm.  Only guardians can see it.", XleColor.Cyan);

            GameControl.Wait(4000);

            Story.HasGuardianMark = true;
        }
    }
}
