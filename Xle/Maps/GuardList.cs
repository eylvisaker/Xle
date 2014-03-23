using AgateLib;
using AgateLib.Geometry;
using AgateLib.Serialization.Xle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps
{
	public class GuardList : IXleSerializable, IList<Guard>
	{
		double lastGuardAnim = 0;
		int guardAnimFrame;

		List<Guard> mGuards = new List<Guard>();

		public GuardList()
		{
		}


		void IXleSerializable.WriteData(XleSerializationInfo info)
		{
			info.Write("GuardDefaultAttack", DefaultAttack);
			info.Write("GuardDefaultDefense", DefaultDefense);
			info.Write("GuardDefaultHP", DefaultHP);
			info.Write("GuardDefaultColor", DefaultColor.ToArgb());
			info.Write("Guards", mGuards);
		}
		void IXleSerializable.ReadData(XleSerializationInfo info)
		{
			mGuards = info.ReadList<Guard>("Guards");
			DefaultAttack = info.ReadInt32("GuardDefaultAttack");
			DefaultColor = Color.FromArgb(info.ReadInt32("GuardDefaultColor"));
			DefaultDefense = info.ReadInt32("GuardDefaultDefense");
			DefaultHP = info.ReadInt32("GuardDefaultHP");

			InitializeGuardData();
		}

		public XleMap TheMap { get; set; }
		
		public int DefaultAttack { get; set; }
		public int DefaultDefense { get; set; }
		public int DefaultHP { get; set; }
		public Color DefaultColor { get; set; }

		public bool IsAngry { get; set; }

		public void AnimateGuards()
		{
			int animTime = 460;

			if (IsAngry)
				animTime = 150;

			if (lastGuardAnim + animTime <= Timing.TotalMilliseconds)
			{
				guardAnimFrame++;

				lastGuardAnim = Timing.TotalMilliseconds;
			}
		}

		public bool GuardInSpot(int x, int y)
		{
			foreach(var guard in this)
			{
				if ((guard.X == x - 1 || guard.X == x || guard.X == x + 1) &&
					(guard.Y == y - 1 || guard.Y == y || guard.Y == y + 1))
				{
					return true;
				}
			}

			return false;
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

			foreach(var guard in this)
			{
				if (TheMap.ClosedRoofAt(guard.X, guard.Y) != null)
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

		private bool CanGuardStepInto(Point pt, Guard guard)
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

					foreach(var otherGuard in this)
					{
						if (guard == otherGuard)
							continue;

						Rectangle otherGuardRect = new Rectangle(otherGuard.Location, guardSize);

						if (guardRect.IntersectsWith(otherGuardRect))
							return false;
					}
				}
			}

			return true;
		}

		private void GuardAttackPlayer(Player player, Guard guard)
		{
			if (guard.SkipAttacking)
				return;

			TheMap.GuardAttackPlayer(player, guard);
		}

		public void InitializeGuardData()
		{
			foreach (var guard in this)
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


		public int IndexOf(Guard item)
		{
			return mGuards.IndexOf(item);
		}

		void IList<Guard>.Insert(int index, Guard item)
		{
			mGuards.Insert(index, item);
		}

		void IList<Guard>.RemoveAt(int index)
		{
			mGuards.RemoveAt(index);
		}

		public Guard this[int index]
		{
			get { return mGuards[index]; }
			set { mGuards[index] = value; }
		}

		public void Add(Guard item)
		{
			mGuards.Add(item);
		}

		public void Clear()
		{
			mGuards.Clear();
		}

		public bool Contains(Guard item)
		{
			return mGuards.Contains(item);
		}

		void ICollection<Guard>.CopyTo(Guard[] array, int arrayIndex)
		{
			mGuards.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return mGuards.Count; }
		}

		bool ICollection<Guard>.IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(Guard item)
		{
			return mGuards.Remove(item);
		}

		public IEnumerator<Guard> GetEnumerator()
		{
			return mGuards.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int AnimFrame { get { return guardAnimFrame; } }
	}
}
