using ERY.Xle.Services.Commands.Implementation;

namespace ERY.Xle.Services.Commands
{
    public interface ICommandFactory : IXleFactory
    {
        ArmorCommand Armor();
        Climb Climb(string name);
        Disembark Disembark();
        End End();
        Fight Fight(string name = null);
        Gamespeed Gamespeed();
        Hold Hold();
        Inventory Inventory();
        Leave Leave(string name = null, string promptText = "", bool confirmPrompt = true);
        Magic Magic();
        Open Open(string name = null);
        Pass Pass();
        Rob Rob(string name = null);
        Speak Speak(string name = null);
        Take Take();
        Use Use(bool showItemNenu = true);
        WeaponCommand Weapon();
        Xamine Xamine(string name = null);
    }
}
