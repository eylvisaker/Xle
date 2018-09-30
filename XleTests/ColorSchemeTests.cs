using ERY.Xle;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ERY.XleTests
{
    public class ColorSchemeTests
    {
        [Fact]
        [Obsolete]
        public void VertLine()
        {
            ColorScheme cs = new ColorScheme();

            cs.VerticalLinePosition = 13 * 16;

            cs.VerticalLinePosition.Should().Be(13 * 16);

            cs.MapAreaWidth.Should().Be(25);
        }
    }
}
