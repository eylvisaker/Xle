using AgateLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xle.Maps.Towns;
using Xle;
using Xle.Commands.Implementation;

namespace Xle.Blacksilver.MapExtenders.Temples
{
    [Transient("TempleFight")]
    public class TempleFight : FightAgainstGuard
    {
        protected override async Task FightInDirection(Direction fightDir)
        {
            await TextArea.PrintLine("\n\nNothing much hit.");
            SoundMan.PlaySound(LotaSound.Bump);
        }
    }
}
