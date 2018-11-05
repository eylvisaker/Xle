using AgateLib;
using System.Linq;
using System.Threading.Tasks;
using Xle.Rendering;
using Xle.XleEventTypes.Extenders;

namespace Xle.Ancients.MapExtenders.Fortress.FirstArea
{
    [Transient("ArmorBox")]
    public class ArmorBox : TreasureChestExtender
    {
        public IXleRenderer Renderer { get; set; }

        public override async Task<bool> Open()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();

            if (TheEvent.Closed)
            {
                await TextArea.PrintLine("you see yellow guard");
                await TextArea.PrintLine("armor in the bottom.");

                PlayOpenChestSound();
                TheEvent.SetOpenTilesOnMap(GameState.Map);

                await GameControl.WaitAsync(GameState.GameSpeed.CastleOpenChestSoundTime);
            }
            else
            {
                await TextArea.PrintLine("box open already.");
            }

            return true;
        }

        public override async Task<bool> Take()
        {
            if (TheEvent.Closed)
                return await base.Take();

            GameState.Map.Guards.IsAngry = false;

            Player.RenderColor = XleColor.Yellow;

            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("you put on armor.");

            await GameControl.WaitAsync(1000);

            Player.AddArmor(4, 3);
            if (Player.CurrentArmor.ID == 0)
                Player.CurrentArmor = Player.Armor.Last();

            return true;
        }
    }
}
