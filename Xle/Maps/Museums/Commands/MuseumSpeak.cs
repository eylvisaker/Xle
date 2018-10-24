using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums
{
    [Transient("MuseumSpeak")]
    public class MuseumSpeak : Speak
    {
        public override async Task Execute()
        {
            await TextArea.PrintLine("\n\nThere is no reply.");
        }
    }
}
