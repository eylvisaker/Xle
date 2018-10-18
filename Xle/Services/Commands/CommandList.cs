using AgateLib;
using ERY.Xle.Services.Commands.Implementation;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ERY.Xle.Services.Commands
{
    public interface ICommandList : IXleService
    {
        List<ICommand> Items { get; }

        bool IsLeftMenuActive { get; }

        ICommand CurrentCommand { get; set; }

        void ResetCurrentCommand();

        ICommand FindCommand(Keys cmd);
    }

    [Singleton]
    public class CommandList : ICommandList
    {
        private Dictionary<Keys, Direction> mDirectionMap = new Dictionary<Keys, Direction>();

        public CommandList()
        {
            mDirectionMap[Keys.Up] = Direction.North;
            mDirectionMap[Keys.Left] = Direction.West;
            mDirectionMap[Keys.Right] = Direction.East;
            mDirectionMap[Keys.Down] = Direction.South;

            mDirectionMap[Keys.OemOpenBrackets] = Direction.North;
            mDirectionMap[Keys.OemSemicolon] = Direction.West;
            mDirectionMap[Keys.OemQuotes] = Direction.East;
            mDirectionMap[Keys.OemQuestion] = Direction.South;

            Items = new List<ICommand>();
        }

        public List<ICommand> Items { get; set; }

        private bool IsCursorMovement(Keys cmd)
        {
            switch (cmd)
            {
                case Keys.Right:
                case Keys.Up:
                case Keys.Left:
                case Keys.Down:
                    return true;

                default:
                    return false;
            }
        }

        public ICommand FindCommand(Keys cmd)
        {
            throw new NotImplementedException();
            //var keystring = AgateInputEventArgs.GetKeyString(cmd, new KeyModifiers());

            //if (string.IsNullOrWhiteSpace(keystring))
            //    return null;

            //var command = Items.Find(x => x.Name.StartsWith(keystring, StringComparison.InvariantCultureIgnoreCase));

            //return command;

        }

        public void ResetCurrentCommand()
        {
            Items.Sort((x, y) => x.Name.CompareTo(y.Name));

            CurrentCommand = Items.Find(x => x is Pass);

            if (CurrentCommand == null)
                CurrentCommand = Items[0];
        }

        public ICommand CurrentCommand { get; set; }

        public bool IsLeftMenuActive { get; set; }
    }
}