using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [ServiceName("MuseumXamine")]
    public class MuseumXamine : Xamine
    {
        MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

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
