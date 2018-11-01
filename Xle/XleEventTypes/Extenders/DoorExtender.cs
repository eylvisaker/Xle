using System.Threading.Tasks;
using Xle.Data;

namespace Xle.XleEventTypes.Extenders
{
    public class DoorExtender : EventExtender
    {
        public XleData Data { get; set; }

        public new Door TheEvent { get { return (Door)base.TheEvent; } }

        public virtual bool ItemUnlocksDoor(int item)
        {
            return item == TheEvent.RequiredItem;
        }

        public virtual async Task<bool> PrintUnlockText(int item)
        {
            await TextArea.PrintLine("Unlock door.");
            return false;
        }

        public virtual async Task<bool> PrintUnlockFailureText(int item)
        {
            await TextArea.PrintLine();
            await GameControl.WaitAsync(300 + 200 * Player.Gamespeed);
            await TextArea.PrintLine("It doesn't fit this door.");

            return false;
        }

        public override async Task<bool> Use(int item)
        {
            if (Data.ItemList.IsKey(item) == false)
                return false;

            bool itemUnlocksDoor = ItemUnlocksDoor(item);

            if (itemUnlocksDoor)
            {
                await UnlockDoor(item);
            }
            else
            {
                await PrintUnlockFailureText(item);
            }

            return true;
        }

        protected virtual async Task UnlockDoor(int item)
        {
            bool handled = false;

            handled |= await PrintUnlockText(item);

            await PlayRemoveSound();
            RemoveDoor();
        }

        public virtual async Task PlayRemoveSound()
        {
            SoundMan.PlaySound(LotaSound.UnlockDoor);
            await GameControl.WaitAsync(250);
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
