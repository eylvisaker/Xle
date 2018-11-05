using AgateLib;
using System.Threading.Tasks;
using Xle.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [Transient("MuseumXamine")]
    public class MuseumXamine : Xamine
    {
        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        public override async Task Execute()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (await InteractWithDisplay())
                return;

            await TextArea.PrintLine("You are in an ancient museum.");
        }

        private async Task<bool> InteractWithDisplay()
        {
            return await Museum.InteractWithDisplay();
        }
    }
}
