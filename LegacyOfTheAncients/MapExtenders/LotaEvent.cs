using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.LotA.MapExtenders
{
    public class LotaEvent : EventExtender
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }
    }
}
