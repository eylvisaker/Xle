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
		public new TownExtender Extender { get; protected set; }

		public List<int> Mail { get;set;}

		protected override MapExtender CreateExtenderImpl()
		{
			if (XleCore.Factory == null)
			{
				Extender = new TownExtender();
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

			info.Write("Mail", Mail.ToArray());
		}
		protected override void ReadData(XleSerializationInfo info)
		{
			base.ReadData(info);

			Mail = info.ReadInt32Array("Mail").ToList();
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
			Extender.UpdateGuards(player);
		}
		public void UpdateGuards(Player player)
		{
			Extender.UpdateGuards(player);
		}

		public override void GuardAttackPlayer(Player player, Guard guard)
		{
			Extender.GuardAttackPlayer(player, guard);

		}
		protected override void PlayerFight(Player player, Direction fightDir)
		{
			throw new NotImplementedException();
		}

		public override bool PlayerLeave(Player player)
		{
			return Extender.PlayerLeave(XleCore.GameState);
		}
		public override bool PlayerXamine(Player player)
		{
			return Extender.PlayerXamine(XleCore.GameState);
		}
		protected override bool PlayerSpeakImpl(Player player)
		{
			throw new NotImplementedException();
			//return Extender.PlayerSpeakImpl(XleCore.GameState);			
		}
		public override bool PlayerRob(GameState state)
		{
			return Extender.PlayerRob(state);
		}
		protected override void AfterStepImpl(Player player, bool didEvent)
		{
			throw new NotImplementedException();
			//Extender.AfterStepImpl(player, didEvent);
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
			Extender.PlayCloseRoofSound(roof);
		}
		protected virtual void PlayOpenRoofSound(Roof roof)
		{
			Extender.PlayOpenRoofSound(roof);
		}

		protected virtual void SpeakToGuard(Player player)
		{
			Extender.SpeakToGuard(XleCore.GameState);
		}


		public int RoofTile(int xx, int yy)
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
		public override int TileToDraw(int xx, int yy)
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
