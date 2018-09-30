using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle;
using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;

using Xunit;
using ERY.Xle.Maps.XleMapTypes;
using FluentAssertions;

namespace ERY.XleTests.LegacyOfTheAncients
{
    public class CasandraTest : LotaTest
    {
        Casandra casandra;

        public CasandraTest()
        {
            casandra = new Casandra();

            InitializeEvent(casandra);
            casandra.QuickMenu = Services.QuickMenu.Object;

            GameState.MapExtender = InitializeMap<CastleMap>(1).Object;
        }

        [Fact]
        public void CasandraGiveGold()
        {
            Services.QuickMenuCallback = menu => 0;

            Player.Gold = 20;

            casandra.Speak();

            Player.Gold.Should().Be(5020);
        }

        [Fact]
        public void CasandraGiveCharm()
        {
            Services.QuickMenuCallback = menu => 1;

            casandra.Speak();

            Player.Attribute[Attributes.charm].Should().Be(30);
        }
    }
}
