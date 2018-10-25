using System.Threading.Tasks;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Fortress.SecondArea
{
    public class Compendium : EventExtender
    {
        private IFortressFinalActivator fortressActivator;

        public Compendium(IFortressFinalActivator fortressActivator)
        {
            this.fortressActivator = fortressActivator;
        }

        public override async Task<bool> Use(int item)
        {
            if (item != (int)LotaItem.GuardJewel)
                return false;
            if (fortressActivator.CompendiumAttacking == false)
                return false;

            //SoundMan.PlaySound("SonicMadgic");
            await GameControl.WaitAsync(2000);

            await TextArea.PrintLine();
            await TextArea.PrintLine("The attack stops.");

            fortressActivator.CompendiumAttacking = false;

            return true;
        }

        public override async Task<bool> Take()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (fortressActivator.CompendiumAttacking)
            {
                await TextArea.PrintLine("You can't hold it.");
            }
            else
            {
                await TextArea.PrintLine("You grab the compendium.");
                SoundMan.PlaySound(LotaSound.VeryGood);
                await GameControl.WaitAsync(500);

                await TextArea.FlashLinesWhile(new CountdownTimer(2500).StillRunning,
                                    XleColor.Yellow, XleColor.Cyan, 50);

                Enabled = false;

                CloseExit(GameState);

                fortressActivator.CreateWarlord();
            }

            return true;
        }

        private void CloseExit(GameState state)
        {
            for (int i = 12; i < 15; i++)
            {
                state.Map[i, 36] = 11;
            }
        }
    }
}
