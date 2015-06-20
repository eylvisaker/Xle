namespace ERY.Xle.Services.Implementation.Commands
{
    public class ArmorCommand : Command
    {
        private ITextArea textArea;
        private IEquipmentPicker equipmentPicker;

        public ArmorCommand(
            ITextArea textArea,
            IEquipmentPicker equipmentPicker)
        {
            this.textArea = textArea;
            this.equipmentPicker = equipmentPicker;
        }

        public override string Name
        {
            get { return "Armor"; }
        }
        public override void Execute(GameState state)
        {
            textArea.PrintLine("-choose above", XleColor.Cyan);

            state.Player.CurrentArmor = equipmentPicker.PickArmor(state.Player.CurrentArmor);
        }
    }
}
