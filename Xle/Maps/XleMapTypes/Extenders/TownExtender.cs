using AgateLib.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class TownExtender : Map2DExtender
	{
		public new Town TheMap { get { return (Town)base.TheMap; } }

		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Orange;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}

		public virtual void OnSetAngry(bool value)
		{
		}

		public override void AfterExecuteCommand(GameState state, AgateLib.InputLib.KeyCode cmd)
		{
			base.AfterExecuteCommand(state, cmd);

			UpdateGuards(state.Player);
		}

		public override void OnAfterEntry(GameState state)
		{
			if (TheMap.MapID == state.Player.LastAttackedMapID)
			{
				IsAngry = true;

				XleCore.TextArea.Clear(true);
				XleCore.TextArea.PrintLine("\nWe remember you - slime!");

				XleCore.Wait(2000);
			}
			else
			{
				state.Player.LastAttackedMapID = 0;
			}
		}


		public virtual double ChanceToHitGuard(Player player, Guard guard, int distance)
		{
			int weaponType = player.CurrentWeaponType;

			return (player.Attribute[Attributes.dexterity] + 16)
				* (99 + weaponType * 8) / 7000.0 / guard.Defense * 99;
		}

		public virtual int RollDamageToGuard(Player player, Guard guard)
		{
			int weaponType = player.CurrentWeaponType;

			double damage = 1 + player.Attribute[Attributes.strength] *
					   (weaponType / 2 + 1) / 4;

			damage *= 0.5 + XleCore.random.NextDouble();

			return (int)Math.Round(damage);
		}


		public override bool UseFancyMagicPrompt
		{
			get { return true; }
		}


		protected override bool GuardInSpot(int x, int y)
		{
			for (int i = 0; i < TheMap.Guards.Count; i++)
			{
				Guard g = TheMap.Guards[i];

				if (g.X != 0 && g.Y != 0)
				{
					if ((g.X == x - 1 || g.X == x || g.X == x + 1) &&
						(g.Y == y - 1 || g.Y == y || g.Y == y + 1))
					{
						return true;
					}
				}
			}

			return false;
		}
		public bool IsAngry
		{
			get { return TheMap.Guards.IsAngry; }
			set
			{
				TheMap.Guards.IsAngry = value;

				OnSetAngry(value);
			}
		}

		public void UpdateGuards(Player player)
		{
			if (IsAngry == false)
				return;

			foreach (var guard in TheMap.Guards)
			{
				if (guard.SkipMovement)
					continue;
				if (TheMap.PointInRoof(guard.X, guard.Y) != -1)
					continue;

				bool badPt = false;

				int xdist = player.X - guard.X;
				int ydist = player.Y - guard.Y;

				int dx = 0;
				int dy = 0;

				if (xdist != 0)
					dx = xdist / Math.Abs(xdist);
				else dx = 0;
				if (ydist != 0)
					dy = ydist / Math.Abs(ydist);
				else dy = 0;

				double dist = Math.Sqrt(Math.Pow(xdist, 2) + Math.Pow(ydist, 2));
				if (dist >= 25)
					continue;

				Point newPt = guard.Location;

				if (Math.Abs(xdist) <= 2 && Math.Abs(ydist) <= 2)
				{
					if (Math.Abs(xdist) > Math.Abs(ydist))
						dy = 0;
					else
						dx = 0;

					GuardAttackPlayer(player, guard);
				}
				else if (dist < 25)
				{
					if (Math.Abs(xdist) > Math.Abs(ydist))
					{
						newPt.X += dx;
						dy = 0;
					}
					else
					{
						newPt.Y += dy;
						dx = 0;
					}

					badPt = !CanGuardStepInto(newPt, guard);

					if (badPt == true)
					{
						newPt = guard.Location;

						if (Math.Abs(xdist) > Math.Abs(ydist))
						{
							if (ydist == 0)
								dy = XleCore.random.Next(2) * 2 - 1;
							else
								dy = ydist / Math.Abs(ydist);

							dx = 0;

							newPt.Y += dy;
						}
						else
						{
							if (xdist == 0)
								dx = XleCore.random.Next(2) * 2 - 1;
							else
								dx = xdist / Math.Abs(xdist);

							dy = 0;

							newPt.X += dx;
						}
						badPt = !CanGuardStepInto(newPt, guard);

						if (badPt == true)
							newPt = guard.Location;

					}

					guard.Location = newPt;
				}

				if (badPt)
				{
					if (Math.Abs(xdist) >= Math.Abs(ydist))
					{
						if (xdist < 0)
							guard.Facing = Direction.West;
						else
							guard.Facing = Direction.East;
					}
					else
					{
						if (ydist < 0)
							guard.Facing = Direction.North;
						else
							guard.Facing = Direction.South;
					}
				}
				else if (dy == 0)
				{
					if (xdist < 0)
					{
						guard.Facing = Direction.West;
					}
					else
					{
						guard.Facing = Direction.East;
					}
				}
				else // dx == 0
				{
					if (ydist < 0)
					{
						guard.Facing = Direction.North;
					}
					else
					{
						guard.Facing = Direction.South;
					}
				}
			}
		}

		bool CanGuardStepInto(Point pt, Guard guard)
		{
			Size guardSize = new Size(2, 2);

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					var tile = TheMap[pt.X + i, pt.Y + j];

					if (TheMap.TileSet[tile] == TileInfo.Blocked ||
						TheMap.TileSet[tile] == TileInfo.NormalBlockGuards)
						return false;

					// check for guard-guard collisions
					Rectangle guardRect = new Rectangle(pt, guardSize);

					foreach (var otherGuard in TheMap.Guards)
					{
						if (otherGuard == guard)
							continue;

						Rectangle otherGuardRect = new Rectangle(otherGuard.Location, guardSize);

						if (guardRect.IntersectsWith(otherGuardRect))
							return false;
					}
				}
			}

			return true;
		}
		void AttackGuard(Player player, int grd, int distance)
		{
			AttackGuard(player, TheMap.Guards[grd], distance);
		}
		void AttackGuard(Player player, Guard guard, int distance)
		{
			if (guard.OnPlayerAttack != null)
			{
				bool cancel = guard.OnPlayerAttack(XleCore.GameState, guard);
				if (cancel)
					return;
			}

			double hitChance = ChanceToHitGuard(player, guard, distance);


			if (XleCore.random.NextDouble() > hitChance)
			{
				XleCore.TextArea.PrintLine("Attack on " + guard.Name + " missed", XleColor.Purple);
				SoundMan.PlaySound(LotaSound.PlayerMiss);
			}
			else
			{
				int dam = RollDamageToGuard(player, guard);

				IsAngry = true;
				player.LastAttackedMapID = TheMap.MapID;

				XleCore.TextArea.Print(guard.Name + " struck  ", XleColor.Yellow);
				XleCore.TextArea.Print(dam.ToString(), XleColor.White);
				XleCore.TextArea.Print("  H.P. Blow", XleColor.White);
				XleCore.TextArea.PrintLine();

				guard.HP -= dam;

				SoundMan.PlaySound(LotaSound.PlayerHit);

				if (guard.HP <= 0)
				{
					XleCore.TextArea.PrintLine(guard.Name + " killed");

					TheMap.Guards.Remove(guard);

					XleCore.Wait(100);

					SoundMan.StopSound(LotaSound.PlayerHit);
					SoundMan.PlaySound(LotaSound.EnemyDie);

					if (guard.OnGuardDead != null)
						guard.OnGuardDead(XleCore.GameState, guard);
				}
			}
		}

		/// <summary>
		/// Returns the index of the roof the character standing at the point
		/// ptx, pty.  -1 if no roof.
		/// </summary>
		/// <param name="ptx"></param>
		/// <param name="pty"></param>
		/// <returns></returns>
		int CharInRoof(int ptx, int pty)
		{
			for (int i = 0; i < TheMap.Roofs.Count; i++)
			{
				Roof r = TheMap.Roofs[i];

				if (r.CharIn(ptx, pty, false))
					return i;
			}

			return -1;
		}

		public virtual void PlayCloseRoofSound(Roof roof)
		{
			SoundMan.PlaySound(LotaSound.BuildingClose);
			XleCore.Wait(50);
		}
		public virtual void PlayOpenRoofSound(Roof roof)
		{
			SoundMan.PlaySound(LotaSound.BuildingOpen);
			XleCore.Wait(50);
		}

		public virtual void SpeakToGuard(GameState gameState)
		{
			XleCore.TextArea.PrintLine("\n\nThe guard salutes.");
		}

		public virtual void GuardAttackPlayer(Player player, Guard guard)
		{
			XleCore.TextArea.PrintLine();

			XleCore.TextArea.Print("Attacked by " + guard.Name + "! -- ", XleColor.White);

			if (XleCore.random.NextDouble() > ChanceToHitPlayer(player, guard))
			{
				XleCore.TextArea.Print("Missed", XleColor.Cyan);
				SoundMan.PlaySound(LotaSound.EnemyMiss);
			}
			else
			{
				int armorType = player.CurrentArmorType;

				int dam = RollDamageToPlayer(player, guard);

				XleCore.TextArea.Print("Blow ", XleColor.Yellow);
				XleCore.TextArea.Print(dam.ToString(), XleColor.White);
				XleCore.TextArea.Print(" H.P.", XleColor.White);

				SoundMan.PlaySound(LotaSound.EnemyHit);

				player.HP -= dam;
			}

			XleCore.TextArea.PrintLine();

			XleCore.Wait(100 * player.Gamespeed);
		}

		protected override void PlayerFight(GameState state, Direction fightDir)
		{
			var player = state.Player;

			bool attacked = false;
			int maxXdist = 1;
			int maxYdist = 1;
			int tile = 0, tile1;
			int hit = 0;

			if (XleCore.Data.WeaponList[player.CurrentWeaponType].Ranged)
			{
				maxXdist = 12;
				maxYdist = 8;
			}

			player.FaceDirection = fightDir;

			Point attackPt = Point.Empty, attackPt2 = Point.Empty;

			attackPt.X = player.X;
			attackPt.Y = player.Y;
			attackPt2.X = attackPt.X;
			attackPt2.Y = attackPt.Y;

			int dx = 0, dy = 0;

			switch (fightDir)
			{
				case Direction.East:
					attackPt.X++;
					attackPt2.X++;
					attackPt2.Y++;
					dx = 1;
					break;
				case Direction.North:
					dy = -1;
					attackPt2.X++;
					break;
				case Direction.West:
					dx = -1;
					attackPt2.Y++;
					break;
				case Direction.South:
					dy = 1;
					attackPt.Y++;
					attackPt2.X++;
					attackPt2.Y++;
					break;
			}

			var guards = TheMap.Guards;

			for (int i = 1; i <= maxXdist && attacked == false && tile < 128; i++)
			{
				for (int j = 1; j <= maxYdist && attacked == false && tile < 128; j++)
				{

					for (int k = 0; k < guards.Count; k++)
					{
						if ((guards[k].X == attackPt.X + dx * i
							|| guards[k].X + 1 == attackPt.X + dx * i
							|| guards[k].X == attackPt2.X + dx * i
							|| guards[k].X + 1 == attackPt2.X + dx * i
							)
							&&
							(guards[k].Y == attackPt.Y + dy * j
							|| guards[k].Y + 1 == attackPt.Y + dy * j
							|| guards[k].Y == attackPt2.Y + dy * j
							|| guards[k].Y + 1 == attackPt2.Y + dy * j
							)
							&&
							attacked == false)
						{
							AttackGuard(player, k, Math.Max(i, j));
							attacked = true;

							XleCore.Wait(200);
						}
					}

					tile = TheMap[attackPt.X + dx, attackPt.Y + dy];
					tile1 = TheMap[attackPt2.X + dx, attackPt2.Y + dy];

					int t = TheMap.RoofTile(attackPt.X + dx, attackPt.Y + dy);
					if (t != 127 && t != 0)
						tile = 128;

					t = TheMap.RoofTile(attackPt2.X + dx, attackPt2.Y + dy);
					if (t != 127 && t != 0)
						tile1 = 128;


					if (tile == 222 || tile == 223 || tile == 238 || tile == 239)
					{
						hit = 1;
					}
					else if (tile1 == 222 || tile1 == 223 || tile1 == 238 || tile1 == 239)
					{
						hit = 2;
					}
					if (hit > 0)
					{
						if (hit == 1)
						{
							if (tile == 223)
							{
								attackPt.X--;
							}
							else if (tile == 238)
							{
								attackPt.Y--;
							}
							else if (tile == 239)
							{
								attackPt.X--;
								attackPt.Y--;
							}
						}
						else if (hit == 2)
						{
							attackPt = attackPt2;

							if (tile1 == 223)
							{
								attackPt.X--;
							}
							else if (tile1 == 238)
							{
								attackPt.Y--;
							}
							else if (tile1 == 239)
							{
								attackPt.X--;
								attackPt.Y--;
							}
						}

						int dam = XleCore.random.Next(10) + 30;

						XleCore.TextArea.PrintLine();
						XleCore.TextArea.PrintLine("Merchant killed by blow of " + dam.ToString());

						TheMap[attackPt.X + dx, attackPt.Y + dy] = 0x52;
						TheMap[attackPt.X + dx, attackPt.Y + dy + 1] = 0x52;
						TheMap[attackPt.X + dx + 1, attackPt.Y + dy + 1] = 0x52;
						TheMap[attackPt.X + dx + 1, attackPt.Y + dy] = 0x52;

						IsAngry = true;

						SoundMan.PlaySound(LotaSound.EnemyDie);

						attacked = true;


					}

					if (tile == 176 || tile1 == 176 || tile == 192 || tile == 192)
					{
						XleCore.TextArea.PrintLine("The prison bars hold.");

						SoundMan.PlaySound(LotaSound.Bump);

						attacked = true;
					}
				}

			}

			if (attacked == false)
			{
				XleCore.TextArea.PrintLine("Nothing hit");
			}

			XleCore.Wait(200 + 50 * player.Gamespeed, true, XleCore.Redraw);
		}



		public override bool PlayerRob(GameState state)
		{
			foreach (var evt in TheMap.EventsAt(state.Player, 1))
			{
				bool handled = false;

				if (evt.AllowRobWhenNotAngry == false && this.IsAngry == false)
				{
					evt.RobFail();
					return true;
				}

				handled = evt.Rob(state);
				IsAngry = true;

				if (handled)
					return handled;
			}

			return PlayerRobImpl(state.Player);
		}
		protected virtual bool PlayerRobImpl(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Nothing to rob");
			XleCore.Wait(500);

			return true;
		}


		protected override void AfterStepImpl(GameState state, bool didEvent)
		{
			var player = state.Player;

			Point pt = new Point(player.X, player.Y);
			var roofs = TheMap.Roofs;

			for (int i = 0; i < roofs.Count; i++)
			{
				if (roofs[i].Open == false && roofs[i].CharIn(pt))
				{
					roofs[i].Open = true;
					PlayOpenRoofSound(roofs[i]);
				}
				else if (roofs[i].Open == true && IsAngry == false
					&& roofs[i].CharIn(pt) == false)
				{
					roofs[i].Open = false;
					PlayCloseRoofSound(roofs[i]);
				}
			}

			if (player.X < 0 || player.X >= TheMap.Width - 1 ||
				player.Y < 0 || player.Y >= TheMap.Height - 1)
			{
				if (IsAngry && this.GetType().Equals(typeof(Town)))
				{
					player.LastAttackedMapID = TheMap.MapID;
				}

				LeaveMap(player);
			}
		}

		protected override bool PlayerSpeakImpl(GameState state)
		{
			var guards = TheMap.Guards;
			var player = state.Player;

			for (int j = -1; j < 3; j++)
			{
				for (int i = -1; i < 3; i++)
				{
					foreach (var guard in guards)
					{
						if ((guard.X == player.X + i || guard.X + 1 == player.X + i) &&
							(guard.Y == player.Y + j || guard.Y + 1 == player.Y + j))
						{
							SpeakToGuard(state);
							return true;

						}
					}
				}
			}

			return false;
		}

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
			if (IsAngry)
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
