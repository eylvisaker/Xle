using ERY.Xle.LotA.MapExtenders.Towns.Stores;
using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Magic = ERY.Xle.Services.Commands.Implementation.MagicCommand;

namespace ERY.Xle.LotA.MapExtenders.Towns
{
    public class LotaTown : TownExtender
    {
        ExtenderDictionary mExtenders = new ExtenderDictionary();

        public LotaTown()
        {
            mExtenders.Add("Healer", new StoreHealer());
            mExtenders.Add("StoreHealer", new StoreHealer());
            mExtenders.Add("StoreFortune", new Fortune());
            mExtenders.Add("StoreFood", new StoreFood());
            mExtenders.Add("StoreWeaponTraining", new StoreWeaponTraining());
            mExtenders.Add("StoreArmorTraining", new StoreArmorTraining());
            mExtenders.Add("StoreMagic", new StoreMagic());
            mExtenders.Add("StoreLending", new StoreLending());
            mExtenders.Add("StoreFlipFlop", new StoreFlipFlop());
            mExtenders.Add("StoreBlackjack", new StoreBlackjack());
            mExtenders.Add("StoreArmor", new StoreArmor());
            mExtenders.Add("StoreWeapon", new StoreWeapon());
            mExtenders.Add("StoreRaft", new StoreRaftExtender());
            mExtenders.Add("StoreBank", new StoreBank());
            mExtenders.Add("StoreVault", new Vault());
            mExtenders.Add("StoreBuyback", new StoreBuyback());
        }

        public XleOptions Options { get; set; }
        public LotaMuseumCoinSale MuseumCoinSale { get; set; }

        public override void OnLoad()
        {
            MuseumCoinSale.ResetMuseumCoinOffers();

            CheckLoan();
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
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Leave("TownLeave", confirmPrompt: Options.EnhancedUserInterface));
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Speak("TownSpeak"));
            commands.Items.Add(CommandFactory.Use("LotaUse"));
            commands.Items.Add(CommandFactory.Xamine());
        }

    }
}
