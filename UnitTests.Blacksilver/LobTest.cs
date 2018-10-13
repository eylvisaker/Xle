using ERY.Xle.LoB;
using ERY.Xle.LoB.MapExtenders;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.LegendOfBlacksilver
{
    public class LobTest : XleTest
    {
        public LobTest()
        {
            Story = new LobStory();
            Player.StoryData = Story;
        }

        protected LobStory Story { get; private set; }

        protected void InitializeEvent(LobEvent evt)
        {
            base.InitializeEvent(evt);

            evt.Input = Services.Input.Object;
            evt.QuickMenu = Services.QuickMenu.Object;
        }
    }
}
