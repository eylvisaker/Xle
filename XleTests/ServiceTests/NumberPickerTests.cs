using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.InputLib;

using ERY.Xle.Services.Menus.Implementation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ERY.XleTests.ServiceTests
{
    [TestClass]
    public class NumberPickerTests : XleTest
    {
        NumberPicker picker;

        public NumberPickerTests()
        {
            picker = new NumberPicker(Services.Screen.Object, Services.Input.Object, Services.TextArea.Object);
        }

        void SetKeys(params KeyCode[] keys)
        {
            Services.KeysToSend.AddRange(keys);
        }

        [TestMethod]
        public void ChooseByKeyboard()
        {
            SetKeys(KeyCode.D2, KeyCode.D3, KeyCode.D4, KeyCode.Enter);

            var result = picker.ChooseNumber(2000);

            Assert.AreEqual(234, result);
        }

        [TestMethod]
        public void ChooseByKeyboardHitMax()
        {
            SetKeys(KeyCode.D2, KeyCode.D3, KeyCode.D4, KeyCode.D5, KeyCode.Enter);

            var result = picker.ChooseNumber(2000);

            Assert.AreEqual(2000, result);
        }

        [TestMethod]
        public void ChooseByKeyboardHitBackspace()
        {
            SetKeys(KeyCode.D2, KeyCode.D3, KeyCode.BackSpace, KeyCode.D5, KeyCode.Enter);

            var result = picker.ChooseNumber(2000);

            Assert.AreEqual(25, result);
        }

        [TestMethod]
        public void ChooseByJoystick()
        {
            SetKeys(KeyCode.Up, KeyCode.Right, KeyCode.Up, KeyCode.Left, KeyCode.Left, KeyCode.Enter);

            var result = picker.ChooseNumber(100);

            Assert.AreEqual(39, result);
        }

        [TestMethod]
        public void ChooseByJoystickHitMax()
        {
            SetKeys(KeyCode.Up, KeyCode.Up, KeyCode.Up, KeyCode.Enter);

            var result = picker.ChooseNumber(50);

            Assert.AreEqual(50, result);
        }

        [TestMethod]
        public void ChooseByJoystickHitMin()
        {
            SetKeys(KeyCode.Right, KeyCode.Right, KeyCode.Down, KeyCode.Enter);

            var result = picker.ChooseNumber(50);

            Assert.AreEqual(0, result);
        }

    }
}
