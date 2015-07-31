using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Museums;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.LotA.MapExtenders.Museum.Commands
{
    public class MuseumRob : Rob
    {
        MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

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
