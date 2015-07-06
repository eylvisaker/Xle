using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.InputLib;

using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;

using Moq;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.MapLoad;

namespace ERY.XleTests
{
    public class XleServices
    {
        public XleServices()
        {
            KeysToSend = new List<KeyCode>();

            Screen = new Mock<IXleScreen>();
            Screen.SetupAllProperties();

            Input = new Mock<IXleInput>();
            Input.SetupAllProperties();
            Input.Setup(x => x.WaitForKey(It.IsAny<Action>(), It.IsAny<KeyCode[]>())).Returns(() =>
            {
                var result = KeysToSend.First();
                KeysToSend.RemoveAt(0);
                return result;
            });

            TextArea = new Mock<ITextArea>();
            TextArea.SetupAllProperties();

            GameControl = new Mock<IXleGameControl>();
            GameControl.SetupAllProperties();

            MapChanger = new Mock<IMapChanger>();
            MapChanger.SetupAllProperties();
        }

        public List<KeyCode> KeysToSend { get; set; } 
        
        public Mock<IXleScreen> Screen { get; set; }
        public Mock<IXleInput> Input { get; set; }
        public Mock<ITextArea> TextArea { get; set; }
        public Mock<IXleGameControl> GameControl { get; set; }
        public Mock<IMapChanger> MapChanger { get; set; }
    }
}
