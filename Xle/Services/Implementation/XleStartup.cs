using AgateLib;
using AgateLib.DisplayLib;

using ERY.Xle.Data;

namespace ERY.Xle.Services.Implementation
{
    public class XleStartup : IXleStartup
    {
        IXleRunner runner;
        IXleGameFactory gameFactory;
        XleData data;

        public XleStartup(
            IXleRunner runner, 
            IXleGameFactory xleGameFactory, 
            XleSystemState systemState, 
            IXleConsole console, 
            XleData data,
            ISoundMan soundMan)
        {
            this.runner = runner;
            this.gameFactory = xleGameFactory;
            this.data = data;
            systemState.Data = data;

            systemState.Factory = xleGameFactory;

            systemState.Factory.LoadSurfaces();
            data.LoadDungeonMonsterSurfaces();

            systemState.Factory.Font.InterpolationHint = InterpolationMode.Fastest;
             
            AgateLib.Core.ErrorReporting.CrossPlatformDebugLevel = CrossPlatformDebugLevel.Exception;

            soundMan.Load();

        }

        public void ProcessArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-debug":
                        XleCore.EnableDebugMode = true;
                        break;
                }
            }
        }

        private void LoadGameFile()
        {
            data.LoadGameFile("Game.xml");
        }

        public void Run()
        {
            LoadGameFile();

            runner.Run(gameFactory);
        }
    }
}
