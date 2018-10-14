﻿using ERY.Xle;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus.Implementation;
using ERY.Xle.Services.XleSystem;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Moq;
using Xunit;

namespace ERY.XleTests.ServiceTests
{
    public class SubmenuTests
    {
        private XleSubMenu subMenu;
        private Mock<IXleInput> input;
        private Mock<IXleSubMenuRedraw> redraw;
        private Mock<IXleGameControl> gameControl;

        public SubmenuTests()
        {
            input = new Mock<IXleInput>();
            redraw = new Mock<IXleSubMenuRedraw>();
            gameControl = new Mock<IXleGameControl>();

            subMenu = new XleSubMenu(gameControl.Object, input.Object, redraw.Object);
        }

        private void SetupInputSequence(params Keys[] keys)
        {
            var setup = input.SetupSequence(x => x.WaitForKey(redraw.Object.Redraw, It.IsAny<Keys[]>()));

            foreach (var key in keys)
                setup.Returns(key);
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
