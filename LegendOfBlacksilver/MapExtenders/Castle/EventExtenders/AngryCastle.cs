using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class AngryCastle : EventExtender
    {
        private DurekCastle durekCastle;

        public AngryCastle(DurekCastle durekCastle)
        {
            this.durekCastle = durekCastle;
        }

        public override bool StepOn(GameState state)
        {
            durekCastle.IsAngry = durekCastle.StoredAngryFlag;
            durekCastle.InOrcArea = false;

            return true;
        }
    }
}
