using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    public class Elevator : EventExtender
    {
        public override async Task<bool> StepOn()
        {
            int ystart = Player.Y;

            while (Player.X < TheEvent.Rectangle.Right)
            {
                await AdvancePlayer();

                if (Player.X == 25)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Player.X = 11;
                        Player.Y = 63;

                        do
                        {
                            await AdvancePlayer();

                        } while (Player.X != 98);
                    }

                    Player.Y = 34;
                    Player.X = 25;
                }
            }

            return true;
        }

        private async Task AdvancePlayer()
        {
            await GameControl.WaitAsync(125);
            Player.X++;
        }
    }
}
