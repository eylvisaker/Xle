
using AgateLib;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes;
using Xle.Maps.XleMapTypes.MuseumDisplays;
using Xle.Services.Commands.Implementation;

namespace Xle.Maps.Museums.Commands
{
    [Transient("MuseumFight")]
    public class MuseumFight : Fight
    {
        private Museum Map { get { return (Museum)GameState.Map; } }

        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        private Point PlayerLookingAt { get { return Museum.PlayerLookingAt; } }

        private Exhibit ExhibitAt(Point location)
        {
            return Museum.ExhibitAt(location);
        }

        private Task PrintExhibitStopsActionMessage() => Museum.PrintExhibitStopsActionMessage();

        public override async Task Execute()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            var lookingAt = PlayerLookingAt;

            if (ExhibitAt(lookingAt) != null)
            {
                await PrintExhibitStopsActionMessage();
            }
            else if (Map[lookingAt] == MuseumTiles.Door)
            {
                SoundMan.PlaySound(LotaSound.PlayerHit);

                await TextArea.PrintLine("The door does not budge.");
            }
            else
                await TextArea.PrintLine("There is nothing to fight.");
        }
    }
}
