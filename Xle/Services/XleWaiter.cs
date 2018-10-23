using System;
using System.Threading.Tasks;
using AgateLib;
using AgateLib.Input;
using AgateLib.Scenes;
using Microsoft.Xna.Framework;

namespace Xle.Services
{
    public interface IXleWaiter
    {
        Task WaitAsync(int howLong_ms, bool allowKeyBreak = false);

        void Wait(int howLong_ms, bool allowKeyBreak = false);
    }

    [Transient]
    public class XleWaiter : IXleWaiter
    {
        private class WaitScene : Scene
        {
            private float timeLeft_ms;
            private bool allowKeyBreak;
            private KeyboardEvents keyboard = new KeyboardEvents();

            public WaitScene()
            {
                keyboard.KeyUp += Keyboard_KeyUp;

                DrawBelow = true;
            }

            public TimeSpan TimeLeft => TimeSpan.FromMilliseconds(timeLeft_ms);

            private void Keyboard_KeyUp(object sender, KeyEventArgs e)
            {
                if (allowKeyBreak)
                    timeLeft_ms = 0;
            }

            public void Initialize(int howLong_ms, bool allowKeyBreak)
            {
                timeLeft_ms = howLong_ms;
                this.allowKeyBreak = allowKeyBreak;

                IsFinished = false;
            }

            protected override void OnUpdateInput(IInputState input)
            {
                base.OnUpdateInput(input);

                keyboard.Update(input);
            }

            protected override void OnUpdate(GameTime time)
            {
                base.OnUpdate(time);
                timeLeft_ms -= (float)time.ElapsedGameTime.TotalMilliseconds;

                if (timeLeft_ms <= 0)
                    IsFinished = true;
            }

            public void TopOff(int howLong_ms)
            {
                timeLeft_ms = Math.Max(timeLeft_ms, howLong_ms);
            }
        }

        private readonly ISceneStack sceneStack;
        private readonly WaitScene waitScene = new WaitScene();

        public XleWaiter(ISceneStack sceneStack)
        {
            this.sceneStack = sceneStack;
        }

        public async Task WaitAsync(int howLong_ms, bool allowKeyBreak = false)
        {
            if (sceneStack.Contains(waitScene))
            {
                // Make sure the wait scene is on top of the stack.
                sceneStack.Remove(waitScene);
                sceneStack.Add(waitScene);

                waitScene.TopOff(howLong_ms);

                await Task.Delay(waitScene.TimeLeft);
            }
            else
            {
                waitScene.Initialize(howLong_ms, allowKeyBreak);

                sceneStack.Add(waitScene);
            }

            await Task.Delay(TimeSpan.FromMilliseconds(howLong_ms));
        }

        public void Wait(int howLong_ms, bool allowKeyBreak = false)
        {
            if (sceneStack.Contains(waitScene))
            {
                waitScene.TopOff(howLong_ms);
            }
            else
            {
                waitScene.Initialize(howLong_ms, allowKeyBreak);

                sceneStack.Add(waitScene);
            }
        }
    }
}
