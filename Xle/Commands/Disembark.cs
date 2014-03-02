using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Commands
{
	public class Disembark : Command
	{
		public override void Execute(GameState state)
		{
			ExecDisembark(state.Player);
		}

		[Obsolete]
		void ExecDisembark(Player player)
		{
			int newx, newy;

			g.UpdateBottom("Enter command: disembark raft");

			if (player.IsOnRaft)
			{

				g.AddBottom("");
				g.AddBottom("Disembark in which direction?");

				do
				{
					XleCore.Redraw();

				} while (!(
					Keyboard.Keys[KeyCode.Left] || Keyboard.Keys[KeyCode.Right] ||
					Keyboard.Keys[KeyCode.Up] || Keyboard.Keys[KeyCode.Down]));

				newx = player.X;
				newy = player.Y;

				Direction dir = Direction.East;

				if (Keyboard.Keys[KeyCode.Left])
					dir = Direction.West;
				else if (Keyboard.Keys[KeyCode.Up])
					dir = Direction.North;
				else if (Keyboard.Keys[KeyCode.Down])
					dir = Direction.South;
				else if (Keyboard.Keys[KeyCode.Right])
					dir = Direction.East;

				player.Disembark(dir);

				SoundMan.StopSound(LotaSound.Raft1);

			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Nothing to disembark", XleColor.Yellow);

			}
		}
	}
}
