﻿namespace ERY.Xle.Services.Commands
{
    public interface ICommand
    {
        string Name { get; }

        void Execute();
    }
}