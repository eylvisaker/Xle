using AgateLib;
using AgateLib.Scenes;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Xle
{
    public interface IXleWaiter
    {
        Keys? PressedKey { get; }

        Task WaitAsync(int howLong_ms, bool allowKeyBreak = false, IRenderer renderer = null);
    }

    // TODO: Merge this class with XleGameControl.
    [Transient]
    public class XleWaiter : IXleWaiter
    {
        private readonly ISceneStack sceneStack;
        private readonly WaitScene waitScene;

        public XleWaiter(ISceneStack sceneStack, GraphicsDevice graphics, WaitScene waitScene)
        {
            this.sceneStack = sceneStack;
            this.waitScene = waitScene;
        }

        public Keys? PressedKey => waitScene.PressedKey;

        public async Task WaitAsync(int howLong_ms, bool allowKeyBreak = false, IRenderer renderer = null)
        {
            if (howLong_ms <= 0)
                return;

            Debug.WriteLine($"Waiting for {howLong_ms}...");

            waitScene.Initialize(howLong_ms, allowKeyBreak, renderer);

            await waitScene.Wait(sceneStack);
        }
    }
}
