using FluentAssertions;
using Microsoft.Xna.Framework;
using Moq;
using System.Threading.Tasks;
using Xle.Services.Menus;
using Xunit;

namespace Xle.ServiceTests
{
    public class EquipmentPickerTest : XleTest
    {
        private EquipmentPicker picker;
        private int selectArmorIndex = -1;
        private int selectWeaponIndex = -1;

        public EquipmentPickerTest()
        {
            picker = new EquipmentPicker(GameState, Services.SubMenu.Object, Services.Data);

            Services.SubMenu.Setup(x => x.SubMenu(
                "Pick Armor", 0, It.IsAny<MenuItemList>(), It.IsAny<Color>()))
                .Returns<string, int, MenuItemList, Color>(
                    (a, b, c, d) =>
                    {
                        if (selectArmorIndex < 0)
                            return Task.FromResult(c.Count - 1);
                        else
                            return Task.FromResult(selectArmorIndex);
                    });

            Services.SubMenu.Setup(x => x.SubMenu(
                "Pick Weapon", 0, It.IsAny<MenuItemList>(), It.IsAny<Color>()))
                .Returns<string, int, MenuItemList, Color>(
                    (a, b, c, d) =>
                    {
                        if (selectWeaponIndex < 0)
                            return Task.FromResult(c.Count - 1);
                        else
                            return Task.FromResult(selectWeaponIndex);
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
        public async Task PickArmor()
        {
            Player.Armor.Add(new ArmorItem { ID = 1, Quality = 2 });
            Player.Armor.Add(new ArmorItem { ID = 2, Quality = 0 });
            Player.Armor.Add(new ArmorItem { ID = 3, Quality = 4 });
            selectArmorIndex = 2;

            var sel = await picker.PickArmor(null);

            sel.Should().BeOfType<ArmorItem>();
            sel.ID.Should().Be(2);
            sel.Quality.Should().Be(0);
        }

        [Fact]
        public async Task PickWeapon()
        {
            Player.Weapons.Add(new WeaponItem { ID = 1, Quality = 2 });
            Player.Weapons.Add(new WeaponItem { ID = 2, Quality = 0 });
            Player.Weapons.Add(new WeaponItem { ID = 3, Quality = 4 });
            selectWeaponIndex = 2;

            var sel = await picker.PickWeapon(null);

            sel.Should().BeOfType<WeaponItem>();
            sel.ID.Should().Be(2);
            sel.Quality.Should().Be(0);
        }
    }
}
