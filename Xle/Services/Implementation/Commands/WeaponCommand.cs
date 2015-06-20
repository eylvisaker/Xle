namespace ERY.Xle.Services.Implementation.Commands
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
        public override void Execute(GameState state)
        {
            textArea.PrintLine("-choose above", XleColor.Cyan);

            state.Player.CurrentWeapon = equipmentPicker.PickWeapon(state.Player.CurrentWeapon);
        }
    }
}
