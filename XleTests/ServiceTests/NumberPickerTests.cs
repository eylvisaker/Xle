using Xle.Services.Menus.Implementation;
using FluentAssertions;
using Microsoft.Xna.Framework.Input;
using Xunit;

namespace Xle.ServiceTests
{
    public class NumberPickerTests : XleTest
    {
        private NumberPicker picker;

        public NumberPickerTests()
        {
            picker = new NumberPicker(Services.Screen.Object, Services.Input.Object, Services.TextArea.Object);
        }

        private void SetKeys(params Keys[] keys)
        {
            Services.KeysToSend.AddRange(keys);
        }

        [Fact]
        public void ChooseByKeyboard()
        {
            SetKeys(Keys.D2, Keys.D3, Keys.D4, Keys.Enter);

            var result = picker.ChooseNumber(2000);

            result.Should().Be(234);
        }

        [Fact]
        public void ChooseByKeyboardHitMax()
        {
            SetKeys(Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.Enter);

            var result = picker.ChooseNumber(2000);

            result.Should().Be(2000);
        }

        [Fact]
        public void ChooseByKeyboardHitBackspace()
        {
            SetKeys(Keys.D2, Keys.D3, Keys.Back, Keys.D5, Keys.Enter);

            var result = picker.ChooseNumber(2000);

            result.Should().Be(25);
        }

        [Fact]
        public void ChooseByJoystick()
        {
            SetKeys(Keys.Up, Keys.Right, Keys.Up, Keys.Left, Keys.Left, Keys.Enter);

            var result = picker.ChooseNumber(100);

            result.Should().Be(39);
        }

        [Fact]
        public void ChooseByJoystickHitMax()
        {
            SetKeys(Keys.Up, Keys.Up, Keys.Up, Keys.Enter);

            var result = picker.ChooseNumber(50);

            result.Should().Be(50);
        }

        [Fact]
        public void ChooseByJoystickHitMin()
        {
            SetKeys(Keys.Right, Keys.Right, Keys.Down, Keys.Enter);

            var result = picker.ChooseNumber(50);

            result.Should().Be(0);
        }

    }
}
