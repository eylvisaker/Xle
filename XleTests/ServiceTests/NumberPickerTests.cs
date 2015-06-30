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

        [TestMethod]
        public void ChooseByKeyboard()
        {
            Services.KeysToSend.AddRange(new []{
                KeyCode.D2, KeyCode.D3, KeyCode.D4, KeyCode.Enter});

            var result = picker.ChooseNumber(() => { }, 2000);

            Assert.AreEqual(234, result);
        }
    }
}
