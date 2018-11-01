using AgateLib;
using System.Threading.Tasks;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;
using Xle.Services.MapLoad;

namespace Xle.Maps.Dungeons
{
    [Transient("DungeonClimb")]
    public class DungeonClimb : Climb
    {
        public IXleGameControl GameControl { get; set; }
        public IMapChanger MapChanger { get; set; }
        public IDungeonAdapter DungeonAdapter { get; set; }

        public override async Task Execute()
        {
            var tile = DungeonAdapter.TileAt(Player.X, Player.Y);
            switch (tile)
            {
                case DungeonTile.CeilingHole:
                    if (Player.DungeonLevel == 0)
                    {
                        await TextArea.PrintLine("\n\nYou climb out of the dungeon.");

                        await OnPlayerExitDungeon();

                        await GameControl.WaitAsync(1000);

                        await MapChanger.ReturnToPreviousMap();

                        return;
                    }
                    else
                    {
                        Player.DungeonLevel--;
                    }
                    break;

                case DungeonTile.FloorHole:
                    Player.DungeonLevel++;
                    break;

                default:
                    await FailMessage();
                    return;

            }

            await DungeonLevelText();
        }

        private Task OnPlayerExitDungeon() => DungeonAdapter.OnPlayerExitDungeon();

        private async Task DungeonLevelText()
        {
            DungeonAdapter.OnCurrentLevelChanged();
            DungeonAdapter.RevealTrapAt(Player.Location);

            await TextArea.PrintLine("\n\nYou are now at level " + (Player.DungeonLevel + 1).ToString() + ".", XleColor.White);
        }

    }
}
