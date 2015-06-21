using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class AngryCastle : CastleEvent
    {

        public override bool StepOn(GameState state)
        {
            DurekCastleObject.IsAngry = DurekCastleObject.StoredAngryFlag;
            DurekCastleObject.InOrcArea = false;

            return true;
        }
    }
}
