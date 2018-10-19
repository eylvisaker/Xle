using Xle;
using Xle.Ancients.MapExtenders.Fortress.SecondArea;
using Xle.Maps;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Xle.LegacyOfTheAncients.Fortress
{
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

        [Fact]
        public void UseMagicIceCausesCompendiumAttack()
        {
            evt.Use((int)LotaItem.MagicIce);

            (fortressActivator.Object.CompendiumAttacking).Should().BeTrue();
        }

        [Fact]
        public void UseOtherItemDoesntCauseAttack()
        {
            evt.Use((int)LotaItem.StoneKey);

            (fortressActivator.Object.CompendiumAttacking).Should().BeFalse();
        }
    }
}
