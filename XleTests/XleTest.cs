using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ERY.XleTests
{
    [TestClass]
    public class XleTest
    {
        protected XleServices Services { get; private set; }

        public XleTest()
        {
            Services = new XleServices();
        }
    }
}
