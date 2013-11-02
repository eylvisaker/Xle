using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERY.Xle;
using AgateLib.Geometry;

namespace ERY.XleTests
{
	[TestClass]
	public class XleBaseTests
	{
		[TestMethod]
		public void ColorStringBuilderTest()
		{
			ColorStringBuilder b = new ColorStringBuilder();

			b.AddText("Hello ", Color.White);

			b.AddText("World", Color.Yellow);

			var colors = b.Colors;

			for (int i = 0; i < 6; i++)
				Assert.AreEqual( Color.White, colors[i]);
			for (int i = 6; i < 11; i++)
				Assert.AreEqual(Color.Yellow, colors[i]);

			Assert.AreEqual("Hello World", b.Text);

		}
	}
}
