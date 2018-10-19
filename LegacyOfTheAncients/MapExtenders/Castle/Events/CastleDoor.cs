using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
	public class CastleDoor : DoorExtender
	{
        
		public override void PrintUnlockFailureText(int item, ref bool handled)
		{
			TextArea.PrintLine("This key does nothing here.");
			handled = true;
		}

		public override void PrintUnlockText(int item, ref bool handled)
		{
			handled = true;
			TextArea.PrintLine("Unlock door."); 
		}
	}
}
