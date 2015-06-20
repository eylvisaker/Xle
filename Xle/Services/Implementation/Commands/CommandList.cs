using System;
using System.Collections.Generic;
using System.Windows.Input;

using AgateLib.InputLib;
using ERY.Xle.Rendering;

namespace ERY.Xle.Services.Implementation.Commands
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

            Items = new List<Command>();
        }

        public List<Command> Items { get; set; }

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

        public Command FindCommand(KeyCode cmd)
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

        public Command CurrentCommand { get; set; }

        public bool IsLeftMenuActive { get; set; }
    }
}