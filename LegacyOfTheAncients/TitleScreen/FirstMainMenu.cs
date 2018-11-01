using AgateLib;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;

namespace Xle.Ancients.TitleScreen
{
    [Transient, InjectProperties]
    public class FirstMainMenu : MainMenuScreen
    {
        public FirstMainMenu(IContentProvider content) : base(content)
        {
            Colors.BorderColor = XleColor.Purple;
            Colors.FrameColor = XleColor.Blue;
            Colors.FrameHighlightColor = XleColor.White;
            Colors.BackColor = XleColor.LightBlue;
            Colors.TextColor = XleColor.White;

            MenuItems.Add("Play a game");
            MenuItems.Add("Some simple instructions");
            MenuItems.Add("Scenes from legacy");
            MenuItems.Add("Quit to desktop");

            Instruction.SetColor(XleColor.Blue);
        }

        public override Task KeyPress(Keys keyCode, string keyString)
        {
            SoundMan.StopSound(LotaSound.Music);
            return base.KeyPress(keyCode, keyString);
        }

        protected override async Task ExecuteMenuItem(int item)
        {
            if (item == 0)
            {
                NewState = Factory.CreateSecondMainMenu();
            }
            else if (item == 3)
            {
                await Wait(500);
                throw new MainWindowClosedException();
            }
        }

    }
}
