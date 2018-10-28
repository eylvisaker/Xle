using AgateLib;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Maps.Museums;

namespace Xle.Ancients.MapExtenders.Museum.Commands
{
    [Transient("LotaMuseumUse")]
    public class LotaMuseumUse : LotaUse
    {
        private LotaStory Story { get { return GameState.Story(); } }

        private XleMap Map { get { return GameState.Map; } }

        private MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }

        private bool IsFacingDoor { get { return Museum.IsFacingDoor; } }

        protected override async Task<bool> UseWithMap(int item)
        {
            // twist gold armband
            if (item == (int)LotaItem.GoldArmband)
            {
                await UseGoldArmband();
                return true;
            }

            return false;
        }

        private async Task UseGoldArmband()
        {
            bool facingDoor = IsFacingDoor;

            if (facingDoor)
            {
                await GameControl.WaitAsync(1000);

                foreach (var entry in Map.EntryPoints)
                {
                    if (entry.Location == Player.Location)
                    {
                        Story.MuseumEntryPoint = Map.EntryPoints.IndexOf(entry);
                    }
                }

                await Museum.LeaveMap();
            }
            else
            {
                await TextArea.PrintLine("The gold armband hums softly.");
            }
        }

    }
}
