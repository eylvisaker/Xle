using Xle.LoB;
using Xle.LoB.MapExtenders;

namespace Xle.Blacksilver
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
