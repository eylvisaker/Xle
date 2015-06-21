using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class PasswordTeleporter : ChangeMapTeleporter
    {
        protected override bool OnStepOnImpl(GameState state, ref bool cancel)
        {
            return false;
        }

        public override bool Speak(GameState state)
        {
            if (Story.CitadelPassword)
            {
                return ExecuteTeleportation();
            }

            return base.Speak(GameState);
        }
    }
}
