using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders.Castle.EventExtenders
{
    public class AngryOrcs : EventExtender
    {
        private DurekCastle durekCastle;

        public AngryOrcs(DurekCastle durekCastle)
        {
            this.durekCastle = durekCastle;
        }

        public override bool StepOn(GameState state)
        {
            if (durekCastle.InOrcArea == false)
            {
                durekCastle.StoredAngryFlag = durekCastle.IsAngry;

                durekCastle.IsAngry = true;
                durekCastle.InOrcArea = true;
            }

            return true;
        }
    }
}
