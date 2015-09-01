using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.MapLoad;

namespace ERY.Xle.Maps.Dungeons
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
