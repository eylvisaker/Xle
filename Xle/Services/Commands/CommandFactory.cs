using AgateLib;
using Xle.Bootstrap;
using Xle.Services.Commands.Implementation;

namespace Xle.Services.Commands
{
    public interface ICommandFactory
    {
        ArmorCommand Armor();
        IClimb Climb(string name);
        Disembark Disembark();
        End End();
        IFight Fight(string name = null);
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

        public IFight Fight(string name = null) => ResolveMaybeNamed<IFight>(name);

        public Gamespeed Gamespeed() => serviceLocator.Resolve<Gamespeed>();

        public Hold Hold() => serviceLocator.Resolve<Hold>();

        public Inventory Inventory() => serviceLocator.Resolve<Inventory>();

        public ILeave Leave(string name = null, string promptText = "", bool confirmPrompt = true)
        {
            ILeave result;
            if (name == null)
                result = serviceLocator.Resolve<ILeave>();
            else
                result = serviceLocator.ResolveNamed<ILeave>(name);

            result.PromptText = promptText;
            result.ConfirmPrompt = confirmPrompt;
            return result;
        }

        public IMagicCommand Magic(string name = null) => ResolveMaybeNamed<IMagicCommand>(name);

        public IOpen Open(string name = null) => ResolveMaybeNamed<IOpen>(name);

        public Pass Pass() => serviceLocator.Resolve<Pass>();

        public IRob Rob(string name = null) => ResolveMaybeNamed<IRob>(name);

        public ISpeak Speak(string name = null) => ResolveMaybeNamed<ISpeak>(name);

        public ITake Take(string name = null) => ResolveMaybeNamed<ITake>(name);

        public IUse Use(string name) => ResolveMaybeNamed<IUse>(name);

        public WeaponCommand Weapon() => serviceLocator.Resolve<WeaponCommand>();

        public IXamine Xamine(string name = null) => ResolveMaybeNamed<IXamine>(name);

        private T ResolveMaybeNamed<T>(string name)
        {
            if (name == null)
                return serviceLocator.Resolve<T>();
            else
                return serviceLocator.ResolveNamed<T>(name);
        }
    }
}
