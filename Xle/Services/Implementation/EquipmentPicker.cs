using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ERY.Xle.Data;

namespace ERY.Xle.Services.Implementation
{
    public class EquipmentPicker : IEquipmentPicker
    {
        private GameState GameState;
        private IXleMenu menu;

        public EquipmentPicker(GameState gameState, 
            IXleMenu menu)
        {
            this.GameState = gameState;
            this.menu = menu;
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

            int sel = menu.SubMenu("Pick Armor", state.Player.Armor.IndexOf(defaultItem) + 1,
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

            int sel = menu.SubMenu("Pick Weapon", state.Player.Weapons.IndexOf(defaultItem) + 1,
                theList, backColor ?? XleColor.Black);

            if (sel == 0)
                return null;
            else
                return state.Player.Weapons[sel - 1];
        }
    }
}
