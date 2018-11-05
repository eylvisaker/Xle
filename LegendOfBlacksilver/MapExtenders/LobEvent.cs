using Xle;
using Xle.Menus;
using Xle.XleSystem;
using Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xle.Blacksilver.MapExtenders
{
    public class LobEvent : EventExtender
    {
        public IQuickMenu QuickMenu { get; set; }

        protected LobStory Story { get { return GameState.Story(); } }
        
    }
}
