using AgateLib;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.MapLoad;

namespace Xle.Blacksilver.MapExtenders.Citadel.EventExtenders
{
    [Transient("Mantrek")]
    public class Mantrek : LobEvent
    {
        public IMapChanger MapChanger { get; set; }

        public override async Task<bool> Speak()
        {
            if (Story.MantrekKilled)
                return false;

            await BegForLife();
            return true;
        }

        private async Task BegForLife()
        {
            await TextArea.PrintLine();
            await TextArea.PrintLine();
            await TextArea.PrintLine("Spare me and I shall");
            await TextArea.PrintLine("give you the staff.");
            await TextArea.PrintLine();
            await QuickMenu.QuickMenuYesNo();

            await MapChanger.ChangeMap(Player.MapID, 1);

            Story.MantrekKilled = true;

            EraseMantrek(Map);
        }

        public void EraseMantrek(XleMap map)
        {
            for (int j = 4; j <= 12; j++)
            {
                for (int i = 28; i <= 36; i++)
                {
                    if (i < 36)
                    {
                        map[i, j] = 37;
                    }
                    else
                    {
                        map[i, j] = 263;
                    }
                }
            }
        }

    }
}
