using AgateLib;
using System.Threading.Tasks;

namespace Xle.Services.Commands.Implementation
{
    [Transient]
    public class Pass : Command
    {
        public override async Task Execute()
        {
            await TextArea.PrintLine();
        }
    }
}
