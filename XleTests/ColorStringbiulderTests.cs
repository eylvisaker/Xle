using System;
using AgateLib.DisplayLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERY.Xle;
using AgateLib.Mathematics.Geometry;

namespace ERY.XleTests
{
    [TestClass]
    public class ColorStringbiulderTests
    {
        [TestMethod]
        public void AddTextTest()
        {
            ColorStringBuilder b = new ColorStringBuilder();

            b.AddText("Hello ", Color.White);

            b.AddText("World", Color.Yellow);

            var colors = b.Colors;

            for (int i = 0; i < 6; i++)
                Assert.AreEqual(Color.White, colors[i]);
            for (int i = 6; i < 11; i++)
                Assert.AreEqual(Color.Yellow, colors[i]);

            Assert.AreEqual("Hello World", b.Text);

        }
    }
}
