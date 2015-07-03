using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.LotA;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ERY.XleTests.LegacyOfTheAncients
{
    [TestClass]
    public class LotaTest : XleTest
    {
        public LotaTest()
        {
            Player.StoryData = new LotaStory();
        }
    }
}
