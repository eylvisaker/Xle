using AgateLib.Scenes;
using System;

namespace Xle.Scenes
{
    public interface ITitleScene : IScene
    {
        event Action<Player> BeginGame;
    }
}
