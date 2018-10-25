using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Castle.Events
{
	public class CastleDoor : DoorExtender
	{
        
		public override async Task<bool> PrintUnlockFailureText(int item)
		{
			await TextArea.PrintLine("This key does nothing here.");
            return true;
		}

		public override async Task<bool> PrintUnlockText(int item)
		{
            await TextArea.PrintLine("Unlock door.");
            return true;
		}
	}
}
