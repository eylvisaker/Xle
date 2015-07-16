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
        public void ArmanDontGiveIfHasElixirs()
        {
            Player.Items[LobItem.LifeElixir] = 2;

            arman.Speak();

            Assert.AreEqual(2, Player.Items[LobItem.LifeElixir]);
            Assert.IsTrue(Services.TextAreaText.Contains("Go away"));
        }

        [TestMethod]
        public void ArmanGiveOneElixir()
        {
            Player.Items[LobItem.LifeElixir] = 1;

            arman.Speak();

            Assert.AreEqual(2, Player.Items[LobItem.LifeElixir]);
        }

        [TestMethod]
        public void ArmanDontGiveTwiceInOneVisit()
        {
            arman.Speak();

            Player.Items[LobItem.LifeElixir] = 0;

            arman.Speak();

            Assert.AreEqual(0, Player.Items[LobItem.LifeElixir]);
        }
    }
}
