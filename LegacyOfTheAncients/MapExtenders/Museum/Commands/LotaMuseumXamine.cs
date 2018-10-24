using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.Museums;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients.MapExtenders.Museum.Commands
{
    [Transient("LotaMuseumXamine")]
    public class LotaMuseumXamine : Xamine
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
