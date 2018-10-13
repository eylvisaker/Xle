using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.LotA;

using Xunit;

namespace ERY.XleTests.LegacyOfTheAncients
{
    public class LotaTest : XleTest
    {
        public LotaTest()
        {
            Player.StoryData = new LotaStory();
        }
    }
}
