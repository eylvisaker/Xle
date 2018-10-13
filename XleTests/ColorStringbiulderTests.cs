using ERY.Xle;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace ERY.XleTests
{
    public class ColorStringbiulderTests
    {
        [Fact]
        public void AddTextTest()
        {
            ColorStringBuilder b = new ColorStringBuilder();

            b.AddText("Hello ", Color.White);

            b.AddText("World", Color.Yellow);

            var colors = b.Colors;

            for (int i = 0; i < 6; i++)
                colors[i].Should().Be(Color.White);
            for (int i = 6; i < 11; i++)
                colors[i].Should().Be(Color.Yellow);

            b.Text.Should().Be("Hello World");

        }
    }
}
