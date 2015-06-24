using ERY.Xle.Services.Rendering;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LotA.MapExtenders.Fortress.FirstArea
{
    public class ArmorBox : TreasureChestExtender
    {
        public IXleRenderer Renderer { get; set; }

        public override bool Open(GameState state)
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (TheEvent.Closed)
            {
                TextArea.PrintLine("you see yellow guard");
                TextArea.PrintLine("armor in the bottom.");

                PlayOpenChestSound();
                TheEvent.SetOpenTilesOnMap(GameState.Map);

                GameControl.Wait(GameState.GameSpeed.CastleOpenChestSoundTime);
            }
            else
            {
                TextArea.PrintLine("box open already.");
            }

            return true;
        }

        public override bool Take(GameState state)
        {
            state.Map.Guards.IsAngry = false;

            Player.RenderColor = XleColor.Yellow;

            TextArea.PrintLine();
            TextArea.PrintLine();
            TextArea.PrintLine("you put on armor.");

            GameControl.Wait(1000);

            return true;
        }
    }
}
