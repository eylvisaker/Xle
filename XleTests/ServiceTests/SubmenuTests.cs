using AgateLib.InputLib;
using ERY.Xle;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus.Implementation;
using ERY.Xle.Services.XleSystem;
using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ERY.XleTests.ServiceTests
{
    public class SubmenuTests
    {
        XleSubMenu subMenu;
        Mock<IXleInput> input;
        Mock<IXleSubMenuRedraw> redraw;
        Mock<IXleGameControl> gameControl;

        public SubmenuTests()
        {
            input = new Mock<IXleInput>();
            redraw = new Mock<IXleSubMenuRedraw>();
            gameControl = new Mock<IXleGameControl>();

            subMenu = new XleSubMenu(gameControl.Object, input.Object, redraw.Object);
        }

        void SetupInputSequence(params KeyCode[] keys)
        {
            var setup = input.SetupSequence(x => x.WaitForKey(redraw.Object.Redraw, It.IsAny<KeyCode[]>()));

            foreach (var key in keys)
                setup.Returns(key);
        }

        [Fact]
        public void ChooseFirstItem()
        {
            SetupInputSequence(KeyCode.Enter);

            var result = subMenu.SubMenu("Title", 0, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(0);
        }

        [Fact]
        public void ChooseDefaultItem()
        {
            SetupInputSequence(KeyCode.Enter);

            var result = subMenu.SubMenu("Title", 1, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(1);
        }

        [Fact]
        public void MoveToTop()
        {
            SetupInputSequence(KeyCode.Up, KeyCode.Up, KeyCode.Up, KeyCode.Up, KeyCode.Enter);

            var result = subMenu.SubMenu("Title", 2, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(0);
        }

        [Fact]
        public void MoveToBottom()
        {
            SetupInputSequence(KeyCode.Down, KeyCode.Down, KeyCode.Down, KeyCode.Down, KeyCode.Enter);

            var result = subMenu.SubMenu("Title", 2, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(3);
        }

        [Fact]
        public void SelectByNumber()
        {
            SetupInputSequence(KeyCode.D2);

            var result = subMenu.SubMenu("Title", 0, new MenuItemList("A", "B", "C", "D"));

            result.Should().Be(2);
        }

        [Fact]
        public void SelectByLetter()
        {
            SetupInputSequence(KeyCode.B);

            var result = subMenu.SubMenu("Title", 0, new MenuItemList("0","1","2","3","4","5","6","7","8","9","A","B","C"));

            result.Should().Be(11);
        }
    }
}
