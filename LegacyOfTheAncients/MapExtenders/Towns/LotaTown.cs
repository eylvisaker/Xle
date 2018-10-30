using AgateLib;
using System.Linq;
using System.Threading.Tasks;
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
            base.OnLoad();
        }

        public override async Task OnAfterEntry()
        {
            await base.OnAfterEntry();

            await CheckLoan();
        }

        private async Task CheckLoan()
        {
            if (TheMap.Events.Any(x => x is Store && x.ExtenderName == "StoreLending"))
            {
                if (Player.loan > 0 && Player.dueDate <= Player.TimeDays)
                {
                    await TextArea.PrintLine("This is your friendly lender.");
                    await TextArea.PrintLine("You owe me money!");
                    SoundMan.PlaySound(LotaSound.Bad);

                    await GameControl.WaitAsync(1000);
                }
            }
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Hold());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

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
