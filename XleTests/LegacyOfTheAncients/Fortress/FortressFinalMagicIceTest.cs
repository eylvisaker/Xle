using ERY.Xle;
using ERY.Xle.LotA.MapExtenders.Fortress.SecondArea;
using ERY.Xle.Maps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.LegacyOfTheAncients.Fortress
{
    [TestClass]
    public class FortressFinalMagicIceTest : MagicIceTest
    {
        Mock<IFortressFinalActivator> fortressActivator = new Mock<IFortressFinalActivator>();
        FinalMagicIce evt;

        public FortressFinalMagicIceTest()
        {
            fortressActivator.SetupAllProperties();

            evt = new FinalMagicIce(fortressActivator.Object);
            InitializeEvent(evt);

        }

        [TestMethod]
        public void UseMagicIceCausesCompendiumAttack()
        {
            evt.Use((int)LotaItem.MagicIce);

            Assert.IsTrue(fortressActivator.Object.CompendiumAttacking);
        }

        [TestMethod]
        public void UseOtherItemDoesntCauseAttack()
        {
            evt.Use((int)LotaItem.StoneKey);

            Assert.IsFalse(fortressActivator.Object.CompendiumAttacking);
        }
    }
}
