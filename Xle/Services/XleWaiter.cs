using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;
using Xle.Services.Rendering;

namespace Xle.Services
{
    public interface IXleWaiter
    {
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

        public async Task WaitAsync(int howLong_ms, bool allowKeyBreak = false, IRenderer renderer = null)
        {
            if (howLong_ms <= 0)
                return;

            waitScene.Initialize(howLong_ms, allowKeyBreak, renderer);

            sceneStack.AddOrBringToTop(waitScene);

            await waitScene.Wait();
        }
    }
}
