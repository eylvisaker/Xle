using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.XleSystem;

namespace Xle.LoB.MapExtenders.Dungeon.Commands
{
    [ServiceName("MarthbaneSpeak")]
    public class MarthbaneSpeak : Speak
    {
        public ISoundMan SoundMan { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public IXleGameControl GameControl { get; set; }

        MarthbaneTunnels dungeon { get { return (MarthbaneTunnels)GameState.MapExtender; } }
        DungeonMonster King { get { return dungeon.King; } }
        DungeonCombat Combat { get { return dungeon.Combat; } }
        LobStory Story { get { return GameState.Story(); } }
        
        public override void Execute()
        {
            if (TalkToKing())
                return;

            base.Execute();
        }

        public bool TalkToKing()
        {
            if (Player.DungeonLevel != 7) return false;
            if (King == null) return false;
            if (King.HP <= 0) return false;

            if (Story.MarthbaneOfferedHelpToKing == false)
            {
                SoundMan.PlaySound(LotaSound.VeryGood);

                TextArea.Clear(true);
                TextArea.PrintLineSlow("I am king durek!!", XleColor.White);
                TextArea.PrintLineSlow("Do you come to help me?", XleColor.White);
                TextArea.PrintLineSlow();

                if (QuickMenu.QuickMenuYesNo() == 1)
                {
                    DoomedMessage();
                    return true;
                }

                Story.MarthbaneOfferedHelpToKing = true;

                TextArea.Clear(true);
                TextArea.PrintLineSlow("I fear you have been caught in the", XleColor.White);
                TextArea.PrintLineSlow("same trap that imprisons me...", XleColor.White);
                TextArea.PrintLineSlow();
                TextArea.PrintLineSlow("unless...", XleColor.White);

                GameControl.Wait(2000);
            }

            TextArea.Clear(true);
            TextArea.PrintLineSlow("Do you carry my signet ring?", XleColor.White);
            TextArea.PrintLineSlow();

            if (QuickMenu.QuickMenuYesNo() == 1)
            {
                DoomedMessage();
                return true;
            }

            GameState.Player.Items[LobItem.SignetRing] = 0;

            TextArea.Clear(true);
            TextArea.PrintLineSlow("In times of distress, the ring will\nreturn me to the castle!!  I fear it\ncan do nothing more than give you a\nroute of escape.", XleColor.White);

            GameControl.Wait(3000);

            TextArea.Clear(true);
            TextArea.PrintLineSlow("\n\n\nNoble adventurer, i am in your debt.\nMay we meet in better times.", XleColor.White);

            GameControl.Wait(3000);

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

        private void DoomedMessage()
        {
            TextArea.PrintLine();
            TextArea.PrintLine("Then I fear we are both doomed.", XleColor.White);
            TextArea.PrintLine();
        }

    }
}
