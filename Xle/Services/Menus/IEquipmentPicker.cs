using Microsoft.Xna.Framework;

namespace Xle.Services.Menus
{
    public interface IEquipmentPicker : IXleService
    {
        ArmorItem PickArmor(ArmorItem armorItem);
        ArmorItem PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null);

        WeaponItem PickWeapon(WeaponItem weaponItem);
        WeaponItem PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null);
    }
}
