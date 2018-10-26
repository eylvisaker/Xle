using AgateLib;
using System.Linq;
using Xle.Maps.Towns;
using Xle.Services.Commands;
using Xle.XleEventTypes.Stores;

namespace Xle.Ancients.MapExtenders.Towns
{
    [Transient("LotaTown")]
    public class LotaTown : TownExtender
    {

        public XleOptions Options { get; set; }
        public LotaMuseumCoinSale MuseumCoinSale { get; set; }

        public override void OnLoad()
        {
            CheckLoan();

            base.OnLoad();
        }

        private void CheckLoan()
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
