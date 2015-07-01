using System;

using AgateLib.Geometry;

using ERY.Xle.Maps;
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

        public void ChangeMap(int mapId, int entryPoint)
        {
            ChangeMapCore(mapId, entryPoint, 0, 0);
        }

        public void ChangeMap(int mapId, Point targetPoint)
        {
            ChangeMapCore(mapId, -1, targetPoint.X, targetPoint.Y);
        }

        void ChangeMapCore(int mMapID, int targetEntryPoint, int targetX, int targetY)
        {
            if (gameState.Map == null)
            {
                Player.MapID = mMapID;
                return;
            }

            var saveMap = gameState.MapExtender;
            var saveX = Player.X;
            var saveY = Player.Y;

            if (gameState.Map is Outside)
            {
                Player.SetReturnLocation(Player.MapID, Player.X, Player.Y, Direction.South);
            }

            bool actualChangeMap = saveMap.MapID != mMapID;

            if (mMapID == 0)
                actualChangeMap = false;

            try
            {
                if (actualChangeMap)
                {
                    gameState.MapExtender = mapLoader.LoadMap(mMapID);
                    Player.MapID = mMapID;

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
                    Player.X = targetX;
                    Player.Y = targetY;

                    if (targetEntryPoint >= 0)
                    {
                        textArea.PrintLine("Failed to find entry point " + targetEntryPoint.ToString(), XleColor.Yellow);
                        textArea.PrintLine();
                    }
                }
                else
                {
                    if (actualChangeMap)
                        gameState.MapExtender.OnBeforeEntry(ref targetEntryPoint);

                    var ep = gameState.Map.EntryPoints[targetEntryPoint];

                    Player.X = ep.Location.X;
                    Player.Y = ep.Location.Y;
                    Player.DungeonLevel = ep.DungeonLevel;

                    if (ep.Facing != Direction.None)
                    {
                        Player.FaceDirection = ep.Facing;
                    }
                }

                SetTilesAndCommands();

                if (actualChangeMap)
                {
                    gameState.MapExtender.OnLoad();
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.ToString());

                Player.MapID = saveMap.MapID;
                gameState.MapExtender = saveMap;
                Player.X = saveX;
                Player.Y = saveY;

                throw;
            }

            if (actualChangeMap)
                gameState.MapExtender.OnAfterEntry();
        }


        public void SetMap(MapExtender map)
        {
            gameState.MapExtender = map;
            gameState.MapExtender.OnLoad();

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
            ChangeMap(Player.returnMap, new Point(Player.returnX, Player.returnY));

            Player.FaceDirection = Player.returnFacing;
        }

    }
}
