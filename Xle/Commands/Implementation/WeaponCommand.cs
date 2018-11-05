using AgateLib;
using System.Threading.Tasks;
using Xle.Menus;
using Xle.ScreenModel;

namespace Xle.Commands.Implementation
{
    [Transient]
    public class WeaponCommand : Command
    {
        private ITextArea textArea;
        private IEquipmentPicker equipmentPicker;

        public WeaponCommand(
            ITextArea textArea,
            IEquipmentPicker equipmentPicker)
        {
            this.textArea = textArea;
            this.equipmentPicker = equipmentPicker;
        }

        public override string Name
        {
            get { return "Weapon"; }
        }

        public override async Task Execute()
        {
            await textArea.PrintLine("-choose above", XleColor.Cyan);

            Player.CurrentWeapon = await equipmentPicker.PickWeapon(Player.CurrentWeapon);
        }
    }
}
