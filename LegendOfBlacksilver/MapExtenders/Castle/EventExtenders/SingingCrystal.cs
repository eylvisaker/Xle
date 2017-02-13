using AgateLib.Mathematics.Geometry;
using ERY.Xle.Maps;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class SingingCrystal : LobEvent
    {
        public override bool Use(int item)
        {
            if (item != (int)LobItem.SingingCrystal)
                return false;

            Rectangle area = new Rectangle(Player.X - 2, Player.Y - 3, 6, 8);

            RemoveRockSlide(area);

            if (area.Right >= TheEvent.Rectangle.Right - 3)
            {
                SoundMan.PlaySound(LotaSound.VeryBad);
                Player.Items[LobItem.SingingCrystal] = 0;

                TextArea.PrintLine("Your singing crystal melts.");

                TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryBad), XleColor.Yellow, XleColor.Red, 250);

                Story.ClearedRockSlide = true;
            }

            return true;
        }

        public void RemoveRockSlide(Rectangle area)
        {
            var group = Map.TileSet.TileGroups.FirstOrDefault(x => x.GroupType == Maps.GroupType.Special1);

            var replacementTile = 6;

            for (int j = area.Top; j < area.Bottom; j++)
            {
                for (int i = area.Left; i < area.Right; i++)
                {
                    var tile = Map[i, j];
                    if (group.Tiles.Contains(tile))
                    {
                        Map[i, j] = replacementTile;
                    }
                }
            }
        }
    }
}
