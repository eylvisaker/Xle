using AgateLib;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [Transient("MuseumXamine")]
    public class MuseumXamine : Xamine
    {
        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (InteractWithDisplay())
                return;

            TextArea.PrintLine("You are in an ancient museum.");
        }

        private bool InteractWithDisplay()
        {
            return Museum.InteractWithDisplay();
        }
    }
}
