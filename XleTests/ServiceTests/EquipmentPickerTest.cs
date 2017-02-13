using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.DisplayLib;
using AgateLib.Mathematics.Geometry;
using AgateLib.UserInterface.Widgets;

using ERY.Xle;
using ERY.Xle.Services.Menus.Implementation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace ERY.XleTests.ServiceTests
{
	[TestClass]
	public class EquipmentPickerTest : XleTest
	{
		EquipmentPicker picker;
		int selectArmorIndex = -1;
		int selectWeaponIndex = -1;

		[TestInitialize]
		public void Initialize()
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

		[TestMethod]
		public void PickArmorNothingAvailable()
		{
			Assert.IsNull(picker.PickArmor(null));
		}

		[TestMethod]
		public void PickWeaponNothingAvailable()
		{
			Assert.IsNull(picker.PickWeapon(null));
		}

		[TestMethod]
		public void PickArmor()
		{
			Player.Armor.Add(new ArmorItem { ID = 1, Quality = 2 });
			Player.Armor.Add(new ArmorItem { ID = 2, Quality = 0 });
			Player.Armor.Add(new ArmorItem { ID = 3, Quality = 4 });
			selectArmorIndex = 2;

			var sel = picker.PickArmor(null);

			Assert.IsInstanceOfType(sel, typeof(ArmorItem));
			Assert.AreEqual(2, sel.ID);
			Assert.AreEqual(0, sel.Quality);
		}

		[TestMethod]
		public void PickWeapon()
		{
			Player.Weapons.Add(new WeaponItem { ID = 1, Quality = 2 });
			Player.Weapons.Add(new WeaponItem { ID = 2, Quality = 0 });
			Player.Weapons.Add(new WeaponItem { ID = 3, Quality = 4 });
			selectWeaponIndex = 2;

			var sel = picker.PickWeapon(null);

			Assert.IsInstanceOfType(sel, typeof(WeaponItem));
			Assert.AreEqual(2, sel.ID);
			Assert.AreEqual(0, sel.Quality);
		}
	}
}
