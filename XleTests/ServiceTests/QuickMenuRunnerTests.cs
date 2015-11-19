using AgateLib.Geometry;
using AgateLib.InputLib;
using ERY.Xle;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus.Implementation;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.XleTests.ServiceTests
{
    [TestClass]
    public class QuickMenuRunnerTests
    {
        QuickMenuRunner qmr;
        Mock<IXleScreen> screen;
        Mock<ITextArea> textArea;
        Mock<IXleInput> input;
        Mock<IXleGameControl> gameControl;

        [TestInitialize]
        public void Init()
        {
            screen = new Mock<IXleScreen>();
            textArea = new Mock<ITextArea>();
            input = new Mock<IXleInput>();
            gameControl = new Mock<IXleGameControl>();

            qmr = new QuickMenuRunner(screen.Object, textArea.Object, input.Object, gameControl.Object);

        }

        [TestMethod]
        public void QuickMenuYesNoReturnsYes()
        {
            input.Setup(x => x.WaitForKey(It.IsAny<Action>())).Returns(KeyCode.Enter);

            var result = qmr.QuickMenuYesNo();

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void QuickMenuYesNoDefaultNoReturnsNo()
        {
            input.Setup(x => x.WaitForKey(It.IsAny<Action>())).Returns(KeyCode.Enter);

            var result = qmr.QuickMenuYesNo(true);

            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void QuickMenuYesNoDefaultNoMoveLeftReturnsYes()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(KeyCode.Left)
                .Returns(KeyCode.Enter);

            var result = qmr.QuickMenuYesNo(true);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void QuickMenuSelectThirdOption()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Enter);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void QuickMenuSelectByKey()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(KeyCode.M);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void QuickMenuCantGoTooFarLeft()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(KeyCode.Left)
                .Returns(KeyCode.Left)
                .Returns(KeyCode.Left)
                .Returns(KeyCode.Enter);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void QuickMenuCantGoTooFarRight()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Right)
                .Returns(KeyCode.Enter);

            var result = qmr.QuickMenu(new MenuItemList("Battle", "Charge", "Magic", "Other"), 2);

            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void QuickMenuVerifyMenuLayout()
        {
            input.SetupSequence(x => x.WaitForKey(It.IsAny<Action>()))
                .Returns(KeyCode.Enter);

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

            Assert.IsTrue(found, "QuickMenu did not produce the correct choice string. It should be '{0}' but was '{1}'.",
                expected, actual);

        }
    }
}
