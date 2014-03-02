using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
	class CastleDoor : DoorExtender
	{
		public override void PrintUnlockFailureText(GameState state, int item, ref bool handled)
		{
			XleCore.TextArea.PrintLine("This key does nothing here.");
			handled = true;
		}

		public override void PrintUnlockText(GameState state, int item, ref bool handled)
		{
			handled = true;
			XleCore.TextArea.PrintLine("Unlock door."); 
		}
	}
}
