using AgateLib.Geometry;
using AgateLib.InputLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Extenders
{
	public abstract class Map2DExtender : MapExtender
	{
		protected void _Move2D(Player player, Direction dir, string textStart, out string command, out Point stepDirection)
		{
			player.FaceDirection = dir;

			command = textStart + " " + dir.ToString();
			int stepSize = StepSize;

			switch (dir)
			{
				case Direction.West:
					stepDirection = new Point(-stepSize, 0);
					break;

				case Direction.North:
					stepDirection = new Point(0, -stepSize);
					break;

				case Direction.East:
					stepDirection = new Point(stepSize, 0);
					break;

				case Direction.South:
					stepDirection = new Point(0, stepSize);
					break;

				default:
					stepDirection = Point.Empty;
					break;
			}

		}

		public override bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			int test = 0;

			if (GuardInSpot(xx, yy))
				return false;

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					test = TheMap[xx + i, yy + j];

					if (IsTileBlocked(test))
						return false;
				}
			}

			return true;
		}

		protected virtual bool GuardInSpot(int xx, int yy)
		{
			return false;
		}

		protected virtual bool IsTileBlocked(int tile)
		{
			return TheMap.TileSet[tile] == TileInfo.Blocked;
		}

		public override void PlayerCursorMovement(GameState state, Direction dir)
		{
			var player = state.Player;

			string command;
			Point stepDirection;

			_Move2D(player, dir, "Move", out command, out stepDirection);

			if (CanPlayerStep(state, stepDirection))
			{
				XleCore.TextArea.PrintLine(command);

				MovePlayer(XleCore.GameState, stepDirection);
				SoundMan.PlaySound(LotaSound.WalkTown);

				player.TimeQuality += 0.03;
			}
			else
			{
				SoundMan.PlaySound(LotaSound.Invalid);

				//Commands.CommandList.UpdateCommand("Move Nowhere");
				XleCore.TextArea.PrintLine("Move nowhere");
			}
		}

		public override bool PlayerFight(GameState state)
		{
			var player = state.Player;

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			XleCore.TextArea.PrintLine("Fight with " + player.CurrentWeaponTypeName);
			XleCore.TextArea.Print("Enter direction: ");

			KeyCode key = XleCore.WaitForKey(KeyCode.Up, KeyCode.Down, KeyCode.Left, KeyCode.Right);

			Direction fightDir;

			switch (key)
			{
				case KeyCode.Right: fightDir = Direction.East; break;
				case KeyCode.Up: fightDir = Direction.North; break;
				case KeyCode.Left: fightDir = Direction.West; break;
				case KeyCode.Down: fightDir = Direction.South; break;
				default:
					throw new InvalidOperationException();
			}

			XleCore.TextArea.PrintLine(fightDir.ToString());

			PlayerFight(state, fightDir);

			return true;
		}

		protected virtual void PlayerFight(GameState state, Direction fightDir)
		{ }


		public override bool PlayerXamine(GameState state)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You are in " + TheMap.MapName + ".");
			XleCore.TextArea.PrintLine("Look about to see more.");

			return true;
		}

		public override bool PlayerLeave(GameState state)
		{
			if (TheMap.HasGuards && TheMap.Guards.IsAngry)
			{
				XleCore.TextArea.PrintLine("Walk out yourself.");
				XleCore.Wait(200);
			}
			else
			{
				XleCore.TextArea.PrintLine("Leave " + TheMap.MapName);
				XleCore.TextArea.PrintLine();

				XleCore.Wait(200);

				state.Player.ReturnToPreviousMap();
			}

			return true;
		}
	}
}
