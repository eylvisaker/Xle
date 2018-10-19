using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.LoB.MapExtenders.Archives.Commands
{
    [ServiceName("ArchiveOpen")]
    public class ArchiveOpen : Open
    {
        LobArchives Archives { get { return (LobArchives)GameState.MapExtender; } }

        bool IsFacingDoor
        {
            get { return Archives.IsFacingDoor; } 
        }
        void LeaveMap()
        {
            Archives.LeaveMap();
        }

        bool InteractWithDisplay()
        {
            return Archives.InteractWithDisplay();
        }

        public override void Execute()
        {
            if (IsFacingDoor)
            {
                TextArea.PrintLine(" door");
                TextArea.PrintLine();

                LeaveMap();
                return;
            }

            TextArea.PrintLine();
            TextArea.PrintLine();

            if (InteractWithDisplay())
                return;

            TextArea.PrintLine("Nothing to open.");
        }
    }
}
