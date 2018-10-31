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
        public void ArmanDontGiveIfOrcsDead()
        {
            Story.DefeatedOrcs = true;

            arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(0);
        }

        [Fact]
        public void ArmanDontGiveIfHasElixirs()
        {
            Player.Items[LobItem.LifeElixir] = 2;

            arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(2);
            (Services.TextAreaText.Contains("Go away")).Should().BeTrue();
        }

        [Fact]
        public void ArmanGiveOneElixir()
        {
            Player.Items[LobItem.LifeElixir] = 1;

            arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(2);
        }

        [Fact]
        public void ArmanDontGiveTwiceInOneVisit()
        {
            arman.Speak();

            Player.Items[LobItem.LifeElixir] = 0;

            arman.Speak();

            Player.Items[LobItem.LifeElixir].Should().Be(0);
        }
    }
}
