using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Geometry;

using ERY.Xle.Maps;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation
{
    public class MapChanger : IMapChanger
    {
        private GameState gameState;
        private ITextArea textArea;
        private ICommandList commands;
        private IMapLoader mapLoader;
        private IXleRenderer renderer;

        public MapChanger(
            GameState gameState,
            ITextArea textArea,
            ICommandList commands,
            IMapLoader mapLoader,
            IXleRenderer renderer)
        {
            this.gameState = gameState;
            this.textArea = textArea;
            this.commands = commands;
            this.mapLoader = mapLoader;
            this.renderer = renderer;
        }

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

            var saveMap = gameState.Map;
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
                    gameState.Map = mapLoader.LoadMap(mMapID);
                    player.MapID = mMapID;

                    if (gameState.Map.GetType() == saveMap.GetType() &&
                        gameState.Map.Guards != null)
                    {
                        gameState.Map.Guards.IsAngry = saveMap.Guards.IsAngry;
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
                    gameState.Map.OnLoad(player);
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
                System.Diagnostics.Debug.Print(e.StackTrace);

                player.MapID = saveMap.MapID;
                gameState.Map = saveMap;
                player.X = saveX;
                player.Y = saveY;

                throw;
            }

            if (actualChangeMap)
                gameState.MapExtender.OnAfterEntry(gameState);
        }


        public void SetMap(XleMap map)
        {
            gameState.Map = map;
            gameState.Map.OnLoad(gameState.Player);

            SetTilesAndCommands();

            renderer.FontColor = gameState.Map.ColorScheme.TextColor;
        }

        private void SetTilesAndCommands()
        {
            commands.Items.Clear();

            gameState.MapExtender.SetCommands(commands);
            commands.ResetCurrentCommand();

            renderer.LoadTiles(gameState.Map.TileImage);
        }

    }
}
