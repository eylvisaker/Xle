using AgateLib;
using System.Threading.Tasks;
using Xle.Menus;

namespace Xle.Commands.Implementation
{
    [Transient]
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

        public override async Task Execute()
        {
            await TextArea.PrintLine("-choose above", XleColor.Cyan);

            Player.CurrentArmor = await equipmentPicker.PickArmor(Player.CurrentArmor);
        }
    }
}
