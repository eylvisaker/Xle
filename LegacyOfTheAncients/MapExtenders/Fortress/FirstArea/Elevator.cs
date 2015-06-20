using ERY.Xle.Services.Implementation;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
    public class Elevator : EventExtender
    {
        public override bool StepOn(GameState state)
        {
            int ystart = Player.Y;

            while (Player.X < TheEvent.Rectangle.Right)
            {
                AdvancePlayer();

                if (Player.X == 25)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Player.X = 11;
                        Player.Y = 63;

                        do
                        {
                            AdvancePlayer();

                        } while (Player.X != 98);
                    }

                    Player.Y = 34;
                    Player.X = 25;
                }
            }

            return true;
        }

        private void AdvancePlayer()
        {
            GameControl.Wait(125);
            Player.X++;
        }
    }
}
