namespace ERY.Xle.Services.Implementation.Commands
{
    public class ArmorCommand : Command
    {
        private IEquipmentPicker equipmentPicker;

        public ArmorCommand(IEquipmentPicker equipmentPicker)
        {
            this.equipmentPicker = equipmentPicker;
        }

        public override string Name
        {
            get { return "Armor"; }
        }
        public override void Execute(GameState state)
        {
            TextArea.PrintLine("-choose above", XleColor.Cyan);

            GameState.Player.CurrentArmor = equipmentPicker.PickArmor(GameState.Player.CurrentArmor);
        }
    }
}
