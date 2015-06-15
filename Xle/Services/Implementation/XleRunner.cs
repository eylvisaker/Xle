﻿using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib.Legacy;
using ERY.Xle.Commands;
using ERY.Xle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle
{
    public class XleRunner : IXleRunner
    {
        private XleSystemState systemState;
        private IXleLegacyCore legacyCore;
        private ITitleScreenFactory titleScreenFactory;
        private ITextArea textArea;
        private IXleInput input;
        private GameState gameState;

        public XleRunner(
            IXleLegacyCore legacyCore,
            XleSystemState systemState,
            ITitleScreenFactory titleScreenFactory,
            ITextArea textArea,
            IXleInput input,
            GameState gameState)
        {
            this.legacyCore = legacyCore;
            this.systemState = systemState;
            this.titleScreenFactory = titleScreenFactory;
            this.textArea = textArea;
            this.input = input;
            this.gameState = gameState;
        }

        public void Run(IXleGameFactory factory)
        {
            try
            {
                if (factory == null) throw new ArgumentNullException();

                systemState.Factory = factory;

                AgateLib.Core.ErrorReporting.CrossPlatformDebugLevel = CrossPlatformDebugLevel.Exception;

                var wind = Display.CurrentWindow;
                var coords = Display.Coordinates;
                int height = coords.Height - systemState.WindowBorderSize.Height * 2;
                int width = (int)(320 / 200.0 * height);

                systemState.WindowBorderSize = new AgateLib.Geometry.Size((coords.Width - width) / 2,
                    systemState.WindowBorderSize.Height);

                SoundMan.Load();

                systemState.Factory.LoadSurfaces();
                legacyCore.Data.LoadDungeonMonsterSurfaces();

                systemState.Factory.Font.InterpolationHint = InterpolationMode.Fastest;
                IXleTitleScreen titleScreen;

                do
                {
                    gameState.Initialize();

                    titleScreen = titleScreenFactory.CreateTitleScreen();
                    titleScreen.Run();
                    systemState.ReturnToTitle = false;

                    RunGame(titleScreen.Player);

                } while (titleScreen.Player != null);
            }
            catch (MainWindowClosedException)
            { }
        }

        private void RunGame(Player thePlayer)
        {
            if (thePlayer == null)
                return;

            textArea.Clear();

            gameState.Initialize();

            gameState.Player = thePlayer;
            gameState.Commands = new CommandList(gameState);

            gameState.Map = LoadMap(gameState.Player.MapID);
            gameState.Map.OnLoad(gameState.Player);

            systemState.Factory.SetGameSpeed(gameState, thePlayer.Gamespeed);

            SetTilesAndCommands();

            XleCore.Renderer.FontColor = gameState.Map.ColorScheme.TextColor;
            gameState.Commands.Prompt();
            input.AcceptKey = true;

            while (Display.CurrentWindow.IsClosed == false && systemState.ReturnToTitle == false)
            {
                Redraw();
            }

        }

        private void Redraw()
        {
            legacyCore.Redraw();
        }

        private void SetTilesAndCommands()
        {
            legacyCore.SetTilesAndCommands();
        }

        private Maps.XleMap LoadMap(int p)
        {
            return legacyCore.LoadMap(p);
        }

    }
}
