using AgateLib;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums
{
    [Transient("MuseumSpeak")]
    public class MuseumSpeak : Speak
    {
        public override void Execute()
        {
            TextArea.PrintLine("\n\nThere is no reply.");
        }
    }
}
