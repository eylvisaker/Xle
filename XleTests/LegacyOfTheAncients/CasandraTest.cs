using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle;
using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ERY.Xle.Maps.XleMapTypes;

namespace ERY.XleTests.LegacyOfTheAncients
{
    [TestClass]
    public class CasandraTest : LotaTest
    {
        Casandra casandra;

        [TestInitialize]
        public void Initialize()
        {
            casandra = new Casandra();

            InitializeEvent(casandra);
            casandra.QuickMenu = Services.QuickMenu.Object;

            GameState.MapExtender = InitializeMap<CastleMap>(1).Object;
        }

        [TestMethod]
        public void CasandraGiveGold()
        {
            Services.QuickMenuCallback = menu => 0;

            Player.Gold = 20;

            casandra.Speak();

            Assert.AreEqual(5020, Player.Gold);
        }

        [TestMethod]
        public void CasandraGiveCharm()
        {
            Services.QuickMenuCallback = menu => 1;

            casandra.Speak();

            Assert.AreEqual(30, Player.Attribute[Attributes.charm]);
        }
    }
}
