using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
	public class CastleDoor : DoorExtender
	{
        
		public override void PrintUnlockFailureText(GameState state, int item, ref bool handled)
		{
			TextArea.PrintLine("This key does nothing here.");
			handled = true;
		}

		public override void PrintUnlockText(GameState state, int item, ref bool handled)
		{
			handled = true;
			TextArea.PrintLine("Unlock door."); 
		}
	}
}
