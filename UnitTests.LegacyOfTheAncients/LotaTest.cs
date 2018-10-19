using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xle.Ancients;

using Xunit;

namespace Xle.LegacyOfTheAncients
{
    public class LotaTest : XleTest
    {
        public LotaTest()
        {
            Player.StoryData = new LotaStory();
        }
    }
}
