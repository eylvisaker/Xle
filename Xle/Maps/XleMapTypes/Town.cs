using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using ERY.Xle.Maps.XleMapTypes.Extenders;


namespace ERY.Xle.Maps.XleMapTypes
{
	public class Town : Map2D
	{
		List<Guard> mGuards = new List<Guard>();

		List<int> mMail = new List<int>();				// towns to carry mail to

		public ITownExtender Extender { get; protected set; }

		protected override IMapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new NullTownExtender();
			}
			else
			{
				Extender = XleCore.Factory.CreateMapExtender(this);
			}

			return Extender;
		}

		#region --- Construction and Serialization ---

		public Town()
		{
			HasRoofs = true;
			HasGuards = true;
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			base.WriteData(info);

			info.Write("Mail", mMail.ToArray());
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			try
			{
				mMail.AddRange(info.ReadInt32Array("Mail"));
			}
			catch (XleSerializationException)
			{ }
		}

		#endregion

		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "towntiles.png";
			}
		}

		protected override bool GuardInSpot(int x, int y)
		{
			for (int i = 0; i < Guards.Count; i++)
			{
				Guard g = Guards[i];

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
			get { return Guards.IsAngry; }
			set
			{
				Guards.IsAngry = value;

				Extender.OnSetAngry(value);
			}
		}

		protected override void AfterExecuteCommandImpl(Player player, KeyCode cmd)
		{
			UpdateGuards(player);
		}
		public void UpdateGuards(Player player)
		{
			if (IsAngry == false)
				return;

			foreach (var guard in Guards)
			{
				if (guard.SkipMovement)
					continue;
				if (PointInRoof(guard.X, guard.Y) != -1)
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
			int i, j, k;
			Size guardSize = new Size(2, 2);

			for (j = 0; j < 2; j++)
			{
				for (i = 0; i < 2; i++)
				{
					var tile = this[pt.X + i, pt.Y + j];

					if (TileSet[tile] == TileInfo.Blocked ||
						TileSet[tile] == TileInfo.NormalBlockGuards)
						return false;

					// check for guard-guard collisions
					Rectangle guardRect = new Rectangle(pt, guardSize);

					foreach (var otherGuard in Guards)
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
			AttackGuard(player, Guards[grd], distance);
		}
		void AttackGuard(Player player, Guard guard, int distance)
		{
			if (guard.OnPlayerAttack != null)
			{
				bool cancel = guard.OnPlayerAttack(XleCore.GameState, guard);
				if (cancel)
					return;
			}

			ColorStringBuilder builder = new ColorStringBuilder();

			double hitChance = Extender.ChanceToHitGuard(player, guard, distance);


			if (XleCore.random.NextDouble() > hitChance)
			{
				XleCore.TextArea.PrintLine("Attack on " + guard.Name + " missed", XleColor.Purple);
				SoundMan.PlaySound(LotaSound.PlayerMiss);
			}
			else
			{
				int dam = Extender.RollDamageToGuard(player, guard);

				IsAngry = true;
				player.LastAttackedMapID = MapID;

				builder.AddText(guard.Name + " struck  ", XleColor.Yellow);
				builder.AddText(dam.ToString(), XleColor.White);
				builder.AddText("  H.P. Blow", XleColor.White);

				g.AddBottom(builder);

				guard.HP -= dam;

				SoundMan.PlaySound(LotaSound.PlayerHit);

				if (guard.HP <= 0)
				{
					XleCore.TextArea.PrintLine(guard.Name + " killed");

					Guards.Remove(guard);

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
			for (int i = 0; i < Roofs.Count; i++)
			{
				Roof r = Roofs[i];

				if (r.CharIn(ptx, pty, false))
					return i;
			}

			return -1;
		}

		/*

		Point RoofAnchor(int r)
		{
			return new Point(0, 0);
			//if (r == 39)
			//{
			//    int i = 0;
			//}

			//int off = RoofOffset(r);
			//Point size;

			//
			//size.X = m[off] * 256 + m[off + 1];
			//size.Y = m[off + 2] * 256 + m[off + 3];
			//

			//return size;
		}

		Point RoofAnchorTarget(int r)
		{
			int off = RoofOffset(r);
			Point size;

			size.X = m[off + 4] * 256 + m[off + 5];
			size.Y = m[off + 6] * 256 + m[off + 7];

			return size;
		}

		Point RoofSize(int r)
		{
			int off = RoofOffset(r);
			Point size;

			size.X = m[off + 8] * 256 + m[off + 9];
			size.Y = m[off + 10] * 256 + m[off + 11];

			return size;
		}

		int RoofIntOffset(int r)
		{
			Point lastSize;
			int last;
			int me;

			if (r == 0)
			{
				return roofOffset;
			}

			last = RoofOffset(r - 1);
			lastSize = RoofSize(r - 1);

			me = last + 12 + lastSize.Y * lastSize.X;

			return me;

		}

		int RoofOffset(int r)
		{
			return eachRoofOffset[r];
		}
		*/

		int RoofTile(int xx, int yy)
		{
			foreach (var r in Roofs)
			{
				Rectangle boundingRect = r.Rectangle;

				if (boundingRect.Contains(new Point(xx, yy)))
				{
					var result = r[xx - r.X, yy - r.Y];

					if (result == 0 || result == 127)
						continue;

					if (r.Open)
						return 0;
				}
			}

			for (int i = 0; i < Roofs.Count; i++)
			{
				Roof r = Roofs[i];
				Rectangle boundingRect = r.Rectangle;

				if (boundingRect.Contains(new Point(xx, yy)))
				{
					var result = r[xx - r.X, yy - r.Y];

					if (result == 0 || result == 127)
						continue;

					return r[xx - r.X, yy - r.Y];
				}
			}

			return 0;
		}


		public override void GuardAttackPlayer(Player player, Guard guard)
		{
			XleCore.TextArea.PrintLine();

			XleCore.TextArea.Print("Attacked by " + guard.Name + "! -- ", XleColor.White);

			if (XleCore.random.NextDouble() > Extender.ChanceToHitPlayer(player, guard))
			{
				XleCore.TextArea.Print("Missed", XleColor.Cyan);
				SoundMan.PlaySound(LotaSound.EnemyMiss);
			}
			else
			{
				int armorType = player.CurrentArmorType;

				int dam = Extender.RollDamageToPlayer(player, guard);

				XleCore.TextArea.Print("Blow ", XleColor.Yellow);
				XleCore.TextArea.Print(dam.ToString(), XleColor.White);
				XleCore.TextArea.Print(" H.P.", XleColor.White);

				SoundMan.PlaySound(LotaSound.EnemyHit);

				player.HP -= dam;
			}

			XleCore.TextArea.PrintLine();

			XleCore.Wait(100 * player.Gamespeed);
		}

		public List<int> Mail
		{
			get { return mMail; }
			set { mMail = value; }
		}

		protected override void PlayerFight(Player player, Direction fightDir)
		{
			bool attacked = false;
			int maxXdist = 1;
			int maxYdist = 1;
			int tile = 0, tile1;
			int hit = 0;

			if (player.WeaponType(player.CurrentWeaponIndex) == 6 ||
				player.WeaponType(player.CurrentWeaponIndex) == 8)
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

			for (int i = 1; i <= maxXdist && attacked == false && tile < 128; i++)
			{
				for (int j = 1; j <= maxYdist && attacked == false && tile < 128; j++)
				{

					for (int k = 0; k < Guards.Count; k++)
					{
						if ((Guards[k].X == attackPt.X + dx * i
							|| Guards[k].X + 1 == attackPt.X + dx * i
							|| Guards[k].X == attackPt2.X + dx * i
							|| Guards[k].X + 1 == attackPt2.X + dx * i
							)
							&&
							(Guards[k].Y == attackPt.Y + dy * j
							|| Guards[k].Y + 1 == attackPt.Y + dy * j
							|| Guards[k].Y == attackPt2.Y + dy * j
							|| Guards[k].Y + 1 == attackPt2.Y + dy * j
							)
							&&
							attacked == false)
						{
							AttackGuard(player, k, Math.Max(i, j));
							attacked = true;

							XleCore.Wait(200);
						}
					}

					tile = this[attackPt.X + dx, attackPt.Y + dy];
					tile1 = this[attackPt2.X + dx, attackPt2.Y + dy];

					int t = RoofTile(attackPt.X + dx, attackPt.Y + dy);
					if (t != 127 && t != 0)
						tile = 128;

					t = RoofTile(attackPt2.X + dx, attackPt2.Y + dy);
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

						this[attackPt.X + dx, attackPt.Y + dy] = 0x52;
						this[attackPt.X + dx, attackPt.Y + dy + 1] = 0x52;
						this[attackPt.X + dx + 1, attackPt.Y + dy + 1] = 0x52;
						this[attackPt.X + dx + 1, attackPt.Y + dy] = 0x52;

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

		public override bool PlayerLeave(Player player)
		{
			if (IsAngry)
			{
				XleCore.TextArea.PrintLine("Walk out yourself.");
				XleCore.Wait(200);
			}
			else
			{
				XleCore.TextArea.PrintLine("Leave " + MapName);
				XleCore.TextArea.PrintLine();

				XleCore.Wait(200);

				player.ReturnToPreviousMap();
			}

			return true;
		}
		public override bool PlayerXamine(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("You are in " + MapName + ".");
			XleCore.TextArea.PrintLine("Look about to see more.");

			return true;
		}
		protected override bool PlayerSpeakImpl(Player player)
		{
			for (int j = -1; j < 3; j++)
			{
				for (int i = -1; i < 3; i++)
				{
					for (int k = 0; k < Guards.Count; k++)
					{
						if ((Guards[k].X == player.X + i ||
							Guards[k].X + 1 == player.X + i) && (
							Guards[k].Y == player.Y + j ||
							Guards[k].Y + 1 == player.Y + j))
						{
							SpeakToGuard(player);
							return true;

						}
					}
				}
			}

			return false;
		}
		public override bool PlayerRob(GameState state)
		{
			XleEvent evt = GetEvent(state.Player, 1);
			bool handled = false;

			if (evt != null)
			{
				if (evt.AllowRobWhenNotAngry == false && this.IsAngry == false)
				{
					evt.RobFail();
					return true;
				}

				handled = evt.Rob(GameState);
				IsAngry = true;

				if (handled)
					return handled;
			}

			return PlayerRobImpl(state.Player);
		}
		protected override bool PlayerRobImpl(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Nothing to rob");
			XleCore.Wait(500);

			return true;
		}

		protected override void AfterStepImpl(Player player, bool didEvent)
		{
			Point pt = new Point(player.X, player.Y);

			for (int i = 0; i < Roofs.Count; i++)
			{
				if (Roofs[i].Open == false && Roofs[i].CharIn(pt))
				{
					Roofs[i].Open = true;
					PlayOpenRoofSound(Roofs[i]);
				}
				else if (Roofs[i].Open == true && IsAngry == false
					&& Roofs[i].CharIn(pt) == false)
				{
					Roofs[i].Open = false;
					PlayCloseRoofSound(Roofs[i]);
				}
			}

			if (player.X < 0 || player.X + 1 >= XleCore.Map.Width ||
				player.Y < 0 || player.Y + 1 >= XleCore.Map.Height)
			{
				if (IsAngry && this.GetType().Equals(typeof(Town)))
				{
					player.LastAttackedMapID = this.MapID;
				}

				LeaveMap(player);
			}
		}

		public override void OnAfterEntry(GameState state)
		{
			if (MapID == state.Player.LastAttackedMapID)
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

		protected virtual void PlayCloseRoofSound(Roof roof)
		{
			SoundMan.PlaySound(LotaSound.BuildingClose);
			XleCore.Wait(50);
		}
		protected virtual void PlayOpenRoofSound(Roof roof)
		{
			SoundMan.PlaySound(LotaSound.BuildingOpen);
			XleCore.Wait(50);
		}

		protected virtual void SpeakToGuard(Player player)
		{
			bool handled = false;
			Extender.SpeakToGuard(GameState, ref handled);

			if (handled == false)
			{
				XleCore.TextArea.PrintLine("\n\nThe guard salutes.");
			}
		}


		public override int DrawTile(int xx, int yy)
		{
			int tile = this[xx, yy];

			int roof = RoofTile(xx, yy);

			if (roof != 127 && roof != 0)
				return roof;
			else
				return tile;
		}
		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			base.DrawImpl(x, y, facingDirection, inRect);

			DrawGuards(new Point(x, y), inRect);
		}

		protected void DrawGuards(Point centerPoint, Rectangle inRect)
		{
			Point topLeftMapPt = new Point(centerPoint.X - 11, centerPoint.Y - 7);

			int px = inRect.Left;
			int py = inRect.Top;

			for (int i = 0; i < Guards.Count; i++)
			{
				Guard guard = Guards[i];

				if (PointInRoof(guard.X, guard.Y) == -1)
				{
					var facing = guard.Facing;

					int rx = px + (guard.X - topLeftMapPt.X) * 16;
					int ry = py + (guard.Y - topLeftMapPt.Y) * 16;

					if (rx >= inRect.Left && ry >= inRect.Top && rx <= inRect.Right - 32 && ry <= inRect.Bottom - 32)
					{
						XleCore.Renderer.DrawCharacterSprite(rx, ry, facing, true, Guards.AnimFrame, false, guard.Color);
					}
				}
			}
		}
	}
}
