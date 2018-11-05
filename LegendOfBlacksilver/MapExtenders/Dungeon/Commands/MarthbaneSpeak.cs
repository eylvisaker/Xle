using AgateLib;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Commands.Implementation;
using Xle.Game;
using Xle.Menus;
using Xle.XleSystem;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
{
    [Transient("MarthbaneSpeak")]
    public class MarthbaneSpeak : Speak
    {
        public ISoundMan SoundMan { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public IXleGameControl GameControl { get; set; }

        private MarthbaneTunnels dungeon { get { return (MarthbaneTunnels)GameState.MapExtender; } }

        private DungeonMonster King { get { return dungeon.King; } }

        private DungeonCombat Combat { get { return dungeon.Combat; } }

        private LobStory Story { get { return GameState.Story(); } }

        public override async Task Execute()
        {
            if (await TalkToKing())
                return;

            await base.Execute();
        }

        public async Task<bool> TalkToKing()
        {
            if (Player.DungeonLevel != 7) return false;
            if (King == null) return false;
            if (King.HP <= 0) return false;

            if (Story.MarthbaneOfferedHelpToKing == false)
            {
                SoundMan.PlaySound(LotaSound.VeryGood);

                TextArea.Clear(true);
                await TextArea.PrintLineSlow("I am king durek!!", XleColor.White);
                await TextArea.PrintLineSlow("Do you come to help me?", XleColor.White);
                await TextArea.PrintLineSlow();

                if (await QuickMenu.QuickMenuYesNo() == 1)
                {
                    await DoomedMessage();
                    return true;
                }

                Story.MarthbaneOfferedHelpToKing = true;

                TextArea.Clear(true);
                await TextArea.PrintLineSlow("I fear you have been caught in the", XleColor.White);
                await TextArea.PrintLineSlow("same trap that imprisons me...", XleColor.White);
                await TextArea.PrintLineSlow();
                await TextArea.PrintLineSlow("unless...", XleColor.White);

                await GameControl.WaitAsync(2000);
            }

            TextArea.Clear(true);
            await TextArea.PrintLineSlow("Do you carry my signet ring?", XleColor.White);
            await TextArea.PrintLineSlow();

            if (await QuickMenu.QuickMenuYesNo() == 1)
            {
                await DoomedMessage();
                return true;
            }

            GameState.Player.Items[LobItem.SignetRing] = 0;

            TextArea.Clear(true);
            await TextArea.PrintLineSlow("In times of distress, the ring will\nreturn me to the castle!!  I fear it\ncan do nothing more than give you a\nroute of escape.", XleColor.White);

            await GameControl.WaitAsync(3000);

            TextArea.Clear(true);
            await TextArea.PrintLineSlow("\n\n\nNoble adventurer, i am in your debt.\nMay we meet in better times.", XleColor.White);

            await GameControl.WaitAsync(3000);

            SoundMan.PlaySound(LotaSound.EnemyMiss);

            Story.RescuedKing = true;
            Combat.Monsters.Remove(King);

            OpenEscapeRoute();

            return true;
        }

        private void OpenEscapeRoute()
        {
            dungeon.OpenEscapeRoute();
        }

        private async Task DoomedMessage()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine("Then I fear we are both doomed.", XleColor.White);
            await TextArea.PrintLine();
        }

    }
}
