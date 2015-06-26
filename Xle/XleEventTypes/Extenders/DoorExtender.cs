using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Data;

namespace ERY.Xle.XleEventTypes.Extenders
{
    public class DoorExtender : EventExtender
    {
        public XleData Data { get; set; }

        public new Door TheEvent { get { return (Door)base.TheEvent; } }
        
        public virtual bool ItemUnlocksDoor(int item)
        {
            return item == TheEvent.RequiredItem;
        }

        public virtual void PrintUnlockText(int item, ref bool handled)
        {
            TextArea.PrintLine("Unlock door.");
        }

        public virtual void PrintUnlockFailureText(int item, ref bool handled)
        {
            TextArea.PrintLine();
            GameControl.Wait(300 + 200 * Player.Gamespeed);
            TextArea.PrintLine("It doesn't fit this door.");
        }

        public override bool Use(int item)
        {
            if (Data.ItemList.IsKey(item) == false)
                return false;

            bool itemUnlocksDoor = ItemUnlocksDoor(item);

            if (itemUnlocksDoor)
            {
                UnlockDoor(item);
            }
            else
            {
                bool unused = false;
                PrintUnlockFailureText(item, ref unused);
            }

            return true;
        }

        protected virtual void UnlockDoor(int item)
        {
            bool handled = false;

            PrintUnlockText(item, ref handled);

            PlayRemoveSound();
            RemoveDoor();
        }

        public virtual void PlayRemoveSound()
        {
            SoundMan.PlaySound(LotaSound.UnlockDoor);
            GameControl.Wait(250);
        }
        public virtual void RemoveDoor()
        {
            for (int j = TheEvent.Rectangle.Y; j < TheEvent.Rectangle.Bottom; j++)
            {
                for (int i = TheEvent.Rectangle.X; i < TheEvent.Rectangle.Right; i++)
                {
                    Map[i, j] = TheEvent.ReplacementTile;
                }
            }

        }
    }
}
