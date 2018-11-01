using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib;

namespace Xle.Blacksilver.MapExtenders.Castle.EventExtenders
{
    [Transient("AngryCastle")]
    public class AngryCastle : CastleEvent
    {

        public override Task<bool> StepOn()
        {
            DurekCastleObject.IsAngry = DurekCastleObject.StoredAngryFlag;
            DurekCastleObject.InOrcArea = false;

            return Task.FromResult(true);
        }
    }
}
