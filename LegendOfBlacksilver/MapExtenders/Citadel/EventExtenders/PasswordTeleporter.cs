using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.XleEventTypes.Extenders;

namespace Xle.LoB.MapExtenders.Citadel.EventExtenders
{
    public class PasswordTeleporter : ChangeMapTeleporter
    {
        protected override bool OnStepOnImpl(ref bool cancel)
        {
            return false;
        }

        public override bool Speak()
        {
            if (Story.CitadelPassword)
            {
                return ExecuteTeleportation();
            }

            return base.Speak();
        }
    }
}
