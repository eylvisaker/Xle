using System;
using System.Collections.Generic;

using AgateLib.InputLib;

namespace ERY.Xle.Services.Commands.Implementation
{
	public class CommandList : ICommandList
	{
		Dictionary<Keys, Direction> mDirectionMap = new Dictionary<Keys, Direction>();

		public CommandList()
		{
			mDirectionMap[Keys.Up] = Direction.North;
			mDirectionMap[Keys.Left] = Direction.West;
			mDirectionMap[Keys.Down] = Direction.South;

			mDirectionMap[Keys.OpenBracket] = Direction.North;
			mDirectionMap[Keys.Semicolon] = Direction.West;
			mDirectionMap[Keys.Quotes] = Direction.East;
			mDirectionMap[Keys.Slash] = Direction.South;

			Items = new List<ICommand>();
		}

		public List<ICommand> Items { get; set; }

		bool IsCursorMovement(Keys cmd)
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
			var keystring = AgateInputEventArgs.GetKeyString(cmd, new KeyModifiers());

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