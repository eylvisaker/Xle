using System.Collections.Generic;
using System.Linq;

using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

namespace ERY.Xle.LotA.MapExtenders.Castle.Events
{
    public class Spiral : EventExtender
    {
        public override bool StepOn()
        {
            if (AnyBad)
            {
                SoundMan.PlaySoundSync(LotaSound.VeryBad);

                ClearSpiral();
                RemoveSpiralEvents();

                return true;
            }
            else
                return false;
        }

        protected void RemoveSpiralEvents()
        {
            foreach (XleEvent spiralEvent in SpiralEvents)
                spiralEvent.Enabled = false;
        }

        private IEnumerable<XleEvent> SpiralEvents
        {
            get
            {
                return (Map.Events.Where(x => x.ExtenderName.ToLowerInvariant().Contains("spiral"))).ToArray();
            }
        }
        protected void ClearSpiral()
        {
            foreach (var evt in SpiralEvents)
            {
                for (int y = evt.Rectangle.Y; y < evt.Rectangle.Bottom; y++)
                {
                    for (int x = evt.Rectangle.X; x < evt.Rectangle.Right; x++)
                    {
                        GameState.Map[x, y] = 2;
                    }
                }
            }
        }

        protected bool AnyBad
        {
            get
            {
                int[] vals = new int[4];
                vals[0] = Map[Player.X, Player.Y];
                vals[1] = Map[Player.X + 1, Player.Y];
                vals[2] = Map[Player.X, Player.Y + 1];
                vals[3] = Map[Player.X + 1, Player.Y + 1];

                bool anyBad = vals.Any(x => x > 68);
                return anyBad;
            }
        }
    }
}
