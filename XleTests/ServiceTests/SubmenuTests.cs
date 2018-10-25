using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Moq;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.Menus.Implementation;
using Xle.Services.Rendering;
using Xle.Services.XleSystem;
using Xunit;

namespace Xle.ServiceTests
{
    public class SubmenuTests
    {
        private XleSubMenu subMenu;
        private Mock<IXleInput> input;
        private Mock<IXleScreenCapture> redraw;
        private Mock<IXleGameControl> gameControl;
        private Mock<IMenuRenderer> renderer;

        public SubmenuTests()
        {
            input = new Mock<IXleInput>();
            redraw = new Mock<IXleScreenCapture>();
            gameControl = new Mock<IXleGameControl>();
            renderer = new Mock<IMenuRenderer>();

            subMenu = new XleSubMenu(gameControl.Object, redraw.Object, renderer.Object);
        }

        private void SetupInputSequence(params Keys[] keys)
        {
            var setup = input.SetupSequence(x => x.WaitForKey(It.IsAny<Keys[]>()));

            foreach (var key in keys)
                setup.ReturnsAsync(key);
        }

        [Fact]
        public void ChooseFirstItem()
        {
            SetupInputSequence(Keys.Enter);

            var result = subMenu.SubMenu("Title", 0, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(0);
        }

        [Fact]
        public void ChooseDefaultItem()
        {
            SetupInputSequence(Keys.Enter);

            var result = subMenu.SubMenu("Title", 1, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(1);
        }

        [Fact]
        public void MoveToTop()
        {
            SetupInputSequence(Keys.Up, Keys.Up, Keys.Up, Keys.Up, Keys.Enter);

            var result = subMenu.SubMenu("Title", 2, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(0);
        }

        [Fact]
        public void MoveToBottom()
        {
            SetupInputSequence(Keys.Down, Keys.Down, Keys.Down, Keys.Down, Keys.Enter);

            var result = subMenu.SubMenu("Title", 2, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(3);
        }

        [Fact]
        public void SelectByNumber()
        {
            SetupInputSequence(Keys.D2);

            var result = subMenu.SubMenu("Title", 0, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(2);
        }

        [Fact]
        public void SelectByLetter()
        {
            SetupInputSequence(Keys.B);

            var result = subMenu.SubMenu("Title", 0, new MenuItemList("0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C"));

            result.Should().Be(11);
        }
    }
}
