using ERY.Xle;
using ERY.Xle.Maps;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Xunit;

namespace ERY.XleTests.Maps
{
    public class GuardListTest
    {
        private GuardList guards;

        public GuardListTest()
        {
            guards = new GuardList();

            for (int i = 0; i < 4; i++)
            {
                guards.Add(new Guard()
                {
                    Facing = Xle.Direction.East,
                    X = 100 + i * 4,
                    Y = 200 + i * 4
                });
            }
        }

        [Fact]
        public void InitializeGuards()
        {
            guards.DefaultAttack = 77;
            guards.DefaultDefense = 88;
            guards.DefaultHP = 45;
            guards.DefaultColor = Color.AliceBlue;

            guards.InitializeGuardData();

            int count = 0;

            foreach (var guard in guards)
            {
                guard.Attack.Should().Be(77);
                guard.Defense.Should().Be(88);
                guard.HP.Should().Be(45);
                guard.Color.Should().Be(Color.AliceBlue);
                guard.Facing.Should().Be(Direction.South);

                count++;
            }

            count.Should().Be(4);
        }

        [Fact]
        public void GuardInSpot()
        {
            guards.GuardInSpot(112, 212).Should().BeTrue();
            guards.GuardInSpot(111, 212).Should().BeTrue();
            guards.GuardInSpot(113, 213).Should().BeTrue();

            guards.GuardInSpot(110, 212).Should().BeFalse();
            guards.GuardInSpot(112, 210).Should().BeFalse();
            guards.GuardInSpot(114, 212).Should().BeFalse();
            guards.GuardInSpot(112, 214).Should().BeFalse();
        }

        [Fact]
        public void GuardListRemoveIndexOfContains()
        {
            var g = guards[1];
            var gafter = guards[2];
            var gbefore = guards[0];

            guards.Remove(g);

            guards.IndexOf(gbefore).Should().Be(0);
            guards.IndexOf(gafter).Should().Be(1);
            guards.Count.Should().Be(3);
            (guards.Contains(gafter)).Should().BeTrue();
            (guards.Contains(g)).Should().BeFalse();
        }

        [Fact]
        public void GuardListRemoveIndexOf()
        {
            var g = guards[1];
            var gafter = guards[2];
            var gbefore = guards[0];

            guards.Remove(g);
            guards.Add(g);

            guards.IndexOf(gbefore).Should().Be(0);
            guards.IndexOf(gafter).Should().Be(1);
            guards.IndexOf(g).Should().Be(3);
            (guards.Contains(gafter)).Should().BeTrue();
            (guards.Contains(g)).Should().BeTrue();
        }
    }
}
