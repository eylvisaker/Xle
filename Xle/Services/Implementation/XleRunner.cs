using System;
using System.Windows.Input;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;

using ERY.Xle.Rendering;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Services.Implementation
{
    public class XleRunner : IXleRunner
    {
        private XleSystemState systemState;
        private IXleLegacyCore legacyCore;
        private ITitleScreenFactory titleScreenFactory;
        private ITextArea textArea;
        private IXleInput input;
        private IXleRenderer renderer;
        private GameState gameState;
        private ICommandList commands;

        public XleRunner(
            IXleLegacyCore legacyCore,
            XleSystemState systemState,
            ITitleScreenFactory titleScreenFactory,
            ITextArea textArea,
            IXleInput input,
            IXleRenderer renderer,
            ICommandList commands,
            GameState gameState)
        {
            this.legacyCore = legacyCore;
            this.systemState = systemState;
            this.titleScreenFactory = titleScreenFactory;
            this.textArea = textArea;
            this.input = input;
            this.renderer = renderer;
            this.commands = commands;
            this.gameState = gameState;
        }

        public void Run(IXleGameFactory factory)
        {
            try
            {
                if (factory == null) throw new ArgumentNullException();

                systemState.Factory = factory;

                AgateLib.Core.ErrorReporting.CrossPlatformDebugLevel = CrossPlatformDebugLevel.Exception;

                Rectangle coords = renderer.Coordinates;
                int height = coords.Height - systemState.WindowBorderSize.Height * 2;
                int width = (int)(320 / 200.0 * height);

                systemState.WindowBorderSize = new Size(
                    (coords.Width - width) / 2,
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

            gameState.Map = LoadMap(gameState.Player.MapID);
            gameState.Map.OnLoad(gameState.Player);

            systemState.Factory.SetGameSpeed(gameState, thePlayer.Gamespeed);

            SetTilesAndCommands();

            renderer.FontColor = gameState.Map.ColorScheme.TextColor;
            commands.Prompt();
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
