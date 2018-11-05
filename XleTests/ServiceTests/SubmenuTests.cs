using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Moq;
using System.Threading.Tasks;
using Xle.Game;
using Xle.Menus;
using Xle.Menus.Implementation;
using Xle.Rendering;
using Xle.XleSystem;
using Xunit;

namespace Xle.ServiceTests
{
    public class SubmenuTests
    {
        private XleSubMenu subMenu;
        private Mock<IXleGameControl> gameControl;
        private Mock<IMenuRenderer> renderer;

        public SubmenuTests()
        {
            gameControl = new Mock<IXleGameControl>();
            renderer = new Mock<IMenuRenderer>();

            subMenu = new XleSubMenu(gameControl.Object, renderer.Object);
        }

        private void SetupInputSequence(params Keys[] keys)
        {
            var setup = gameControl.SetupSequence(x => x.WaitForKey(It.IsAny<bool>()));

            foreach (var key in keys)
                setup.ReturnsAsync(key);
        }

        [Fact]
        public async Task ChooseFirstItem()
        {
            SetupInputSequence(Keys.Enter);

            var result = await subMenu.SubMenu("Title", 0, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(0);
        }

        [Fact]
        public async Task ChooseDefaultItem()
        {
            SetupInputSequence(Keys.Enter);

            var result = await subMenu.SubMenu("Title", 1, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(1);
        }

        [Fact]
        public async Task MoveToTop()
        {
            SetupInputSequence(Keys.Up, Keys.Up, Keys.Up, Keys.Up, Keys.Enter);

            var result = await subMenu.SubMenu("Title", 2, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(0);
        }

        [Fact]
        public async Task MoveToBottom()
        {
            SetupInputSequence(Keys.Down, Keys.Down, Keys.Down, Keys.Down, Keys.Enter);

            var result = await subMenu.SubMenu("Title", 2, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(3);
        }

        [Fact]
        public async Task SelectByNumber()
        {
            SetupInputSequence(Keys.D2);

            var result = await subMenu.SubMenu("Title", 0, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(2);
        }

        [Fact]
        public async Task SelectByLetter()
        {
            SetupInputSequence(Keys.B);

            var result = await subMenu.SubMenu("Title", 0, new MenuItemList("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C"));

            result.Should().Be(11);
        }
    }
}
