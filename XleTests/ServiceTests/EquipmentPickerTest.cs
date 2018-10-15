
using ERY.Xle;
using ERY.Xle.Services.Menus.Implementation;
using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using Xunit;

namespace ERY.XleTests.ServiceTests
{
    public class EquipmentPickerTest : XleTest
    {
        private EquipmentPicker picker;
        private int selectArmorIndex = -1;
        private int selectWeaponIndex = -1;

        public EquipmentPickerTest()
        {
            picker = new EquipmentPicker(GameState, Services.SubMenu.Object) { Data = Services.Data };

            Services.SubMenu.Setup(x => x.SubMenu(
                "Pick Armor", 0, It.IsAny<MenuItemList>(), It.IsAny<Color>()))
                .Returns<string, int, MenuItemList, Color>(
                    (a, b, c, d) =>
                    {
                        if (selectArmorIndex < 0)
                            return c.Count - 1;
                        else
                            return selectArmorIndex;
                    });

            Services.SubMenu.Setup(x => x.SubMenu(
                "Pick Weapon", 0, It.IsAny<MenuItemList>(), It.IsAny<Color>()))
                .Returns<string, int, MenuItemList, Color>(
                    (a, b, c, d) =>
                    {
                        if (selectWeaponIndex < 0)
                            return c.Count - 1;
                        else
                            return selectWeaponIndex;
                    });

        }

        [Fact]
        public void PickArmorNothingAvailable()
        {
            picker.PickArmor(null).Should().BeNull();
        }

        [Fact]
        public void PickWeaponNothingAvailable()
        {
            picker.PickWeapon(null).Should().BeNull();
        }

        [Fact]
        public void PickArmor()
        {
            Player.Armor.Add(new ArmorItem { ID = 1, Quality = 2 });
            Player.Armor.Add(new ArmorItem { ID = 2, Quality = 0 });
            Player.Armor.Add(new ArmorItem { ID = 3, Quality = 4 });
            selectArmorIndex = 2;

            var sel = picker.PickArmor(null);

            sel.Should().BeOfType<ArmorItem>();
            sel.ID.Should().Be(2);
            sel.Quality.Should().Be(0);
        }

        [Fact]
        public void PickWeapon()
        {
            Player.Weapons.Add(new WeaponItem { ID = 1, Quality = 2 });
            Player.Weapons.Add(new WeaponItem { ID = 2, Quality = 0 });
            Player.Weapons.Add(new WeaponItem { ID = 3, Quality = 4 });
            selectWeaponIndex = 2;

            var sel = picker.PickWeapon(null);

            sel.Should().BeOfType<WeaponItem>();
            sel.ID.Should().Be(2);
            sel.Quality.Should().Be(0);
        }
    }
}
