
using AgateLib;
using Xle.Data;
using Xle.Services.Game;

namespace Xle.Services.XleSystem.Implementation
{
    public class XleStartup : IXleStartup
    {
        private IXleRunner runner;
        private IXleGameFactory gameFactory;
        private readonly IContentProvider content;
        private XleData data;
        private readonly IXleDataLoader dataLoader;
        private XleOptions options;

        public XleStartup(
            IXleRunner runner,
            IXleGameFactory xleGameFactory,
            XleSystemState systemState,
            IXleConsole console,
            IContentProvider content,
            XleOptions options,
            XleData data,
            IXleDataLoader dataLoader,
            ISoundMan soundMan)
        {
            this.runner = runner;
            this.gameFactory = xleGameFactory;
            this.content = content;
            this.data = data;
            this.dataLoader = dataLoader;
            this.options = options;

            console.Initialize();

            systemState.Data = data;

            LoadGameFile();

            systemState.Factory = xleGameFactory;

            systemState.Factory.LoadSurfaces();

            dataLoader.LoadDungeonMonsterSurfaces();
        }

        public void ProcessArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-debug":
                        options.EnableDebugMode = true;
                        break;
                }
            }
        }

        private void LoadGameFile()
        {
            dataLoader.LoadGameFile("Game.xml");
        }

    }
}
