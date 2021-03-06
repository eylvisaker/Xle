﻿using AgateLib;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.Maps;
using Xle.Commands;
using Xle.Menus;
using Xle.Rendering;
using Xle.ScreenModel;

namespace Xle.MapLoad
{
    public interface IMapChanger
    {
        void SetMap(IMapExtender map);

        Task ChangeMap(int mapId, int entryPoint);
        Task ChangeMap(int mapId, Point targetPoint);

        Task ReturnToPreviousMap();
    }

    [Singleton]
    public class MapChanger : IMapChanger
    {
        private GameState gameState;
        private IXleScreen screen;
        private ITextArea textArea;
        private ICommandList commands;
        private IMapLoader mapLoader;
        private IXleImages images;
        private readonly IMuseumCoinSale museumCoinSale;

        public MapChanger(
            GameState gameState,
            IXleScreen screen,
            IXleImages images,
            ITextArea textArea,
            ICommandList commands,
            IMapLoader mapLoader,
            IMuseumCoinSale museumCoinSale)
        {
            this.gameState = gameState;
            this.screen = screen;
            this.images = images;
            this.textArea = textArea;
            this.commands = commands;
            this.mapLoader = mapLoader;
            this.museumCoinSale = museumCoinSale;
        }

        private Player Player { get { return gameState.Player; } }

        public Task ChangeMap(int mapId, int entryPoint)
        {
            return ChangeMapCore(mapId, entryPoint, 0, 0);
        }

        public Task ChangeMap(int mapId, Point targetPoint)
        {
            return ChangeMapCore(mapId, -1, targetPoint.X, targetPoint.Y);
        }

        private async Task ChangeMapCore(int mMapID, int targetEntryPoint, int targetX, int targetY)
        {
            if (gameState.Map == null)
            {
                Player.MapID = mMapID;
                return;
            }

            var saveMap = gameState.MapExtender;
            var saveX = Player.X;
            var saveY = Player.Y;

            SetReturnLocationIfOutside();

            if (saveMap.MapID == mMapID || mMapID == 0)
            {
                var ep = DetermineEntryPoint(targetEntryPoint, targetX, targetY);

                MoveToEntryPoint(ep);

                return;
            }

            try
            {
                gameState.MapExtender = mapLoader.LoadMap(mMapID);
                Player.MapID = mMapID;

                TransferAngryStateIfNeeded(saveMap);

                textArea.Clear();

                targetEntryPoint = ModifyTargetEntryPoint(targetEntryPoint);

                var ep = DetermineEntryPoint(targetEntryPoint, targetX, targetY);

                MoveToEntryPoint(ep);

                SetTilesAndCommands();
                gameState.MapExtender.OnLoad();
                museumCoinSale.ResetCoinOffers();

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

            await gameState.MapExtender.OnAfterEntry();
        }

        private void TransferAngryStateIfNeeded(IMapExtender saveMap)
        {
            // Preserve guard anger state for castle 
            // when changing levels
            if (gameState.Map.GetType() == saveMap.GetType() &&
                gameState.Map.Guards != null)
            {
                gameState.Map.Guards.IsAngry = saveMap.TheMap.Guards.IsAngry;
            }
        }

        private int ModifyTargetEntryPoint(int targetEntryPoint)
        {
            if (targetEntryPoint >= 0)
            {
                MapEntryParams entryParams = new MapEntryParams { EntryPoint = targetEntryPoint };

                gameState.MapExtender.ModifyEntryPoint(entryParams);

                targetEntryPoint = entryParams.EntryPoint;
            }
            return targetEntryPoint;
        }

        private EntryPoint DetermineEntryPoint(int targetEntryPoint, int targetX, int targetY)
        {
            EntryPoint ep;

            if (targetEntryPoint >= gameState.Map.EntryPoints.Count)
            {
                throw new InvalidOperationException("Failed to find entry point " + targetEntryPoint);
            }

            if (targetEntryPoint >= 0)
            {
                ep = gameState.Map.EntryPoints[targetEntryPoint];
            }
            else
            {
                ep = new EntryPoint
                {
                    Location = new Point(targetX, targetY),
                    DungeonLevel = Player.DungeonLevel,
                    Facing = Player.FaceDirection
                };
            }
            return ep;
        }

        private void MoveToEntryPoint(EntryPoint ep)
        {
            Player.X = ep.Location.X;
            Player.Y = ep.Location.Y;
            Player.DungeonLevel = ep.DungeonLevel;

            if (ep.Facing != Direction.None)
            {
                Player.FaceDirection = ep.Facing;
            }
        }
        private void MoveToLocation(int targetEntryPoint, int targetX, int targetY)
        {
        }

        private void SetReturnLocationIfOutside()
        {
            if (gameState.Map.StoreReturnLocation)
            {
                Player.SetReturnLocation(Player.MapID, Player.X, Player.Y, Direction.South);
            }
        }


        public void SetMap(IMapExtender map)
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

        public async Task ReturnToPreviousMap()
        {
            await ChangeMap(Player.returnMap, new Point(Player.returnX, Player.returnY));

            Player.FaceDirection = Player.returnFacing;
        }

    }
}
