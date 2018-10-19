using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class AngryCastle : CastleEvent
    {

        public override bool StepOn()
        {
            DurekCastleObject.IsAngry = DurekCastleObject.StoredAngryFlag;
            DurekCastleObject.InOrcArea = false;

            return true;
        }
    }
}
