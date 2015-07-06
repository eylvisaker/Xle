using ERY.Xle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests
{
    [TestClass]
    public class ColorSchemeTests
    {
        [TestMethod]
        [Obsolete]
        public void VertLine()
        {
            ColorScheme cs = new ColorScheme();

            cs.VerticalLinePosition = 13 * 16;

            Assert.AreEqual(13 * 16, cs.VerticalLinePosition);

            Assert.AreEqual(25, cs.MapAreaWidth);
        }
    }
}
