using System;

using Xle.Data;
using Xle.Services.Game;
using Xle.Services.Menus;
using Xle.Services.XleSystem;

namespace Xle.Services.Commands.Implementation
{
    public interface IFight : ICommand { }
    public abstract class Fight : Command, IFight
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
