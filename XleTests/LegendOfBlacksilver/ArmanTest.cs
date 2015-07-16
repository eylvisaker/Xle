using ERY.Xle;
using ERY.Xle.LoB.MapExtenders.Castle.EventExtenders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.LegendOfBlacksilver
{
    [TestClass]
    public class ArmanTest : LobTest
    {
        Arman arman;

        [TestInitialize]
        public void Initialize()
        {
            arman = new Arman();                
            InitializeEvent(arman);

            Assert.AreEqual(0, Player.Items[LobItem.LifeElixir]);
        }

        [TestMethod]
        public void ArmanGiveElixirs()
        {
            arman.Speak();

            Assert.AreEqual(2, Player.Items[LobItem.LifeElixir]);
        }

        [TestMethod]
        public void ArmanDontGiveIfOrcsDead()
        {
            Story.DefeatedOrcs = true;

            arman.Speak();

            Assert.AreEqual(0, Player.Items[LobItem.LifeElixir]);
        }

        [TestMethod]
        public void ArmanDontGiveIfAlreadyHappened()
        {
            Story.ArmanGaveElixirs = true;

            arman.Speak();

            Assert.AreEqual(0, Player.Items[LobItem.LifeElixir]);
        }
    }
}
