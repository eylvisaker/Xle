﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using AgateLib;
using AgateLib.InputLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;


namespace ERY.Xle.XleMapTypes
{
	public class Town : XleMap, ERY.Xle.IHasGuards, ERY.Xle.IHasRoofs
	{
		int mWidth;
		int mHeight;
		int[] mData;
		int mOutsideTile = 0;

		List<Roof> mRoofs = new List<Roof>();
		List<Guard> mGuards = new List<Guard>();

		List<int> mMail = new List<int>();				// towns to carry mail to



		#region --- Construction and Serialization ---

		public Town()
		{ }

		protected override void WriteData(XleSerializationInfo info)
		{
			info.Write("Width", mWidth);
			info.Write("Height", mHeight);
			info.Write("OutsideTile", mOutsideTile);
			info.Write("MapData", mData);
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

		#endregion

		public override IEnumerable<string> AvailableTilesets
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

				if (mIsAngry)
				{
					g.invisible = false;
					g.guard = false;
				}
			}
		}

		public override void AfterExecuteCommand(Player player, KeyCode cmd)
		{
			UpdateGuards(player);
		}
		public void UpdateGuards(Player player)
		{
			if (IsAngry)
			{
				int i, j;
				double dist;
				int xdist;
				int ydist;
				int dx;
				int dy;
				Point newPt;
				bool badPt = false;
				Color[] color = new Color[40];
				string tempString;
				int dam;

				for (i = 0; i < Guards.Count; i++)
				{
					Guard guard = Guards[i];

					if (PointInRoof(guard.X, guard.Y) == -1)
					{
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

						if (Math.Abs(xdist) <= 2 && Math.Abs(ydist) <= 2)
						{

							g.AddBottom("");

							tempString = "Attacked by guard! -- ";
							dam = player.Damage(guard.Attack);

							for (j = 0; j < tempString.Length; j++)
							{
								color[j] = XleColor.White;
							}

							if (dam > 0)
							{
								tempString += "Blow ";

								for (; j < tempString.Length; j++)
								{
									color[j] = XleColor.Yellow;
								}
								tempString += dam;
								tempString += " H.P.";

								for (; j < tempString.Length; j++)
								{
									color[j] = XleColor.White;
								}

								SoundMan.PlaySound(LotaSound.EnemyHit);

							}
							else
							{
								tempString += "Missed";

								for (; j < tempString.Length; j++)
								{
									color[j] = XleColor.Cyan;
								}
								SoundMan.PlaySound(LotaSound.EnemyMiss);
							}


							g.AddBottom(tempString, color);


							XleCore.wait(100 * player.Gamespeed);


						}
						else if (dist < 25)
						{
							if (Math.Abs(xdist) > Math.Abs(ydist))
							{
								newPt.X += dx;
							}
							else
							{
								newPt.Y += dy;
							}

							badPt = !CheckGuard(newPt, i);

							if (badPt == true)
							{
								newPt = guard.Location;

								if (Math.Abs(xdist) > Math.Abs(ydist))
								{
									xdist = 0;

									if (ydist == 0)
									{
										dy = XleCore.random.Next(2) * 2 - 1;
									}

									dx = 0;

									newPt.Y += dy;
								}
								else
								{
									ydist = 0;

									if (xdist == 0)
									{
										dx = XleCore.random.Next(2) * 2 - 1;
									}

									dy = 0;

									newPt.X += dx;
								}
								badPt = !CheckGuard(newPt, i);

								if (badPt == true)
									newPt = guard.Location;

							}

							guard.Location = newPt;

							if (Math.Abs(xdist) > Math.Abs(ydist))
							{
								if (dx < 0)
								{
									guard.Facing = Direction.West;
								}
								else
								{
									guard.Facing = Direction.East;
								}
							}
							else
							{
								if (dy < 0)
								{
									guard.Facing = Direction.North;
								}
								else
								{
									guard.Facing = Direction.South;
								}
							}

						}
					}			// guard(x,y) != (0,0)
				}

			}
		}

		bool CheckGuard(Point pt, int grd)
		{
			int i, j, k;
			Size guardSize = new Size(2, 2);

			for (j = 0; j < 2; j++)
			{
				for (i = 0; i < 2; i++)
				{
					if (this[pt.X + i, pt.Y + j] >= 128 || (this[pt.X + i, pt.Y + j] % 16) >= 7)
					{
						return false;
					}

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

						/*
							if ((guard.X == pt.X - 1 || guard[k].X == pt.X || guard[k].X == pt.X + 1) &&
								(guard.Y == pt.Y - 1 || guard[k].Y == pt.Y || guard[k].Y == pt.Y + 1))
							{
								return false;
							}
						 * */

					}
				}
			}

			return true;
		}
		int AttackGuard(Player player, int grd)
		{
			int dam = 0;
			ColorStringBuilder builder = new ColorStringBuilder();

			dam = player.Hit(Guards[grd].Defense);

			if (dam > 0)
			{
				IsAngry = true;
				//g.player.lastAttacked = MapNumber();

				builder.AddText("Guard struck  ", XleColor.Yellow);
				builder.AddText(dam.ToString(), XleColor.White);

				g.AddBottom(builder);

				Guards[grd].HP -= dam;

				SoundMan.PlaySound(LotaSound.PlayerHit);

				if (Guards[grd].HP <= 0)
				{
					g.AddBottom("Guard killed");

					Guards.RemoveAt(grd);

					XleCore.wait(100);

					SoundMan.StopSound(LotaSound.PlayerHit);
					SoundMan.PlaySound(LotaSound.EnemyDie);

				}

			}
			else
			{
				g.AddBottom("Attack on guard missed", XleColor.Purple);
				SoundMan.PlaySound(LotaSound.PlayerMiss);
			}

			return 0;

		}

		/*
 Point GuardPos(int i)
 {
	 return guad
	 Point tempPt;

	 tempPt.X = guard[i].X;
	 tempPt.Y = guard[i].Y;

	 return tempPt;

 }
 */
		/*
		int SpecialDataLength()
		{
			switch (MapType())
			{
				case MapTypes.Outside:
					return 104;
					break;
				case MapTypes.Town:
				case MapTypes.Castle:
					return 104;
					break;
			}

			return 5;
		}
		*/



		/*
		void CheckRoof(int ptx, int pty)
		{
			if (mapType != MapTypes.Town && mapType != MapTypes.Castle)
			{
				return;
			}

			if (!IsAngry)
			{
				for (int i = 0; i < mRoofs.Count; i++)
					mRoofs[i].Open = false;
			}

			int roofID = InRoof(ptx, pty);

			if (roofID >= 0)
			{
				mRoofs[roofID].Open = true;
			}

		}

		*/

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
				if (mRoofs[i].PointInRoof(ptx, pty, false) && mRoofs[i].Open == false)
				{
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
			for (int i = 0; i < Roofs.Count; i++)
			{
				Roof r = Roofs[i];
				Rectangle boundingRect = r.Rectangle;

				if (r.Open)
					continue;

				if (boundingRect.Contains(new Point(xx, yy)))
				{
					return r[xx - r.X, yy - r.Y];
				}
			}

			return 127;
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
					return mOutsideTile;
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

			Commands.UpdateCommand(command);

			if (player.Move(stepDirection))
				SoundMan.PlaySound(LotaSound.TownWalk);
			else
			{
				SoundMan.PlaySound(LotaSound.Invalid);

				Commands.UpdateCommand("Move Nowhere");
			}

		}
		public override bool PlayerFight(Player player)
		{
			string tempstring;
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
			ColorStringBuilder builder;

			weaponName = player.CurrentWeaponName;


			if (player.WeaponType(player.CurrentWeapon) == 6 ||
				player.WeaponType(player.CurrentWeapon) == 8)
			{
				maxXdist = 12;
				maxYdist = 8;
			}

			g.AddBottom("");

			tempstring = "";

			//TODO: Loadstring(g.hInstance(), player.WeaponType(player.CurrentWeapon()), tempstring, 25);
			tempstring = tempstring.TrimEnd();

			if (tempstring == "")
				tempstring = "Bare hands";


			g.AddBottom("Fight with " + tempstring);
			g.AddBottom("Enter direction: ");

			tempstring = "Enter direction: ";

			KeyCode key = XleCore.WaitForKey(KeyCode.Up, KeyCode.Down, KeyCode.Left, KeyCode.Right);


			attackPt.X = player.X;
			attackPt.Y = player.Y;
			attackPt2.X = attackPt.X;
			attackPt2.Y = attackPt.Y;

			switch (key)
			{
				case KeyCode.Right:
					tempstring += "east";
					player.FaceDirection = Direction.East;
					attackPt.X++;
					attackPt2.X++;
					attackPt2.Y++;
					dx = 1;
					break;
				case KeyCode.Up:
					tempstring += "north";
					player.FaceDirection = Direction.North;
					dy = -1;
					attackPt2.X++;
					break;
				case KeyCode.Left:
					tempstring += "west";
					player.FaceDirection = Direction.West;
					dx = -1;
					attackPt2.Y++;
					break;
				case KeyCode.Down:
					tempstring += "south";
					player.FaceDirection = Direction.South;
					dy = 1;
					attackPt.Y++;
					attackPt2.X++;
					attackPt2.Y++;
					break;
			}

			g.UpdateBottom(tempstring);

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
							AttackGuard(player, k);
							attacked = true;

							XleCore.wait(200);
						}
					}

					tile = this[attackPt.X + dx, attackPt.Y + dy];
					tile1 = this[attackPt2.X + dx, attackPt2.Y + dy];

					int t = RoofTile(attackPt.X + dx, attackPt.Y + dy);
					if (t != 127)
						tile = 128;

					t = RoofTile(attackPt2.X + dx, attackPt2.Y + dy);
					if (t != 127)
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

						tempstring = (string)"Merchant killed by blow of " + dam.ToString();

						g.AddBottom("");
						g.AddBottom(tempstring);

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
						g.AddBottom("The prison bars hold.");

						SoundMan.PlaySound(LotaSound.Bump);

						attacked = true;
					}
				}

			}

			if (attacked == false)
			{
				g.AddBottom("Nothing hit");
			}

			XleCore.wait(XleCore.Redraw, 200 + 50 * player.Gamespeed, true);

			return true;
		}

		public override bool PlayerLeave(Player player)
		{
			if (IsAngry)
			{
				g.AddBottom("");
				g.AddBottom("Walk out yourself.");
			}
			else
			{
				g.AddBottom("");
				g.AddBottom("Leave " + MapName);
				g.AddBottom("");

				XleCore.wait(200);

				player.ReturnToOutside();
			}

			return true;
		}
		public override bool PlayerXamine(Player player)
		{
			g.AddBottom("");
			g.AddBottom("You are in " + XleCore.Map.MapName + ".");
			g.AddBottom("Look about to see more.");

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

				handled = evt.Rob(player);
				IsAngry = true;

				if (handled)
					return handled;
			}

			return PlayerRobImpl(player);
		}
		protected override bool PlayerRobImpl(Player player)
		{
			g.AddBottom();
			g.AddBottom("Nothing to rob");
			XleCore.wait(500);

			return true;
		}

		protected override void PlayerStepImpl(Player player)
		{
			Point pt = new Point(player.X, player.Y);

			for (int i = 0; i < mRoofs.Count; i++)
			{
				if (Roofs[i].Open == false && Roofs[i].CharIn(pt))
				{
					Roofs[i].Open = true;
					OpenRoof(mRoofs[i]);
				}
				else if (Roofs[i].Open == true && IsAngry == false
					&& Roofs[i].CharIn(pt) == false)
				{
					Roofs[i].Open = false;
					CloseRoof(mRoofs[i]);
				}
			}



			if (player.X < 0 || player.X + 1 >= XleCore.Map.Width ||
				player.Y < 0 || player.Y + 1 >= XleCore.Map.Height)
			{

				if (IsAngry && this.GetType().Equals(typeof(Town)))
				{
					player.LastAttacked = this.MapID;
				}

				g.AddBottom("");
				g.AddBottom("Leave " + XleCore.Map.MapName);

				g.AddBottom("");

				XleCore.wait(2000);

				player.ReturnToOutside();

				g.AddBottom("");

			}

		}

		protected virtual void CloseRoof(Roof roof)
		{
			SoundMan.PlaySound(LotaSound.BuildingClose);
			XleCore.wait(50);

		}
		protected virtual void OpenRoof(Roof roof)
		{
			SoundMan.PlaySound(LotaSound.BuildingOpen);
			XleCore.wait(50);

		}

		protected virtual void SpeakToGuard(Player player)
		{
			g.AddBottom("");
			g.AddBottom("The guard salutes.");
		}

		public override bool CanPlayerStepInto(Player player, int xx, int yy)
		{
			int test = 0;
			int t = 0;

			if (GuardInSpot(xx, yy))
				t = 3;


			for (int j = 0; j < 2; j++)
			{
				for (int i = 0; i < 2; i++)
				{

					int xLimit = 8;
					const int yLimit = 8;

					//test = (int)(Lota.Map.M(yy + j, xx + i) / 16) * 16;
					test = XleCore.Map[xx + i, yy + j];

					if (test >= 16 * yLimit || test % 16 >= xLimit)
					{
						t = 3;
					}
				}
			}

			if (t > 0)
			{
				return false;
			}

			return true;
		}

		public override int DrawTile(int xx, int yy)
		{
			int tile = this[xx, yy];


			int roof = RoofTile(xx, yy);

			if (roof != 127)
				return roof;
			else
				return tile;
		}
		protected override void DrawImpl(int x, int y, Direction facingDirection, Rectangle inRect)
		{
			Draw2D(x, y, facingDirection,  inRect);

			DrawGuards(new Point(x, y), inRect);

		}
		public void AnimateGuards()
		{
			int animTime = 1000;

			if (IsAngry)
				animTime = 150;

			if (lastGuardAnim + animTime <= Timing.TotalMilliseconds)
			{
				guardAnim++;
				if (guardAnim > 2)
					guardAnim = 0;

				lastGuardAnim = Timing.TotalMilliseconds;
			}
		}
		protected void DrawGuards(Point centerPoint, Rectangle inRect)
		{
			int px = inRect.Left + (inRect.Width / 16) / 2 * 16;
			int py = inRect.Top + inRect.Height / 2;

			for (int i = 0; i < Guards.Count; i++)
			{
				Guard guard = Guards[i];

				if (PointInRoof(guard.X, guard.Y) == -1)
				{
					int tx = guardAnim * 32;
					int ty;

					if (IsAngry)
						ty = ((int)guard.Facing + 3) * 32;
					else
						ty = 7 * 32;

					Rectangle charRect = new Rectangle(tx, ty, 32, 32);

					int rx = px - (centerPoint.X - guard.X) * 16;
					int ry = py - (centerPoint.Y - guard.Y) * 16;

					if (rx >= inRect.Left && ry >= inRect.Top && rx <= inRect.Right - 32 && ry < inRect.Bottom)
					{
						Rectangle destRect = new Rectangle(rx, ry, 32, 32);
						g.Character.Draw(charRect, destRect);
					}
				}
			}
		}

		public override void UpdateAnim()
		{
			AnimateGuards();
		}
		public override string[] MapMenu()
		{
			List<string> retval = new List<string>();

			retval.Add("Armor");
			retval.Add("Fight");
			retval.Add("Gamespeed");
			retval.Add("Hold");
			retval.Add("Inventory");
			retval.Add("Leave");
			retval.Add("Magic");
			retval.Add("Pass");
			retval.Add("Rob");
			retval.Add("Speak");
			retval.Add("Use");
			retval.Add("Weapon");
			retval.Add("Xamine");

			return retval.ToArray();
		}

		protected override bool CheckMovementImpl(Player player, int dx, int dy)
		{
			return true;
		}

		public override void GetBoxColors(out Color boxColor, out Color innerColor, out Color fontColor, out int vertLine)
		{
			fontColor = XleColor.White;


			boxColor = XleColor.Orange;
			innerColor = XleColor.Yellow;
			vertLine = 13 * 16;
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