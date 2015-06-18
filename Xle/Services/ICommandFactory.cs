using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Services
{
    public interface ICommandFactory : IXleFactory
    {
        ArmorCommand Armor();
        Climb Climb();
        Disembark Disembark();
        End End();
        Fight Fight();
        Gamespeed Gamespeed();
        Hold Hold();
        Inventory Inventory();
        Leave Leave(bool confirmPrompt = true);
        Magic Magic();
        Open Open();
        Pass Pass();
        Rob Rob();
        Speak Speak();
        Take Take();
        Use Use(bool showItemNenu = true);
        WeaponCommand Weapon();
        Xamine Xamine();

    }
}
