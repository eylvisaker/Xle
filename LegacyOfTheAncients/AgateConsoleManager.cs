using AgateLib;
using AgateLib.Diagnostics;
using AgateLib.Scenes;
using AgateLib.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Xle.Diagnostics;

namespace Xle.Ancients
{
    public interface IAgateConsoleManager
    {
        void Update(GameTime gameTime);

        void AddVocabulary(IVocabulary vocab);
    }

    [Singleton]
    public class AgateConsoleManager : IAgateConsoleManager
    {
        private readonly ISceneStack sceneStack;
        private readonly AgateConsole console;
        private readonly AgateConsoleScene consoleScene;

        public AgateConsoleManager(ISceneStack sceneStack, AgateConsole console, AgateConsoleScene consoleScene)
        {
            this.sceneStack = sceneStack;
            this.console = console;
            this.consoleScene = consoleScene;
        }

        /// <summary>
        /// Gets or sets the keyboard key that can be used to open the console.
        /// </summary>
        public Keys ActivationKey { get; set; } = Keys.OemTilde;

        public void AddVocabulary(IVocabulary vocab)
        {
            console.AddVocabulary(vocab);
        }

        public void Update(GameTime gameTime)
        {
            if (sceneStack.Contains(consoleScene))
                return;

            var keys = Keyboard.GetState();

            if (keys.IsKeyDown(ActivationKey))
            {
                sceneStack.Add(consoleScene);
            }
        }
    }
}
