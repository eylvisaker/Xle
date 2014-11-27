using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders
{
	class PasswordTeleporter : ChangeMapTeleporter
	{
		protected override bool OnStepOnImpl(GameState state, ref bool cancel)
		{
			return false;
		}

		public override bool Speak(GameState state)
		{
			if (Lob.Story.CitadelPassword)
			{
				return ExecuteTeleportation(state);
			}

			return base.Speak(state);
		}
	}
}
