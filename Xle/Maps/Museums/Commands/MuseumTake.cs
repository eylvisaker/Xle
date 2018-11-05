using AgateLib;
using System.Threading.Tasks;
using Xle.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [Transient("MuseumTake")]
    public class MuseumTake : Take
    {
        public override async Task Execute()
        {
            await TextArea.PrintLine("\n\nThere is nothing to take.");
        }
    }
}
