using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xle.Data;

namespace Xle.Services.Menus
{
    public interface IEquipmentPicker
    {
        Task<ArmorItem> PickArmor(ArmorItem armorItem);
        Task<ArmorItem> PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null);

        Task<WeaponItem> PickWeapon(WeaponItem weaponItem);
        Task<WeaponItem> PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null);
    }

    [Singleton]
    public class EquipmentPicker : IEquipmentPicker
    {
        private GameState GameState;
        private IXleSubMenu subMenu;

        public EquipmentPicker(GameState gameState,
            IXleSubMenu subMenu, XleData data)
        {
            this.GameState = gameState;
            this.subMenu = subMenu;
            Data = data;
        }

        public XleData Data { get; set; }

        public Task<ArmorItem> PickArmor(ArmorItem defaultItem)
        {
            return PickArmor(GameState, defaultItem);
        }

        public async Task<ArmorItem> PickArmor(GameState state, ArmorItem defaultItem, Color? backColor = null)
        {
            MenuItemList theList = new MenuItemList();

            theList.Add("Nothing");
            theList.AddRange(state.Player.Armor.Select(x => x.NameWithQuality(Data)));

            int sel = await subMenu.SubMenu("Pick Armor", state.Player.Armor.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Armor[sel - 1];
        }

        public Task<WeaponItem> PickWeapon(WeaponItem defaultItem)
        {
            return PickWeapon(GameState, defaultItem);
        }

        public async Task<WeaponItem> PickWeapon(GameState state, WeaponItem defaultItem, Color? backColor = null)
        {
            MenuItemList theList = new MenuItemList();

            theList.Add("Nothing");
            theList.AddRange(state.Player.Weapons.Select(x => x.NameWithQuality(Data)));

            int sel = await subMenu.SubMenu("Pick Weapon", state.Player.Weapons.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Weapons[sel - 1];
        }
    }
}
