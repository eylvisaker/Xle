using ERY.Xle;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus.Implementation;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moq;
using System;
using Xunit;

namespace ERY.XleTests.ServiceTests
{
    public class QuickMenuRunnerTests
    {
        private QuickMenuRunner qmr;
        private Mock<IXleScreen> screen;
        private Mock<ITextArea> textArea;
        private Mock<IXleInput> input;
        private Mock<IXleGameControl> gameControl;

        public QuickMenuRunnerTests()
        {
            screen = new Mock<IXleScreen>();
            textArea = new Mock<ITextArea>();
            input = new Mock<IXleInput>();
            gameControl = new Mock<IXleGameControl>();

            qmr = new QuickMenuRunner(screen.Object, textArea.Object, input.Object, gameControl.Object);

        }

        [Fact]
        public void QuickMenuYesNoReturnsYes()
        {
            input.Setup(x => x.WaitForKey(It.IsAny<Action>())).Returns(Keys.Enter);

            var result = qmr.QuickMenuYesNo();

            result.Should().Be(0);
        }

        [Fact]
        public void QuickMenuYesNoDefaultNoReturnsNo()
        {
            input.Setup(x => x.WaitForKey(It.IsAny<Action>())).Returns(Keys.Enter);

            var result = qmr.QuickMenuYesNo(true);

            result.Should().Be(1);
        }

        [Fact]
        public void QuickMenuYesNoDefaultNoMoveLeftReturnsYes()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(Keys.Left)
                .Returns(Keys.Enter);

            var result = qmr.QuickMenuYesNo(true);

            result.Should().Be(0);
        }

        [Fact]
        public void QuickMenuSelectThirdOption()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(Keys.Right)
                .Returns(Keys.Right)
                .Returns(Keys.Enter);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(2);
        }

        [Fact]
        public void QuickMenuSelectByKey()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(Keys.M);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(2);
        }

        [Fact]
        public void QuickMenuCantGoTooFarLeft()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(Keys.Left)
                .Returns(Keys.Left)
                .Returns(Keys.Left)
                .Returns(Keys.Enter);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(0);
        }

        [Fact]
        public void QuickMenuCantGoTooFarRight()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(Keys.Right)
                .Returns(Keys.Right)
                .Returns(Keys.Right)
                .Returns(Keys.Right)
                .Returns(Keys.Right)
                .Returns(Keys.Enter);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(3);
        }

        [Fact]
        public void QuickMenuVerifyMenuLayout()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(Keys.Enter);

            string expected = "Choose: Battle  Charge  Magic  Other";
            string actual = null;
            bool found = false;

            textArea.Setup(x => x.PrintLine(It.IsAny<string>(), It.IsAny<Color>()))
                .Callback<string, Color>((s, c) =>
                {
                    actual = actual ?? s;
                    if (s.TrimEnd() == expected)
                        found = true;
                });

            qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            found.Should().BeTrue(
                $"QuickMenu did not produce the correct choice string. It should be '{expected}' but was '{actual}'.");

        }
    }
}
