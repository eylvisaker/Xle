using System;
using System.Collections.Generic;

using AgateLib.InputLib;

namespace ERY.Xle.Services.Commands.Implementation
{
    public class CommandList : ICommandList
    {
        Dictionary<KeyCode, Direction> mDirectionMap = new Dictionary<KeyCode, Direction>();

        public CommandList()
        {
            mDirectionMap[KeyCode.Up] = Direction.North;
            mDirectionMap[KeyCode.Left] = Direction.West;
            mDirectionMap[KeyCode.Down] = Direction.South;

            mDirectionMap[KeyCode.OpenBracket] = Direction.North;
            mDirectionMap[KeyCode.Semicolon] = Direction.West;
            mDirectionMap[KeyCode.Quotes] = Direction.East;
            mDirectionMap[KeyCode.Slash] = Direction.South;

            Items = new List<ICommand>();
        }

        public List<ICommand> Items { get; set; }

        bool IsCursorMovement(KeyCode cmd)
        {
            switch (cmd)
            {
                case KeyCode.Right:
                case KeyCode.Up:
                case KeyCode.Left:
                case KeyCode.Down:
                    return true;

                default:
                    return false;
            }
        }

        public ICommand FindCommand(KeyCode cmd)
        {
            var keystring = AgateLib.InputLib.Legacy.Keyboard.GetKeyString(cmd, new KeyModifiers());

            if (string.IsNullOrWhiteSpace(keystring))
                return null;

            var command = Items.Find(x => x.Name.StartsWith(keystring, StringComparison.InvariantCultureIgnoreCase));

            return command;

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