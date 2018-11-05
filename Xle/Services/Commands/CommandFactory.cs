using AgateLib;
using AgateLib.Foundation;
using Xle.Services.Commands.Implementation;

namespace Xle.Services.Commands
{
    public interface ICommandFactory
    {
        ArmorCommand Armor();
        IClimb Climb(string name);
        Disembark Disembark();
        End End();
        IFight Fight(string name);
        Gamespeed Gamespeed();
        Hold Hold();
        Inventory Inventory();
        ILeave Leave(string name = null, string promptText = "", bool confirmPrompt = true);
        IMagicCommand Magic(string name = null);
        IOpen Open(string name = null);
        Pass Pass();
        IRob Rob(string name = null);
        ISpeak Speak(string name = null);
        ITake Take(string name = null);
        IUse Use(string name);
        WeaponCommand Weapon();
        IXamine Xamine(string name = null);
    }

    [Singleton]
    public class CommandFactory : ICommandFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public CommandFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ArmorCommand Armor() => serviceLocator.Resolve<ArmorCommand>();

        public IClimb Climb(string name) => serviceLocator.ResolveNamed<IClimb>(name);

        public Disembark Disembark() => serviceLocator.Resolve<Disembark>();

        public End End() => serviceLocator.Resolve<End>();

        public IFight Fight(string name) => ResolveNamed<IFight>(name);

        public Gamespeed Gamespeed() => serviceLocator.Resolve<Gamespeed>();

        public Hold Hold() => serviceLocator.Resolve<Hold>();

        public Inventory Inventory() => serviceLocator.Resolve<Inventory>();

        public ILeave Leave(string name = null, string promptText = "", bool confirmPrompt = true)
        {
            ILeave result = ResolveNamed<ILeave>(name ?? "Leave");

            result.PromptText = promptText;
            result.ConfirmPrompt = confirmPrompt;
            return result;
        }

        public IMagicCommand Magic(string name = null) => ResolveNamed<IMagicCommand>(name ?? "Magic");

        public IOpen Open(string name = null) => ResolveNamed<IOpen>(name ?? "Open");

        public Pass Pass() => serviceLocator.Resolve<Pass>();

        public IRob Rob(string name = null) => ResolveNamed<IRob>(name ?? "Rob");

        public ISpeak Speak(string name = null) => ResolveNamed<ISpeak>(name ?? "Speak");

        public ITake Take(string name = null) => ResolveNamed<ITake>(name ?? "Take");

        public IUse Use(string name) => ResolveNamed<IUse>(name);

        public WeaponCommand Weapon() => serviceLocator.Resolve<WeaponCommand>();

        public IXamine Xamine(string name = null) => ResolveNamed<IXamine>(name ?? "Xamine");

        private T ResolveNamed<T>(string name)
        {
            return serviceLocator.ResolveNamed<T>(name);
        }
    }
}
