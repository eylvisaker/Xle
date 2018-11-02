using FluentAssertions;
using System.Threading.Tasks;
using Xle.Ancients.MapExtenders.Castle.Events;
using Xle.Maps.XleMapTypes;
using Xunit;

namespace Xle.Ancients
{
    public class CasandraTest : LotaTest
    {
        private Casandra casandra;

        public CasandraTest()
        {
            casandra = new Casandra();

            InitializeEvent(casandra);
            casandra.QuickMenu = Services.QuickMenu.Object;

            GameState.MapExtender = InitializeMap<CastleMap>(1).Object;
        }

        [Fact]
        public async Task CasandraGiveGold()
        {
            Services.QuickMenuCallback = menu => 0;

            Player.Gold = 20;

            await casandra.Speak();

            Player.Gold.Should().Be(5020);
        }

        [Fact]
        public async Task CasandraGiveCharm()
        {
            Services.QuickMenuCallback = menu => 1;

            await casandra.Speak();

            Player.Attribute[Attributes.charm].Should().Be(30);
        }
    }
}
