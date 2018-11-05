using AgateLib;
using AgateLib.Quality;

using Xle.Commands;
using Xle.Game;
using Xle.MapLoad;
using Xle.ScreenModel;

namespace Xle.XleSystem
{
    public interface IXleRunner
    {
        void Run(Player player);
    }

    [Singleton]
    public class XleRunner : IXleRunner
    {
        private XleSystemState systemState;
        private ITextArea textArea;
        private IXleInput input;
        private GameState gameState;
        private ICommandExecutor commandExecutor;
        private IMapLoader mapLoader;
        private IMapChanger mapChanger;
        private readonly IXleGameFactory gameFactory;

        public XleRunner(
            XleSystemState systemState,
            ITextArea textArea,
            IXleInput input,
            ICommandExecutor commandExecutor,
            IMapLoader mapLoader,
            IMapChanger mapChanger,
            IXleGameFactory gameFactory,
            GameState gameState)
        {
            this.systemState = systemState;
            this.textArea = textArea;
            this.input = input;
            this.commandExecutor = commandExecutor;
            this.mapLoader = mapLoader;
            this.mapChanger = mapChanger;
            this.gameFactory = gameFactory;
            this.gameState = gameState;
        }

        public void Run(Player thePlayer)
        {
            if (thePlayer == null)
                return;

            gameState.Initialize(thePlayer);

            systemState.Factory = gameFactory;
            systemState.Factory.LoadSurfaces();
            systemState.Factory.SetGameSpeed(gameState, thePlayer.Gamespeed);

            var map = mapLoader.LoadMap(gameState.Player.MapID);
            mapChanger.SetMap(map);

            textArea.Clear();
            commandExecutor.Prompt();
        }
    }
}
