using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Geometry;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Maps.Museums.Commands
{
    [ServiceName("MuseumFight")]
    public class MuseumFight : Fight
    {
        Museum Map { get { return (Museum)GameState.Map; } }
        MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        Point PlayerLookingAt { get { return Museum.PlayerLookingAt; } }

        Exhibit ExhibitAt(Point location)
        {
            return Museum.ExhibitAt(location);
        }

        void PrintExhibitStopsActionMessage()
        {
            Museum.PrintExhibitStopsActionMessage();
        }
        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            var lookingAt = PlayerLookingAt;

            if (ExhibitAt(lookingAt) != null)
            {
                PrintExhibitStopsActionMessage();
            }
            else if (Map[lookingAt] == MuseumTiles.Door)
            {
                SoundMan.PlaySound(LotaSound.PlayerHit);

                TextArea.PrintLine("The door does not budge.");
            }
            else
                TextArea.PrintLine("There is nothing to fight.");
        }
    }
}
