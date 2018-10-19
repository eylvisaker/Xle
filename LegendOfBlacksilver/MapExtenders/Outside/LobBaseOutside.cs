using Xle.LoB.MapExtenders.Outside.Events;
using Xle.Maps.Outdoors;
using Xle.Services;
using Xle.Services.Commands;
using Xle.XleEventTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Outside
{
    public class LobBaseOutside : OutsideExtender
    {

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LobProgram.CommonLobCommands);

            commands.Items.Add(CommandFactory.Disembark());
            commands.Items.Add(CommandFactory.End());
            commands.Items.Add(CommandFactory.Fight("OutsideFight"));
            commands.Items.Add(CommandFactory.Magic("LobOutsideMagic"));
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Speak("OutsideSpeak"));
            commands.Items.Add(CommandFactory.Use("OutsideUse"));
            commands.Items.Add(CommandFactory.Xamine("OutsideXamine"));

        }
        public override int StepSize
        {
            get { return 2; }
        }

        public override void AfterPlayerStep()
        {
            if (Player.X % 2 == 1) Player.X--;
            if (Player.Y % 2 == 1) Player.Y--;

            base.AfterPlayerStep();
        }
    }
}
