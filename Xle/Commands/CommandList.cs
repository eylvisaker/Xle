using System;
using System.Collections.Generic;
using System.Text;
using AgateLib;
using AgateLib.DisplayLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using ERY.Xle.Commands;

namespace ERY.Xle.Commands
{
	public class CommandList
	{
		public GameState State { get; set; }
		Player player { get { return State.Player; } }

		Dictionary<KeyCode, Direction> mDirectionMap = new Dictionary<KeyCode, Direction>();

		public CommandList(GameState state)
		{
			State = state;

			mDirectionMap[KeyCode.Right] = Direction.East;
			mDirectionMap[KeyCode.Up] = Direction.North;
			mDirectionMap[KeyCode.Left] = Direction.West;
			mDirectionMap[KeyCode.Down] = Direction.South;

			mDirectionMap[KeyCode.OpenBracket] = Direction.North;
			mDirectionMap[KeyCode.Semicolon] = Direction.West;
			mDirectionMap[KeyCode.Quotes] = Direction.East;
			mDirectionMap[KeyCode.Slash] = Direction.South;

			Items = new List<Command>();

			Items.Add(new Armor());
			Items.Add(new Climb());
			Items.Add(new Disembark());
			Items.Add(new End());
			Items.Add(new Fight());
			Items.Add(new Gamespeed());
			Items.Add(new Hold());
			Items.Add(new Inventory());
			Items.Add(new Leave { ConfirmPrompt = false });
			Items.Add(new Magic());
			Items.Add(new Open());
			Items.Add(new Pass());
			Items.Add(new Rob());
			Items.Add(new Speak());
			Items.Add(new Take());
			Items.Add(new Use { ShowItemMenu = false });
			Items.Add(new Weapon());
			Items.Add(new Xamine());
		}

		public void Prompt()
		{
			if (player.HP <= 0 || player.Food <= 0)
			{
				XleCore.PlayerIsDead();
			}

			XleCore.TextArea.Print("\nEnter command: ");
		}

		public List<Command> Items { get; set; }
		/// <summary>
		/// Returns true if the command is a cursor movement.
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		private void CursorMovement(KeyCode cmd)
		{
			Direction dir = mDirectionMap[cmd];

			XleCore.Map.PlayerCursorMovement(player, dir);
		}

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
		public void DoCommand(KeyCode cmd)
		{
			if (cmd == KeyCode.None)
				return;

			int waitTime = 700;

			if (IsCursorMovement(cmd))
			{
				ExecuteCursorMovement(cmd);
				return;
			}

			var command = FindCommand(cmd);

			if (command != null)
			{
				CurrentCommand = command;

				XleCore.TextArea.Print(command.Name);

				command.Execute(State);
			}
			else
			{
				SoundMan.PlaySound(LotaSound.Invalid);

				XleCore.Wait(waitTime);
				return;
			}

			AfterDoCommand(waitTime, cmd);
		}

		private Command FindCommand(KeyCode cmd)
		{
			var command = Items.Find(x => x.Name.StartsWith(AgateLib.InputLib.Keyboard.GetKeyString(cmd,
				new KeyModifiers()), StringComparison.InvariantCultureIgnoreCase));

			return command;

		}

		private void ExecuteCursorMovement(KeyCode cmd)
		{
			var wasRaft = player.BoardedRaft;

			CursorMovement(cmd);

			XleCore.Renderer.AnimateStep();

			var waitTime = State.Map.WaitTimeAfterStep;

			if (wasRaft != player.BoardedRaft)
			{
				if (player.IsOnRaft)
				{
					XleCore.TextArea.PrintLine();
					XleCore.TextArea.PrintLine("You climb onto a raft.");

					SoundMan.PlaySound(LotaSound.BoardRaft);
				}

			}

			// check for events
			State.Map.AfterPlayerStep(player);

			AfterDoCommand(waitTime, cmd);
		}

		[Obsolete]
		public static void UpdateCommand(string command)
		{
			g.UpdateBottom("Enter Command: " + command);
		}

		public void ResetCommands()
		{
			Items.Sort((x, y) => x.Name.CompareTo(y.Name));
			CurrentCommand = Items.Find(x => x is Pass);

			if (CurrentCommand == null)
				CurrentCommand = Items[0];
		}

		public Command CurrentCommand { get; set; }

		private void AfterDoCommand(int waitTime, KeyCode cmd)
		{
			State.Map.AfterExecuteCommand(player, cmd);

			XleCore.Wait(waitTime, false, XleCore.Redraw);

			Prompt();
		}

		public bool IsLeftMenuActive { get; set; }
	}
}