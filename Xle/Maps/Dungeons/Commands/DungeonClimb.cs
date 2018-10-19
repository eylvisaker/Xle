using Xle.Maps.XleMapTypes;
using Xle.Services;
using Xle.Services.Commands.Implementation;
using Xle.Services.Game;
using Xle.Services.MapLoad;

namespace Xle.Maps.Dungeons
{
    [ServiceName("DungeonClimb")]
    public class DungeonClimb : Climb
    {
        public IXleGameControl GameControl { get; set; }
        public IMapChanger MapChanger { get; set; }
        public IDungeonAdapter DungeonAdapter { get; set; }

        public override void Execute()
        {
            var tile = DungeonAdapter.TileAt(Player.X, Player.Y);
            switch (tile)
            {
                case DungeonTile.CeilingHole:
                    if (Player.DungeonLevel == 0)
                    {
                        TextArea.PrintLine("\n\nYou climb out of the dungeon.");

                        OnPlayerExitDungeon();

                        GameControl.Wait(1000);

                        MapChanger.ReturnToPreviousMap();

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
                    FailMessage();
                    return;

            }

            DungeonLevelText();
        }

        private void OnPlayerExitDungeon()
        {
            DungeonAdapter.OnPlayerExitDungeon();
        }

        private void DungeonLevelText()
        {
            DungeonAdapter.OnCurrentLevelChanged();
            DungeonAdapter.RevealTrapAt(Player.Location);

            TextArea.PrintLine("\n\nYou are now at level " + (Player.DungeonLevel + 1).ToString() + ".", XleColor.White);
        }

    }
}
