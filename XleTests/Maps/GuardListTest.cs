using AgateLib.Geometry;
using ERY.Xle;
using ERY.Xle.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.Maps
{
    [TestClass]
    public class GuardListTest
    {
        GuardList guards;

        [TestInitialize]
        public void Init()
        {
            guards = new GuardList();

            for(int i = 0; i < 4; i++)
            {
                guards.Add(new Guard()
                {
                    Facing = Xle.Direction.East,
                    X = 100 + i * 4,
                    Y = 200 + i * 4
                });
            }
        }

        [TestMethod]
        public void InitializeGuards()
        {
            guards.DefaultAttack = 77;
            guards.DefaultDefense = 88;
            guards.DefaultHP = 45;
            guards.DefaultColor = Color.AliceBlue;
            
            guards.InitializeGuardData();

            int count = 0;

            foreach(var guard in guards)
            {
                Assert.AreEqual(77, guard.Attack);
                Assert.AreEqual(88, guard.Defense);
                Assert.AreEqual(45, guard.HP);
                Assert.AreEqual(Color.AliceBlue, guard.Color);
                Assert.AreEqual(Direction.South, guard.Facing);

                count++;
            }

            Assert.AreEqual(4, count);
        }

        [TestMethod]
        public void GuardInSpot()
        {
            Assert.IsTrue(guards.GuardInSpot(112, 212));
            Assert.IsTrue(guards.GuardInSpot(111, 212));
            Assert.IsTrue(guards.GuardInSpot(113, 213));

            Assert.IsFalse(guards.GuardInSpot(110, 212));
            Assert.IsFalse(guards.GuardInSpot(112, 210));
            Assert.IsFalse(guards.GuardInSpot(114, 212));
            Assert.IsFalse(guards.GuardInSpot(112, 214));
        }

        [TestMethod]
        public void GuardListRemoveIndexOfContains()
        {
            var g = guards[1];
            var gafter = guards[2];
            var gbefore = guards[0];

            guards.Remove(g);

            Assert.AreEqual(0, guards.IndexOf(gbefore));
            Assert.AreEqual(1, guards.IndexOf(gafter));
            Assert.AreEqual(3, guards.Count);
            Assert.IsTrue(guards.Contains(gafter));
            Assert.IsFalse(guards.Contains(g));
        }

        [TestMethod]
        public void GuardListRemoveIndexOf()
        {
            var g = guards[1];
            var gafter = guards[2];
            var gbefore = guards[0];

            guards.Remove(g);
            guards.Add(g);

            Assert.AreEqual(0, guards.IndexOf(gbefore));
            Assert.AreEqual(1, guards.IndexOf(gafter));
            Assert.AreEqual(3, guards.IndexOf(g));
            Assert.IsTrue(guards.Contains(gafter));
            Assert.IsTrue(guards.Contains(g));
        }
    }
}
