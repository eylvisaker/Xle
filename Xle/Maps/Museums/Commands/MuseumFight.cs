
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Maps.XleMapTypes.MuseumDisplays;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using Microsoft.Xna.Framework;

namespace ERY.Xle.Maps.Museums.Commands
{
    [ServiceName("MuseumFight")]
    public class MuseumFight : Fight
    {
        private Museum Map { get { return (Museum)GameState.Map; } }

        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        private Point PlayerLookingAt { get { return Museum.PlayerLookingAt; } }

        private Exhibit ExhibitAt(Point location)
        {
            return Museum.ExhibitAt(location);
        }

        private void PrintExhibitStopsActionMessage()
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
