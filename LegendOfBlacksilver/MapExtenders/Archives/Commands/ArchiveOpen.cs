using AgateLib;
using System.Threading.Tasks;
using Xle.Commands.Implementation;

namespace Xle.Blacksilver.MapExtenders.Archives.Commands
{
    [Transient("ArchiveOpen")]
    public class ArchiveOpen : Open
    {
        private LobArchives Archives { get { return (LobArchives)GameState.MapExtender; } }

        private bool IsFacingDoor
        {
            get { return Archives.IsFacingDoor; }
        }

        private Task LeaveMap()
        {
            return Archives.LeaveMap();
        }

        private Task<bool> InteractWithDisplay()
        {
            return Archives.InteractWithDisplay();
        }

        public override async Task Execute()
        {
            if (IsFacingDoor)
            {
                await TextArea.PrintLine(" door");
                await TextArea.PrintLine();

                await LeaveMap();
                return;
            }

            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (await InteractWithDisplay())
                return;

            await TextArea.PrintLine("Nothing to open.");
        }
    }
}
