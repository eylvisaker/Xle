using Xle.Data;
using Microsoft.Xna.Framework;
using System.Linq;
using AgateLib;

namespace Xle.Services.Menus
{
    public interface IEquipmentPicker
    {
        ArmorItem PickArmor(ArmorItem armorItem);
        ArmorItem PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null);

        WeaponItem PickWeapon(WeaponItem weaponItem);
        WeaponItem PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null);
    }

    [Singleton]
    public class EquipmentPicker : IEquipmentPicker
    {
        private GameState GameState;
        private IXleSubMenu subMenu;

        public EquipmentPicker(GameState gameState,
            IXleSubMenu subMenu)
        {
            this.GameState = gameState;
            this.subMenu = subMenu;
        }

        public XleData Data { get; set; }

        public ArmorItem PickArmor(ArmorItem defaultItem)
        {
            return PickArmor(GameState, defaultItem);
        }
        public ArmorItem PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null)
        {
            MenuItemList theList = new MenuItemList();

            theList.Add("Nothing");
            theList.AddRange(state.Player.Armor.Select(x => x.NameWithQuality(Data)));

            int sel = subMenu.SubMenu("Pick Armor", state.Player.Armor.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Armor[sel - 1];
        }

        public WeaponItem PickWeapon(WeaponItem defaultItem)
        {
            return PickWeapon(GameState, defaultItem);
        }
        public WeaponItem PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null)
        {
            MenuItemList theList = new MenuItemList();

            theList.Add("Nothing");
            theList.AddRange(state.Player.Weapons.Select(x => x.NameWithQuality(Data)));

            int sel = subMenu.SubMenu("Pick Weapon", state.Player.Weapons.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Weapons[sel - 1];
        }
    }
}
