using AgateLib;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Xle.Ancients.TitleScreen;
using Xle.Bootstrap;
using Xle.Diagnostics;
using Xle.Scenes;
using Xle.Services.Commands;

namespace Xle.Ancients
{
    [Singleton]
    public class LotaProgram
    {
        private static ICommandFactory commandFactory;

        // Copy these instead:
        //    commands.Items.Add(CommandFactory.Armor());
        //    commands.Items.Add(CommandFactory.Gamespeed());
        //    commands.Items.Add(CommandFactory.Hold());
        //    commands.Items.Add(CommandFactory.Inventory());
        //    commands.Items.Add(CommandFactory.Pass());
        //    commands.Items.Add(CommandFactory.Weapon());

        [Obsolete("Just put the ones you want.")]
        public static IEnumerable<Command> CommonLotaCommands
        {
            get
            {
                yield return commandFactory.Armor();
                yield return commandFactory.Gamespeed();
                yield return commandFactory.Hold();
                yield return commandFactory.Inventory();
                yield return commandFactory.Pass();
                yield return commandFactory.Weapon();
            }
        }

        private readonly SceneStack scenes;
        private readonly SceneFactory sceneFactory;
        private readonly IAgateConsoleManager consoleManager;

        public LotaProgram(SceneStack scenes,
                           SceneFactory sceneFactory,
                           IXleConsoleCommands consoleCommands,
                           IAgateConsoleManager consoleManager)
        {
            this.scenes = scenes;
            this.sceneFactory = sceneFactory;
            this.consoleManager = consoleManager;

            consoleManager.AddVocabulary(consoleCommands);

            StartTitle();
        }

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

        ///// <summary>
        ///// The main entry point for the application.
        ///// </summary>
        //[STAThread]
        //static void Main(string[] args)
        //{
        //	RunGame(args);
        //}

        //private static void RunGame(string[] args)
        //{
        //	using (new AgateWinForms(args)
        //		.AssetPath("LotA")
        //		.Initialize())
        //	using (new DisplayWindowBuilder(args)
        //		.BackbufferSize(680, 440)
        //		.Title("Legacy of the Ancients")
        //		.WithCoordinates(new FixedCoordinateSystem(new Rectangle(-20, -20, 680, 440)))
        //		.AllowResize()
        //		.Build())
        //	{
        //		AgateApp.SetAssetPath("LotA");

        //		GameRunner(args);
        //	}
        //}

        //private static void GameRunner(string[] args)
        //{
        //	var initializer = new WindsorInitializer();
        //	var container = initializer.BootstrapContainer(typeof(LotaProgram).Assembly);

        //	IXleStartup core = container.Resolve<IXleStartup>();
        //	core.ProcessArguments(args);
        //	commandFactory = container.Resolve<ICommandFactory>();

        //	core.Run();
        //}

    }

    [Singleton]
    public class SceneFactory
    {
        private readonly IAgateServiceLocator serviceLocator;

        public SceneFactory(IAgateServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public LotaTitleScene CreateTitleScene()
        {
            return serviceLocator.Resolve<LotaTitleScene>();
        }

        public GamePlayScene CreateGamePlayScene()
        {
            return serviceLocator.Resolve<GamePlayScene>();
        }
    }
}
