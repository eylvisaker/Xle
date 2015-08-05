using System;

using ERY.Xle.Data;
using ERY.Xle.Services.Game;
using ERY.Xle.Services.Menus;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Services.Commands.Implementation
{
    public abstract class Fight : Command
    {
        public IXleGameControl GameControl { get; set; }
        public ISoundMan SoundMan { get; set; }
        public XleData Data { get; set; }
        public IQuickMenu QuickMenu { get; set; }
        public Random Random { get; set; }

        public override string Name
        {
            get { return "Fight"; }
        }
    }
}
