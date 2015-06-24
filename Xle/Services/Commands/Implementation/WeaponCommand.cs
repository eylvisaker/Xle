using ERY.Xle.Services.Menus;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.Commands.Implementation
{
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
        public override void Execute()
        {
            textArea.PrintLine("-choose above", XleColor.Cyan);

            Player.CurrentWeapon = equipmentPicker.PickWeapon(Player.CurrentWeapon);
        }
    }
}
