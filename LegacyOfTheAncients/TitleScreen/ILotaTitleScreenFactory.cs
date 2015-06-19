using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services;

namespace ERY.Xle.LotA.TitleScreen
{
    public interface ILotaTitleScreenFactory : IXleFactory
    {
        Splash CreateSplash();

        FirstMainMenu CreateFirstMainMenu();
        SecondMainMenu CreateSecondMainMenu();

        NewGame CreateNewGame();
        LoadGame CreateLoadGame();
        EraseGame CreateEraseGame();

        Introduction CreateIntroduction(string enteredName);
    }
}
