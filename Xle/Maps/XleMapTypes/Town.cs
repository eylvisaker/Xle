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
	public class Town : XleMap, IHasGuards, IHasRoofs
	{
		int mWidth;
		int mHeight;
		int[] mData;
		int mOutsideTile = 0;

		List<Roof> mRoofs = new List<Roof>();
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
		}

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth);
			info.Write("Height", mHeight);
			info.Write("OutsideTile", mOutsideTile);
			info.Write("MapData", mData, NumericEncoding.Csv);
			info.Write("Mail", mMail.ToArray());
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			mWidth = info.ReadInt32("Width");
			mHeight = info.ReadInt32("Height");
			mOutsideTile = info.ReadInt32("OutsideTile");
			mData = info.ReadInt32Array("MapData");

			try
			{
				mMail.AddRange(info.ReadInt32Array("Mail"));
			}
			catch (XleSerializationException)
			{ }
		}

		public void InitializeGuardData()
		{
			foreach (var guard in Guards)
			{
				guard.Attack = DefaultAttack;
				guard.Defense = DefaultDefense;
				guard.HP = DefaultHP;
				guard.Color = DefaultColor;
				guard.Facing = Direction.South;

				if (guard.Color.ToArgb() == 0)
					guard.Color = XleColor.Yellow;
			}
		}

		#endregion

		public override IEnumerable<string> AvailableTileImages
		{
			get
			{
				yield return "towntiles.png";
			}
		}

		double lastGuardAnim = 0;

		public List<Roof> Roofs
		{
			get { return mRoofs; }
			set { mRoofs = value; }
		}

		bool mIsAngry = false;					// whether or not the guards are chasing the player

		int guardAnim;


		public List<Guard> Guards
		{
			get { return mGuards; }
			set { mGuards = value; }
		}
		public bool GuardInSpot(int x, int y)
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
			get { return mIsAngry; }
			set
			{
				mIsAngry = value;

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

			double dist;
			int xdist;
			int ydist;
			int dx;
			int dy;
			Point newPt;
			bool badPt = false;

			for (int i = 0; i < Guards.Count; i++)
			{
				Guard guard = Guards[i];

				if (PointInRoof(guard.X, guard.Y) != -1)
					continue;

				badPt = false;

				newPt = guard.Location;

				xdist = player.X - guard.X;
				ydist = player.Y - guard.Y;

				if (xdist != 0)
					dx = xdist / Math.Abs(xdist);
				else dx = 0;
				if (ydist != 0)
					dy = ydist / Math.Abs(ydist);
				else dy = 0;

				dist = Math.Sqrt(Math.Pow(xdist, 2) + Math.Pow(ydist, 2));
				if (dist >= 25)
					continue;

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

					badPt = !CanGuardStepInto(newPt, i);

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
						badPt = !CanGuardStepInto(newPt, i);

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

		private void GuardAttackPlayer(Player player, Guard guard)
		{
			if (guard.SkipAttacking)
				return;

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

		bool CanGuardStepInto(Point pt, int grd)
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
					Rectangle grdRect = new Rectangle(pt, guardSize);

					for (k = 0; k < Guards.Count; k++)
					{
						if (k == grd)
							continue;

						Guard guard = Guards[k];
						Rectangle guardRect = new Rectangle(guard.Location, guardSize);

						if (grdRect.IntersectsWith(guardRect))
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
				player.LastAttacked = MapID;

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
		int PointInRoof(int ptx, int pty)
		{
			for (int i = 0; i < mRoofs.Count; i++)
			{
				if (mRoofs[i].PointInRoof(ptx, pty, false))
				{
					if (mRoofs[i].Open)
						return -1;
					else
						return i;
				}
			}

			return -1;

			/*
			if (pty >= 0 && pty < Height && ptx >= 0 && ptx < Width)
			{
				//roof = mRoof[pty, ptx];

				if (mRoofs[roof].Open)
				{
					roof = -1;
				}

			}
			return roof;
			*/

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


		public List<int> Mail
		{
			get { return mMail; }
			set { mMail = value; }
		}


		public override void InitializeMap(int width, int height)
		{
			mWidth = width;
			mHeight = height;

			mData = new int[mWidth * mHeight];
		}

		public override int Height
		{
			get { return mHeight; }
		}
		public override int Width
		{
			get { return mWidth; }
		}
		public int OutsideTile
		{
			get { return mOutsideTile; }
			set { mOutsideTile = value; }
		}

		public override int this[int xx, int yy]
		{
			get
			{
				if (yy < 0 || yy >= Height || xx < 0 || xx >= Width)
				{
					var outsideTile = mBaseExtender.GetOutsideTile(centerPoint, xx, yy);

					if (outsideTile == -1)
						return mOutsideTile;
					else
						return outsideTile;
				}
				else
				{
					return mData[xx + yy * mWidth];
				}
			}
			set
			{
				if (yy < 0 || yy >= Height ||
					xx < 0 || xx >= Width)
				{
					return;
				}
				else
				{
					mData[xx + yy * mWidth] = value;
				}
			}
		}

		public override void PlayerCursorMovement(Player player, Direction dir)
		{
			string command;
			Point stepDirection;

			_Move2D(player, dir, "Move", out command, out stepDirection);

			XleCore.TextArea.PrintLine(command);

			if (player.Move(stepDirection))
			{
				SoundMan.PlaySound(LotaSound.WalkTown);

				player.TimeQuality += 0.03;
			}
			else
			{
				SoundMan.PlaySound(LotaSound.Invalid);

				Commands.CommandList.UpdateCommand("Move Nowhere");
			}

		}
		public override bool PlayerFight(Player player)
		{
			string weaponName;
			Point attackPt = Point.Empty, attackPt2 = Point.Empty;
			int i = 0, j = 0;
			int dx = 0, dy = 0;
			bool attacked = false;
			int maxXdist = 1;
			int maxYdist = 1;
			int tile = 0, tile1;
			int hit = 0;
			Color[] colors = new Color[40];

			weaponName = player.CurrentWeaponTypeName;

			if (player.WeaponType(player.CurrentWeapon) == 6 ||
				player.WeaponType(player.CurrentWeapon) == 8)
			{
				maxXdist = 12;
				maxYdist = 8;
			}

			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();

			XleCore.TextArea.PrintLine("Fight with " + player.CurrentWeaponTypeName);
			XleCore.TextArea.Print("Enter direction: ");

			KeyCode key = XleCore.WaitForKey(KeyCode.Up, KeyCode.Down, KeyCode.Left, KeyCode.Right);


			attackPt.X = player.X;
			attackPt.Y = player.Y;
			attackPt2.X = attackPt.X;
			attackPt2.Y = attackPt.Y;

			switch (key)
			{
				case KeyCode.Right:
					player.FaceDirection = Direction.East;
					attackPt.X++;
					attackPt2.X++;
					attackPt2.Y++;
					dx = 1;
					break;
				case KeyCode.Up:
					player.FaceDirection = Direction.North;
					dy = -1;
					attackPt2.X++;
					break;
				case KeyCode.Left:
					player.FaceDirection = Direction.West;
					dx = -1;
					attackPt2.Y++;
					break;
				case KeyCode.Down:
					player.FaceDirection = Direction.South;
					dy = 1;
					attackPt.Y++;
					attackPt2.X++;
					attackPt2.Y++;
					break;
			}

			XleCore.TextArea.PrintLine(player.FaceDirection.ToString());

			for (i = 1; i <= maxXdist && attacked == false && tile < 128; i++)
			{
				for (j = 1; j <= maxYdist && attacked == false && tile < 128; j++)
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

			return true;
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
		public override bool PlayerRob(Player player)
		{
			XleEvent evt = GetEvent(player, 1);
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

			return PlayerRobImpl(player);
		}
		protected override bool PlayerRobImpl(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Nothing to rob");
			XleCore.Wait(500);

			return true;
		}

		protected override void PlayerStepImpl(Player player, bool didEvent)
		{
			Point pt = new Point(player.X, player.Y);

			for (int i = 0; i < mRoofs.Count; i++)
			{
				if (Roofs[i].Open == false && Roofs[i].CharIn(pt))
				{
					Roofs[i].Open = true;
					PlayOpenRoofSound(mRoofs[i]);
				}
				else if (Roofs[i].Open == true && IsAngry == false
					&& Roofs[i].CharIn(pt) == false)
				{
					Roofs[i].Open = false;
					PlayCloseRoofSound(mRoofs[i]);
				}
			}

			if (player.X < 0 || player.X + 1 >= XleCore.Map.Width ||
				player.Y < 0 || player.Y + 1 >= XleCore.Map.Height)
			{
				if (IsAngry && this.GetType().Equals(typeof(Town)))
				{
					player.LastAttacked = this.MapID;
				}

				LeaveMap(player);
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

		public override bool CanPlayerStepInto(Player player, int xx, int yy)
		{
			int test = 0;

			if (GuardInSpot(xx, yy))
				return false;

			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{
					test = this[xx + i, yy + j];

					if (IsTileBlocked(test))
						return false;
				}
			}

			return true;
		}

		private bool IsTileBlocked(int tile)
		{
			return TileSet[tile] == TileInfo.Blocked;
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
			Draw2D(x, y, facingDirection, inRect);

			DrawGuards(new Point(x, y), inRect);
		}
		public void AnimateGuards()
		{
			int animTime = 460;

			if (IsAngry)
				animTime = 150;

			if (lastGuardAnim + animTime <= Timing.TotalMilliseconds)
			{
				guardAnim++;

				lastGuardAnim = Timing.TotalMilliseconds;
			}
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
						XleCore.Renderer.DrawCharacterSprite(rx, ry, facing, true, guardAnim, false, guard.Color);
					}
				}
			}
		}

		protected override void AnimateTiles(Rectangle rectangle)
		{
			AnimateGuards();
			base.AnimateTiles(rectangle);
		}

		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			return true;
		}


		#region IHasGuards Members

		int attack, defense, hp;
		Color clr;

		public int DefaultAttack
		{
			get { return attack; }
			set { attack = value; }
		}
		public int DefaultDefense
		{
			get { return defense; }
			set { defense = value; }
		}

		public int DefaultHP
		{
			get { return hp; }
			set { hp = value; }
		}

		public new Color DefaultColor
		{
			get { return clr; }
			set { clr = value; }
		}

		#endregion



	}
}
