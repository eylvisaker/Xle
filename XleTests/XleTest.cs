using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ERY.XleTests
{
    [TestClass]
    public class XleTest
    {
        protected Player Player { get; private set; }
        protected GameState GameState { get; private set; }
        protected XleServices Services { get; private set; }

        public XleTest()
        {
            Player = new Player();
            GameState = new GameState { Player = Player };

            Services = new XleServices();

        }
    }
}
