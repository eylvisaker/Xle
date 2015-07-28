using ERY.Xle.Maps.Dungeons;
using ERY.Xle.Services;
using ERY.Xle.Services.XleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Dungeon.Commands
{
    [ServiceName("BlackmireClimb")]
    public class BlackmireClimb : DungeonClimb
    {
        protected LobStory Story { get { return GameState.Story(); } }

        public ISoundMan SoundMan { get; set; }

        public override void Execute()
        {
            base.Execute();

            if (Player.DungeonLevel == 4 && Story.RotlungContracted == false)
            {
                SoundMan.PlaySound(LotaSound.VeryBad);

                TextArea.PrintLineSlow("You have contracted rotlung.");
                TextArea.PrintLine("Endurance  - 10");

                Story.RotlungContracted = true;
                Player.Attribute[Attributes.endurance] -= 10;
            }

            if (Player.DungeonLevel == 6 && Story.Illusion == false)
            {
                // TODO: turn off the display.
            }
        }
    }
}
