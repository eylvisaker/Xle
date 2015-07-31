using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Castles;
using ERY.Xle.Services;
using ERY.Xle.Services.Game;

namespace ERY.Xle.LoB.MapExtenders.Castle.Commands
{
    [ServiceName("DurekCastleSpeak")]
    public class DurekCastleSpeak : CastleSpeak
    {
        public IXleGameControl GameControl { get; set; }

        protected override void SpeakToGuard()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Player.Items[LobItem.FalconFeather] > 0)
            {
                TextArea.PrintLine("I see you have the feather,");
                TextArea.PrintLine("why not use it?");
                GameControl.Wait(1500);
            }
            else
            {
                TextArea.PrintLine("I should not converse, sir.");
            }
        }
    }
}
