﻿using AgateLib;
using Xle.Services.ScreenModel;

namespace Xle.Services.Commands
{
    public interface ICommand
    {
        string Name { get; }

        void Execute();
    }

    [InjectProperties]
    public abstract class Command : ICommand
    {
        public ITextArea TextArea { get; set; }
        public GameState GameState { get; set; }

        protected Player Player { get { return GameState.Player; } }

        public virtual string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        public abstract void Execute();
    }
}