using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
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
