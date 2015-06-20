using System;
using System.Windows.Input;

using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.Geometry;
using AgateLib.Quality;

using ERY.Xle.Data;
using ERY.Xle.Rendering;
using ERY.Xle.Services.Implementation.Commands;

namespace ERY.Xle.Services.Implementation
{
    public class XleRunner : IXleRunner
    {
        private XleSystemState systemState;
        private ITitleScreenFactory titleScreenFactory;
        private ITextArea textArea;
        private IXleInput input;
        private GameState gameState;
        private ICommandExecutor commandExecutor;
        private IMapLoader mapLoader;
        private IXleScreen screen;
        private IXleGameControl gameControl;
        private IMapChanger mapChanger;

        public XleRunner(
            XleSystemState systemState,
            ITitleScreenFactory titleScreenFactory,
            ITextArea textArea,
            IXleInput input,
            IXleScreen screen,
            ICommandExecutor commandExecutor,
            IMapLoader mapLoader,
            IXleGameControl gameControl,
            IMapChanger mapChanger,
            GameState gameState)
        {
            this.screen = screen;
            this.systemState = systemState;
            this.titleScreenFactory = titleScreenFactory;
            this.textArea = textArea;
            this.input = input;
            this.commandExecutor = commandExecutor;
            this.mapLoader = mapLoader;
            this.gameControl = gameControl;
            this.mapChanger = mapChanger;
            this.gameState = gameState;
        }

        public void Run(IXleGameFactory factory)
        {
            Condition.RequireArgumentNotNull(factory, "factory");

            try
            {
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

            gameState.Initialize(thePlayer);
            systemState.Factory.SetGameSpeed(gameState, thePlayer.Gamespeed);

            var map = mapLoader.LoadMap(gameState.Player.MapID);
            mapChanger.SetMap(map);

            input.AcceptKey = true;

            textArea.Clear();
            commandExecutor.Prompt();

            gameControl.RunRedrawLoop();
        }

    }
}
