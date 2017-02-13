using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.InputLib;

using ERY.Xle;
using ERY.Xle.Data;
using ERY.Xle.Maps.Dungeons;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.MapLoad;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.ScreenModel;
using ERY.Xle.Services.XleSystem;
using ERY.Xle.XleEventTypes.Stores.Buyback;
using Moq;

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
            TextArea.Setup(x => x.Print(It.IsAny<string>(), It.IsAny<Color[]>())).Callback((string text, Color[] colors) => AppendTextAreaText(text));
            TextArea.Setup(x => x.Print(It.IsAny<string>(), It.IsAny<Color?>())).Callback((string text, Color? color) => AppendTextAreaText(text));
            TextArea.Setup(x => x.PrintLine(It.IsAny<string>(), It.IsAny<Color[]>())).Callback((string text, Color[] colors) => AppendTextAreaText(text + Environment.NewLine));
            TextArea.Setup(x => x.PrintLine(It.IsAny<string>(), It.IsAny<Color>())).Callback((string text, Color color) => AppendTextAreaText(text + Environment.NewLine));

            SubMenu = new Mock<IXleSubMenu>();
            SubMenu.SetupAllProperties();

            Images = new Mock<IXleImages>();
            Images.SetupAllProperties();

            CommandList = new Mock<ICommandList>();
            CommandList.SetupAllProperties();
            CommandList.SetupGet(x => x.Items).Returns(new List<ICommand>());

            MapLoader = new Mock<IMapLoader>();
            MapLoader.SetupAllProperties();

            NumberPicker = new Mock<INumberPicker>();
            NumberPicker.SetupAllProperties();
            GameControl = new Mock<IXleGameControl>();
            GameControl.SetupAllProperties();

            SoundMan = new Mock<ISoundMan>();
            SoundMan.SetupAllProperties();

            QuickMenu = new Mock<IQuickMenu>();
            QuickMenu.SetupAllProperties();
            InitializeQuickMenu();

            MapChanger = new Mock<IMapChanger>();
            MapChanger.SetupAllProperties();

            DungeonAdapter = new Mock<IDungeonAdapter>();
            DungeonAdapter.SetupAllProperties();

            StatsDisplay = new Mock<IStatsDisplay>();
            StatsDisplay.SetupAllProperties();

            BuybackFormatter = new Mock<IBuybackFormatter>();
            BuybackFormatter.SetupAllProperties();

            BuybackOfferWindow = new Mock<IBuybackOfferWindow>();
            BuybackOfferWindow.SetupAllProperties();

            Data = new XleData();
            InitializeData();
        }

        private void AppendTextAreaText(string text)
        {
            TextAreaText += text;
        }

        private void InitializeQuickMenu()
        {
            QuickMenu.Setup(
                x =>
                    x.QuickMenu(It.IsAny<MenuItemList>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Color?>(), It.IsAny<Color?>(),
                        It.IsAny<Action>()))
                .Returns((MenuItemList menu, int a, int b, Color? c, Color? d, Action e) =>
                {
                    if (QuickMenuCallback != null)
                    {
                        return QuickMenuCallback(menu);
                    }

                    return 0;
                });
        }

        public delegate int QuickMenuHandler(MenuItemList menu);
        public QuickMenuHandler QuickMenuCallback;

        public Mock<IXleScreen> Screen { get; set; }
        public Mock<IXleInput> Input { get; set; }
        public Mock<ITextArea> TextArea { get; set; }
        public Mock<IXleSubMenu> SubMenu { get; set; }
        public Mock<IXleImages> Images { get; set; }
        public Mock<ICommandList> CommandList { get; set; }
        public Mock<IMapLoader> MapLoader { get; set; }
        public Mock<IXleGameControl> GameControl { get; set; }
        public Mock<ISoundMan> SoundMan { get; set; }
        public Mock<IQuickMenu> QuickMenu { get; set; }
        public Mock<IMapChanger> MapChanger { get; set; }
        public Mock<IDungeonAdapter> DungeonAdapter { get; set; }
        public Mock<IStatsDisplay> StatsDisplay { get; set; }

        public XleData Data { get; set; }

        public string TextAreaText { get; set; }

        public List<KeyCode> KeysToSend { get; set; }
        public Mock<IBuybackFormatter> BuybackFormatter { get; set; }
        public Mock<IBuybackOfferWindow> BuybackOfferWindow { get; internal set; }
        public Mock<INumberPicker> NumberPicker { get; internal set; }

        private void InitializeData()
        {
            Data.WeaponList.Add(1, "Knife", "30,40,52,66,81");
            Data.WeaponList.Add(2, "Leaded Club", "66,80,97,114,133");
            Data.WeaponList.Add(3, "Bladed Staff", "115,134,155,177,200");

            Data.ArmorList.Add(1, "Studded Hide", "50,65,82,103,127");
            Data.ArmorList.Add(2, "Ring Mail", "120,147,177,211,249");
            Data.ArmorList.Add(3, "Double Mail", "239,281,327,379,436");

            Data.QualityList.Add(0, "Shoddy");
            Data.QualityList.Add(1, "Fair");
            Data.QualityList.Add(2, "Good");
            Data.QualityList.Add(3, "Great");
            Data.QualityList.Add(4, "Superb");
        }
    }
}
