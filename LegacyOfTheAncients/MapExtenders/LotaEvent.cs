using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xle.Ancients.MapExtenders
{
    public class LotaEvent : EventExtender
    {
        public LotaStory Story
        {
            get { return GameState.Story(); }
        }
    }
}
