using AgateLib;
using System.Threading.Tasks;
using Xle.Commands.Implementation;

namespace Xle.Blacksilver.MapExtenders.Archives.Commands
{
    [Transient("ArchiveXamine")]
    public class ArchiveXamine : Xamine
    {
        public override async Task Execute()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("You are in ancient archives.");
        }
    }
}
