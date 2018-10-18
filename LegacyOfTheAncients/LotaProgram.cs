using AgateLib;
using AgateLib.Scenes;
using ERY.Xle.Services.Commands;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Xle.Ancients.TitleScreen;

namespace ERY.Xle.LotA
{
    [Singleton]
    public class LotaProgram
    {
        private static ICommandFactory commandFactory;

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

        private SceneStack scenes;

        public LotaProgram(SceneStack scenes, LotaTitleScene title)
        {
            this.scenes = scenes;

            scenes.Add(title);
        }

        public void Update(GameTime gameTime)
        {
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
}
