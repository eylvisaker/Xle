using AgateLib;

using Xle.Maps.Museums;
using Xle.Services.Commands.Implementation;

namespace Xle.Ancients.MapExtenders.Museum.Commands
{
    [Transient("MuseumRob")]
    public class MuseumRob : Rob
    {
        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Museum.ExhibitAt(Museum.PlayerLookingAt) != null)
            {
                PrintExhibitStopsActionMessage();
            }
            else
            {
                TextArea.PrintLine("There is nothing to rob.");
            }
        }

        protected virtual void PrintExhibitStopsActionMessage()
        {
            TextArea.PrintLine("The display case");
            TextArea.PrintLine("force field stops you.");
        }
    }
}
