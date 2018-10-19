using AgateLib;
using AgateLib.Scenes;
using ERY.Xle;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xle.Scenes
{
    [Transient]
    public class GamePlayScene : Scene
    {
        public Player Player { get; internal set; }
    }
}
