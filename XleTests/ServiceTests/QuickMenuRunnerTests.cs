using FluentAssertions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Moq;
using System.Threading.Tasks;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.ScreenModel;
using Xunit;

namespace Xle.ServiceTests
{
    public class QuickMenuRunnerTests
    {
        private QuickMenuRunner qmr;
        private Mock<IXleScreen> screen;
        private Mock<ITextArea> textArea;
        private Mock<IXleGameControl> gameControl;

        public QuickMenuRunnerTests()
        {
            screen = new Mock<IXleScreen>();
            textArea = new Mock<ITextArea>();
            gameControl = new Mock<IXleGameControl>();

            textArea.Setup(x => x.Print(It.IsAny<string>(), It.IsAny<Color[]>())).Returns(Task.CompletedTask);
            textArea.Setup(x => x.PrintLine(It.IsAny<string>(), It.IsAny<Color>())).Returns(Task.CompletedTask);
            textArea.Setup(x => x.PrintLine(It.IsAny<string>(), It.IsAny<Color[]>())).Returns(Task.CompletedTask);
            textArea.Setup(x => x.PrintLineCentered(It.IsAny<string>(), It.IsAny<Color>())).Returns(Task.CompletedTask);
            textArea.Setup(x => x.PrintLineSlow(It.IsAny<string>(), It.IsAny<Color[]>())).Returns(Task.CompletedTask);
            textArea.Setup(x => x.PrintSlow(It.IsAny<string>(), It.IsAny<Color[]>())).Returns(Task.CompletedTask);

            qmr = new QuickMenuRunner(screen.Object, textArea.Object, gameControl.Object);
        }

        private void SetupInputSequence(params Keys[] keys)
        {
            var sequence = gameControl.SetupSequence(x => x.WaitForKey(It.IsAny<bool>()));

            foreach (var key in keys)
            {
                sequence.ReturnsAsync(key);
            }
        }

        [Fact]
        public async Task QuickMenuYesNoReturnsYes()
        {
            SetupInputSequence(Keys.Enter);

            var result = await qmr.QuickMenuYesNo();

            result.Should().Be(0);
        }

        [Fact]
        public async Task QuickMenuYesNoDefaultNoReturnsNo()
        {
            SetupInputSequence(Keys.Enter);

            var result = await qmr.QuickMenuYesNo(true);

            result.Should().Be(1);
        }

        [Fact]
        public async Task QuickMenuYesNoDefaultNoMoveLeftReturnsYes()
        {
            SetupInputSequence(Keys.Left, Keys.Enter);

            var result = await qmr.QuickMenuYesNo(true);

            result.Should().Be(0);
        }

        [Fact]
        public async Task QuickMenuSelectThirdOption()
        {
            SetupInputSequence(Keys.Right, Keys.Right, Keys.Enter);

            var result = await qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(2);
        }

        [Fact]
        public async Task QuickMenuSelectByKey()
        {
            SetupInputSequence(Keys.M);

            var result = await qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(2);
        }

        [Fact]
        public async Task QuickMenuCantGoTooFarLeft()
        {
            SetupInputSequence(Keys.Left, Keys.Left, Keys.Left, Keys.Enter);

            var result = await qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(0);
        }

        [Fact]
        public async Task QuickMenuCantGoTooFarRight()
        {
            SetupInputSequence(Keys.Right, Keys.Right, Keys.Right, Keys.Right, Keys.Right, Keys.Right, Keys.Enter);

            var result = await qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            result.Should().Be(3);
        }

        [Fact]
        public async Task QuickMenuVerifyMenuLayout()
        {
            SetupInputSequence(Keys.Enter);

            string expected = "Choose: Battle  Charge  Magic  Other";
            string actual = null;
            bool found = false;

            textArea.Setup(x => x.PrintLine(It.IsAny<string>(), It.IsAny<Color>()))
                .Returns<string, Color>((s, c) =>
                {
                    actual = actual ?? s;
                    if (s.TrimEnd() == expected)
                        found = true;
                    return Task.CompletedTask;
                });

            await qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            found.Should().BeTrue(
                $"QuickMenu did not produce the correct choice string. It should be '{expected}' but was '{actual}'.");
        }
    }
}
