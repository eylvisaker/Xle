using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle.Services
{
    public interface IEquipmentPicker : IXleService
    {
        ArmorItem PickArmor(ArmorItem armorItem);
        WeaponItem PickWeapon(WeaponItem weaponItem);
    }
}
