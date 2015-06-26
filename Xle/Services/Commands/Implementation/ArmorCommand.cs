﻿using ERY.Xle.Services.Menus;

namespace ERY.Xle.Services.Commands.Implementation
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

        public override void Execute()
        {
            TextArea.PrintLine("-choose above", XleColor.Cyan);

            Player.CurrentArmor = equipmentPicker.PickArmor(Player.CurrentArmor);
        }
    }
}