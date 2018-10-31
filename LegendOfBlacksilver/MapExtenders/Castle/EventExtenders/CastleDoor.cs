using Xle.XleEventTypes;
using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class CastleDoor : DoorExtender
    {
        public override bool ItemUnlocksDoor(int item)
        {
            if (item == (int)LobItem.GoldKey)
                return true;
            else
                return base.ItemUnlocksDoor(item);
        }

        public override async Task<bool> PrintUnlockFailureText(int item)
        {
            await TextArea.PrintLine("It doesn't fit this door.");
            return true;
        }
    }

    public class FeatherDoor : CastleDoor
    {
        public override bool ItemUnlocksDoor(int item)
        {
            if (item == (int)LobItem.SmallKey || item == (int)LobItem.GoldKey)
                return true;
            else
                return base.ItemUnlocksDoor(item);
        }
        public override void RemoveDoor()
        {
            var rect = TheEvent.Rectangle;
            var doorEvent = (Door)TheEvent;

            for (int j = rect.Y; j < rect.Bottom; j++)
            {
                for (int i = rect.X; i < rect.Right; i++)
                {
                    Map[i, j] = doorEvent.ReplacementTile;
                }
            }

            for (int j = rect.Y + 1; j < rect.Bottom; j += 2)
            {
                int i = rect.X + 1;

                Map[i, j] = doorEvent.ReplacementTile;
            }
        }
    }
}
