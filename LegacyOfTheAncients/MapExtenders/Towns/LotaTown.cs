using Xle.Ancients.MapExtenders.Towns.Stores;
using Xle.Maps.Towns;
using Xle.Services;
using Xle.Services.Commands;
using Xle.XleEventTypes;
using Xle.XleEventTypes.Stores;
using Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Magic = Xle.Services.Commands.Implementation.MagicCommand;

namespace Xle.Ancients.MapExtenders.Towns
{
    public class LotaTown : TownExtender
    {

        public XleOptions Options { get; set; }
        public LotaMuseumCoinSale MuseumCoinSale { get; set; }

        public override void OnLoad()
        {
            CheckLoan();

            base.OnLoad();
        }

        void CheckLoan()
        {
            if (TheMap.Events.Any(x => x is Store && x.ExtenderName == "StoreLending"))
            {
                if (Player.loan > 0 && Player.dueDate <= Player.TimeDays)
                {
                    TextArea.PrintLine("This is your friendly lender.");
                    TextArea.PrintLine("You owe me money!");
                    SoundMan.PlaySound(LotaSound.Bad);

                    GameControl.Wait(1000);
                }
            }
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LotaProgram.CommonLotaCommands);

            commands.Items.Add(CommandFactory.Fight("FightAgainstGuard"));
            commands.Items.Add(CommandFactory.Magic("TownMagic"));
            commands.Items.Add(CommandFactory.Leave("TownLeave", confirmPrompt: Options.EnhancedUserInterface));
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Speak("TownSpeak"));
            commands.Items.Add(CommandFactory.Use("LotaUse"));
            commands.Items.Add(CommandFactory.Xamine());
        }

    }
}
