using FluentAssertions;
using System.Threading.Tasks;
using Xle.Blacksilver.MapExtenders.Castle.EventExtenders;
using Xunit;

namespace Xle.Blacksilver
{
    public class ArmanTest : LobTest
    {
        private Arman arman;

        public ArmanTest()
        {
            arman = new Arman();
            InitializeEvent(arman);

            Player.Items[LobItem.LifeElixir].Should().Be(0);
        }

        [Fact]
        public async Task ArmanGiveElixirs()
        {
            await arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(2);
        }

        [Fact]
        public async Task ArmanDontGiveIfOrcsDead()
        {
            Story.DefeatedOrcs = true;

            await arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(0);
        }

        [Fact]
        public async Task ArmanDontGiveIfHasElixirs()
        {
            Player.Items[LobItem.LifeElixir] = 2;

            await arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(2);
            (Services.TextAreaText.Contains("Go away")).Should().BeTrue();
        }

        [Fact]
        public async Task ArmanGiveOneElixir()
        {
            Player.Items[LobItem.LifeElixir] = 1;

            await arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(2);
        }

        [Fact]
        public async Task ArmanDontGiveTwiceInOneVisit()
        {
            await arman.Speak();

            Player.Items[LobItem.LifeElixir] = 0;

            await arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(0);
        }
    }
}
