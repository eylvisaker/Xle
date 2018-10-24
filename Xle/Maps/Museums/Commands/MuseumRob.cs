using AgateLib;
using System.Threading.Tasks;
using Xle.Maps.Museums;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients.MapExtenders.Museum.Commands
{
    [Transient("MuseumRob")]
    public class MuseumRob : Rob
    {
        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        public override async Task Execute()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (Museum.ExhibitAt(Museum.PlayerLookingAt) != null)
            {
                await PrintExhibitStopsActionMessage();
            }
            else
            {
                await TextArea.PrintLine("There is nothing to rob.");
            }
        }

        protected virtual async Task PrintExhibitStopsActionMessage()
        {
            await TextArea.PrintLine("The display case");
            await TextArea.PrintLine("force field stops you.");
        }
    }
}
