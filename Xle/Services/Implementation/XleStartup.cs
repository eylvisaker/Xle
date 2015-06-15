using ERY.Xle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle
{
    public class XleStartup : IXleStartup
    {
        IXleRunner runner;
        IXleGameFactory gameFactory;
        XleSystemState systemState;

        public XleStartup(IXleRunner runner, IXleGameFactory xleGameFactory, XleSystemState systemState)
        {
            this.runner = runner;
            this.gameFactory = xleGameFactory;
            this.systemState = systemState;
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
            systemState.Data = new XleData();
            systemState.Data.LoadGameFile("Game.xml");
        }

        public void Run()
        {
            LoadGameFile();

            runner.Run(gameFactory);
        }
    }
}
