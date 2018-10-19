using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;

namespace Xle.Maps.Towns
{
    [ServiceName("TownLeave")]
    public class TownLeave : Leave
    {
        public TownLeave(string promptText, bool confirmPrompt = true)
            : base(promptText, confirmPrompt)
        {
        }

        public IXleGameControl GameControl { get; set; }

        public override void Execute()
        {
            if (!ConfirmLeave())
                return;
            
            if (GameState.Map.Guards.IsAngry)
            {
                TextArea.PrintLine("Walk out yourself.");
                GameControl.Wait(200);
            }
            else
            {
                GameState.MapExtender.LeaveMap();
            }
        }
    }
}
