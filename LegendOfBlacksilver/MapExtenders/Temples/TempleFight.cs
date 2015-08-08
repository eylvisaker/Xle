using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Maps.Towns;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.LoB.MapExtenders.Temples
{
    [ServiceName("TempleFight")]
    public class TempleFight : FightAgainstGuard
    {
        protected override void FightInDirection(Direction fightDir)
        {
            TextArea.PrintLine("\n\nNothing much hit.");
            SoundMan.PlaySound(LotaSound.Bump);
        }
    }
}
