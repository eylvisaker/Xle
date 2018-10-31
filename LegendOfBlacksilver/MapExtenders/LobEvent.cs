using Xle.Services;
using Xle.Services.Menus;
using Xle.Services.XleSystem;
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
        public IXleInput Input { get; set; }
        public IQuickMenu QuickMenu { get; set; }

        protected LobStory Story { get { return GameState.Story(); } }
        
    }
}
