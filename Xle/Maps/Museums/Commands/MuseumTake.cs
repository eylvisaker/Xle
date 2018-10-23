using AgateLib;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [Transient("MuseumTake")]
    public class MuseumTake : Take
    {
        public override void Execute()
        {
            TextArea.PrintLine("\n\nThere is nothing to take.");
        }
    }
}
