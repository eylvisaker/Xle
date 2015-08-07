using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps;
using ERY.Xle.Maps.Museums;
using ERY.Xle.Services;

namespace ERY.Xle.LotA.MapExtenders.Museum.Commands
{
    [ServiceName("MuseumUse")]
    public class MuseumUse : LotaUse
    {
        LotaStory Story { get { return GameState.Story(); } }
        XleMap Map { get { return GameState.Map; } }
        MuseumExtender Museum { get { return (MuseumExtender)GameState.MapExtender; } }
        bool IsFacingDoor { get { return Museum.IsFacingDoor; } }

        protected override bool UseWithMap(int item)
        {
            // twist gold armband
            if (item == (int)LotaItem.GoldArmband)
            {
                UseGoldArmband();
                return true;
            }

            return false;
        }

        private void UseGoldArmband()
        {
            bool facingDoor = IsFacingDoor;

            if (facingDoor)
            {
                GameControl.Wait(1000);

                foreach (var entry in Map.EntryPoints)
                {
                    if (entry.Location == Player.Location)
                    {
                        Story.MuseumEntryPoint = Map.EntryPoints.IndexOf(entry);
                    }
                }

                Museum.LeaveMap();
            }
            else
            {
                TextArea.PrintLine("The gold armband hums softly.");
            }
        }

    }
}
