using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.InputLib;

using ERY.Xle.Services.Menus.Implementation;
using FluentAssertions;
using Xunit;

namespace ERY.XleTests.ServiceTests
{
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

        [Fact]
        public void ChooseByKeyboard()
        {
            SetKeys(KeyCode.D2, KeyCode.D3, KeyCode.D4, KeyCode.Enter);

            var result = picker.ChooseNumber(2000);

            result.Should().Be(234);
        }

        [Fact]
        public void ChooseByKeyboardHitMax()
        {
            SetKeys(KeyCode.D2, KeyCode.D3, KeyCode.D4, KeyCode.D5, KeyCode.Enter);

            var result = picker.ChooseNumber(2000);

            result.Should().Be(2000);
        }

        [Fact]
        public void ChooseByKeyboardHitBackspace()
        {
            SetKeys(KeyCode.D2, KeyCode.D3, KeyCode.BackSpace, KeyCode.D5, KeyCode.Enter);

            var result = picker.ChooseNumber(2000);

            result.Should().Be(25);
        }

        [Fact]
        public void ChooseByJoystick()
        {
            SetKeys(KeyCode.Up, KeyCode.Right, KeyCode.Up, KeyCode.Left, KeyCode.Left, KeyCode.Enter);

            var result = picker.ChooseNumber(100);

            result.Should().Be(39);
        }

        [Fact]
        public void ChooseByJoystickHitMax()
        {
            SetKeys(KeyCode.Up, KeyCode.Up, KeyCode.Up, KeyCode.Enter);

            var result = picker.ChooseNumber(50);

            result.Should().Be(50);
        }

        [Fact]
        public void ChooseByJoystickHitMin()
        {
            SetKeys(KeyCode.Right, KeyCode.Right, KeyCode.Down, KeyCode.Enter);

            var result = picker.ChooseNumber(50);

            result.Should().Be(0);
        }

    }
}
