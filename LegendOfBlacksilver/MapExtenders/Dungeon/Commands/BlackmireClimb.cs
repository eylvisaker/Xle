using Xle.Maps.Dungeons;
using Xle;
using Xle.XleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Dungeon.Commands
{
    [Transient("BlackmireClimb")]
    public class BlackmireClimb : DungeonClimb
    {
        protected LobStory Story { get { return GameState.Story(); } }

        public ISoundMan SoundMan { get; set; }

        public override async Task Execute()
        {
            await base.Execute();

            if (Player.DungeonLevel == 4 && Story.RotlungContracted == false)
            {
                SoundMan.PlaySound(LotaSound.VeryBad);

                await TextArea.PrintLineSlow("You have contracted rotlung.");
                await TextArea.PrintLine("Endurance  - 10");

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
