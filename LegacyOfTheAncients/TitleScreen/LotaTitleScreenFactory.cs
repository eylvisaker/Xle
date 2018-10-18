using AgateLib;
using ERY.Xle.Bootstrap;
using ERY.Xle.Services;
using System;

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

    [Singleton]
    public class LotaTitleScreenFactory : ILotaTitleScreenFactory
    {
        private readonly IServiceLocator serviceLocator;

        public LotaTitleScreenFactory(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public Splash CreateSplash() => serviceLocator.Resolve<Splash>();

        public FirstMainMenu CreateFirstMainMenu() => serviceLocator.Resolve<FirstMainMenu>();
        public SecondMainMenu CreateSecondMainMenu() => serviceLocator.Resolve<SecondMainMenu>();

        public NewGame CreateNewGame() => serviceLocator.Resolve<NewGame>();
        public LoadGame CreateLoadGame() => serviceLocator.Resolve<LoadGame>();
        public EraseGame CreateEraseGame() => serviceLocator.Resolve<EraseGame>();

        public Introduction CreateIntroduction(string enteredName) => serviceLocator.Resolve<Introduction>(new { enteredName = enteredName });
    }
}
