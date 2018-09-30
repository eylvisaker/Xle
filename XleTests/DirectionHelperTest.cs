﻿using AgateLib.Mathematics.Geometry;
using AgateLib.InputLib;
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
    public class DirectionHelperTest
    {
        private void Verify<Tin,Tout>(Tout[] expected, Tin[] inputs, Func<Tin, Tout> trans)
        {
            inputs.Length.Should().Be(expected.Length, "Array inputs must be equal length.");

            for(int i = 0; i < inputs.Length; i++)
            {
                trans(inputs[i]).Should().Be(expected[i], $"Failed on index {i}.");
            }
        }

        [Fact]
        public void DirectionToPointAndStepDirection()
        {
            var inputs = new[] { Direction.West, Direction.East, Direction.North, Direction.South};
            var expected = new[] { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };

            Verify(expected, inputs, x => x.ToPoint());
            Verify(expected, inputs, x => x.StepDirection());
        }

        [Fact]
        public void DirectionToLeftDirection()
        {
            var inputs = new[] { Direction.West, Direction.East, Direction.North, Direction.South };
            var expected = new[] { new Point(0, 1), new Point(0, -1), new Point(-1, 0), new Point(1, 0) };

            Verify(expected, inputs, x => x.LeftDirection());
        }

        [Fact]
        public void DirectionToRightDirection()
        {
            var inputs = new[] { Direction.West, Direction.East, Direction.North, Direction.South };
            var expected = new[] { new Point(0, -1), new Point(0, 1), new Point(1, 0), new Point(-1, 0) };

            Verify(expected, inputs, x => x.RightDirection());
        }

        [Fact]
        public void PointToDirection()
        {
            var inputs = new[] { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1) };
            var expected = new[] { Direction.West, Direction.East, Direction.North, Direction.South };

            Verify(expected, inputs, x => x.ToDirection());
        }

        [Fact]
        public void KeyCodeToDirection()
        {
            var inputs = new[] { KeyCode.Left, KeyCode.Right, KeyCode.Up, KeyCode.Down };
            var expected = new[] { Direction.West, Direction.East, Direction.North, Direction.South };

            Verify(expected, inputs, x => x.ToDirection());
        }
    }
}
