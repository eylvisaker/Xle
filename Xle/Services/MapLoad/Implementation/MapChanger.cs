using System;

using AgateLib.Geometry;

using ERY.Xle.Maps.Extenders;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Commands;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Services.MapLoad.Implementation
{
    public class MapChanger : IMapChanger
    {
        private GameState gameState;
        private IXleScreen screen;
        private ITextArea textArea;
        private ICommandList commands;
        private IMapLoader mapLoader;
        private IXleImages images;

        public MapChanger(
            GameState gameState,
            IXleScreen screen,
            IXleImages images,
            ITextArea textArea,
            ICommandList commands,
            IMapLoader mapLoader)
        {
            this.gameState = gameState;
            this.screen = screen;
            this.images = images;
            this.textArea = textArea;
            this.commands = commands;
            this.mapLoader = mapLoader;
        }

        Player Player { get { return gameState.Player; } }

        public void ChangeMap(Player player, int mapId, int entryPoint)
        {
            ChangeMapCore(player, mapId, entryPoint, 0, 0);
        }

        public void ChangeMap(Player player, int mapId, Point targetPoint)
        {
            ChangeMapCore(player, mapId, -1, targetPoint.X, targetPoint.Y);
        }

        void ChangeMapCore(Player player, int mMapID, int targetEntryPoint, int targetX, int targetY)
        {
            if (gameState.Map == null)
            {
                player.MapID = mMapID;
                return;
            }

            var saveMap = gameState.MapExtender;
            var saveX = player.X;
            var saveY = player.Y;

            if (gameState.Map is Outside)
            {
                player.SetReturnLocation(player.MapID, player.X, player.Y, Direction.South);
            }

            bool actualChangeMap = saveMap.MapID != mMapID;

            if (mMapID == 0)
                actualChangeMap = false;

            try
            {
                if (actualChangeMap)
                {
                    gameState.MapExtender = mapLoader.LoadMap(mMapID);
                    player.MapID = mMapID;

                    if (gameState.Map.GetType() == saveMap.GetType() &&
                        gameState.Map.Guards != null)
                    {
                        gameState.Map.Guards.IsAngry = saveMap.TheMap.Guards.IsAngry;
                    }

                    textArea.Clear();
                }

                if (targetEntryPoint < 0 ||
                    targetEntryPoint >= gameState.Map.EntryPoints.Count)
                {
                    player.X = targetX;
                    player.Y = targetY;

                    if (targetEntryPoint >= 0)
                    {
                        textArea.PrintLine("Failed to find entry point " + targetEntryPoint.ToString(), XleColor.Yellow);
                        textArea.PrintLine();
                    }
                }
                else
                {
                    if (actualChangeMap)
                        gameState.MapExtender.OnBeforeEntry(gameState, ref targetEntryPoint);

                    var ep = gameState.Map.EntryPoints[targetEntryPoint];

                    player.X = ep.Location.X;
                    player.Y = ep.Location.Y;
                    player.DungeonLevel = ep.DungeonLevel;

                    if (ep.Facing != Direction.None)
                    {
                        player.FaceDirection = ep.Facing;
                    }
                }

                SetTilesAndCommands();

                if (actualChangeMap)
                {
                    gameState.MapExtender.OnLoad(gameState);
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
                System.Diagnostics.Debug.Print(e.StackTrace);

                player.MapID = saveMap.MapID;
                gameState.MapExtender = saveMap;
                player.X = saveX;
                player.Y = saveY;

                throw;
            }

            if (actualChangeMap)
                gameState.MapExtender.OnAfterEntry(gameState);
        }


        public void SetMap(MapExtender map)
        {
            gameState.MapExtender = map;
            gameState.MapExtender.OnLoad(gameState);

            SetTilesAndCommands();

            screen.FontColor = gameState.Map.ColorScheme.TextColor;
        }

        private void SetTilesAndCommands()
        {
            commands.Items.Clear();

            gameState.MapExtender.SetCommands(commands);
            commands.ResetCurrentCommand();

            images.LoadTiles(gameState.Map.TileImage);
        }



        public void ReturnToPreviousMap()
        {
            ChangeMap(Player, Player.returnMap, new Point(Player.returnX, Player.returnY));

            Player.FaceDirection = Player.returnFacing;
        }

    }
}
