using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Geometry;

namespace ERY.Xle.Services
{
    public interface IEquipmentPicker : IXleService
    {
        ArmorItem PickArmor(ArmorItem armorItem);
        ArmorItem PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null);

        WeaponItem PickWeapon(WeaponItem weaponItem);
        WeaponItem PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null);
    }
}
