using ERY.Xle.LotA.MapExtenders.Towns.Stores;
using ERY.Xle.Maps.Extenders;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Stores;
using ERY.Xle.XleEventTypes.Stores.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Magic = ERY.Xle.Services.Implementation.Commands.Magic;

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

        public override void OnLoad(GameState state)
        {
            MuseumCoinSale.ResetMuseumCoinOffers();

            CheckLoan(state);
        }


        void CheckLoan(GameState state)
        {
            if (state.Map.Events.Any(x => x is Store && x.ExtenderName == "StoreLending"))
            {
                if (state.Player.loan > 0 && state.Player.dueDate <= state.Player.TimeDays)
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

            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Leave(confirmPrompt: Options.EnhancedUserInterface));
            commands.Items.Add(CommandFactory.Rob());
            commands.Items.Add(CommandFactory.Speak());
        }

    }
}
