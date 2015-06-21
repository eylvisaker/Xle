using ERY.Xle.Services;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.LoB.MapExtenders
{
    public class LobEvent : EventExtender
    {
        public IXleInput Input { get; set; }
        public IQuickMenu QuickMenu { get; set; }

        protected LobStory Story { get { return GameState.Story(); } }
        
    }
}
