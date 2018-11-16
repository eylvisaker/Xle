using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.Foundation;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Xle.Scenes;

namespace Xle
{
    [Singleton]
    public class XleProgram
    {
        private readonly SceneStack scenes;
        private readonly SceneFactory sceneFactory;
        private readonly IAgateConsoleManager consoleManager;

        public XleProgram(SceneStack scenes,
                          SceneFactory sceneFactory,
                          IVocabulary[] consoleCommands,
                          IAgateConsoleManager consoleManager)
        {
            this.scenes = scenes;
            this.sceneFactory = sceneFactory;
            this.consoleManager = consoleManager;

            consoleManager.AddVocabulary(consoleCommands);

            consoleManager.Quit += () => IsFinished = true;

            StartTitle();
        }

        public bool IsFinished { get; private set; }

        private void StartTitle()
        {
            var title = sceneFactory.CreateTitleScene();

            scenes.Add(title);

            title.BeginGame += player =>
            {
                var game = sceneFactory.CreateGamePlayScene();
                game.Run(player);

                scenes.Remove(title);
                scenes.Add(game);

                game.SceneEnd += (_, __) => StartTitle();
            };
        }

        public void Update(GameTime gameTime)
        {
            consoleManager.Update(gameTime);

            scenes.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            scenes.Draw(gameTime);
        }
    }

    [Singleton]
    public class SceneFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public SceneFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ITitleScene CreateTitleScene()
        {
            return serviceLocator.Resolve<ITitleScene>();
        }

        public GamePlayScene CreateGamePlayScene()
        {
            return serviceLocator.Resolve<GamePlayScene>();
        }
    }
}
