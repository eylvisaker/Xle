using AgateLib.Mathematics.Geometry;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;

namespace Xle.Maps.Towns
{
    public class TownExtender : Map2DExtender
    {
        public new Town TheMap
        {
            get { return (Town)base.TheMap; }
            set { base.TheMap = value; }
        }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.TownRenderer(this);
        }

        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.White;

            scheme.FrameColor = XleColor.Orange;
            scheme.FrameHighlightColor = XleColor.Yellow;
        }

        public override async Task AfterExecuteCommand(Keys cmd)
        {
            await base.AfterExecuteCommand(cmd);

            await UpdateGuards();
        }

        public override async Task OnAfterEntry()
        {
            if (TheMap.MapID == Player.LastAttackedMapID)
            {
                IsAngry = true;

                TextArea.Clear(true);
                await TextArea.PrintLine("\nWe remember you - slime!");

                await GameControl.WaitAsync(2000);
            }
            else
            {
                Player.LastAttackedMapID = 0;
            }
        }

        public virtual double ChanceToHitGuard(Guard guard, int distance)
        {
            int weaponType = Player.CurrentWeapon.ID;

            return (Player.Attribute[Attributes.dexterity] + 16)
                * (99 + weaponType * 8) / 7000.0 / guard.Defense * 99;
        }

        public virtual int RollDamageToGuard(Guard guard)
        {
            int weaponType = Player.CurrentWeapon.ID;

            double damage = 1 + Player.Attribute[Attributes.strength] *
                       (weaponType / 2 + 1) / 4;

            damage *= 0.5 + Random.NextDouble();

            return (int)Math.Round(damage);
        }

        protected override bool GuardInSpot(int x, int y)
        {
            foreach (Guard g in TheMap.Guards)
            {
                if ((g.X == x - 1 || g.X == x || g.X == x + 1) &&
                    (g.Y == y - 1 || g.Y == y || g.Y == y + 1))
                {
                    return true;
                }
            }

            return false;
        }

        public async Task UpdateGuards()
        {
            if (IsAngry == false)
                return;

            foreach (var guard in TheMap.Guards)
            {
                if (guard.SkipMovement)
                    continue;
                if (TheMap.ClosedRoofAt(guard.X, guard.Y) != null)
                    continue;

                bool badPt = false;

                int xdist = Player.X - guard.X;
                int ydist = Player.Y - guard.Y;

                int dx = 0;
                int dy = 0;

                if (xdist != 0)
                    dx = xdist / Math.Abs(xdist);
                else dx = 0;
                if (ydist != 0)
                    dy = ydist / Math.Abs(ydist);
                else dy = 0;

                if (Math.Abs(xdist) > 12) continue;
                if (Math.Abs(ydist) > 9) continue;

                Point newPt = guard.Location;

                if (Math.Abs(xdist) <= 2 && Math.Abs(ydist) <= 2)
                {
                    if (Math.Abs(xdist) > Math.Abs(ydist))
                        dy = 0;
                    else
                        dx = 0;

                    await GuardAttackPlayer(guard);
                }
                else
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
                                dy = Random.Next(2) * 2 - 1;
                            else
                                dy = ydist / Math.Abs(ydist);

                            dx = 0;

                            newPt.Y += dy;
                        }
                        else
                        {
                            if (xdist == 0)
                                dx = Random.Next(2) * 2 - 1;
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

                    foreach (var otherGuard in TheMap.Guards)
                    {
                        if (otherGuard == guard)
                            continue;

                        Rectangle otherGuardRect = new Rectangle(otherGuard.Location, guardSize);

                        if (guardRect.Intersects(otherGuardRect))
                            return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns the index of the roof the character standing at the point
        /// ptx, pty.  -1 if no roof.
        /// </summary>
        /// <param name="ptx"></param>
        /// <param name="pty"></param>
        /// <returns></returns>
        private int CharInRoof(int ptx, int pty)
        {
            for (int i = 0; i < TheMap.Roofs.Count; i++)
            {
                Roof r = TheMap.Roofs[i];

                if (r.CharIn(ptx, pty, false))
                    return i;
            }

            return -1;
        }

        public virtual async Task PlayCloseRoofSound(Roof roof)
        {
            SoundMan.PlaySound(LotaSound.BuildingClose);
            await GameControl.WaitAsync(50);
        }
        public virtual async Task PlayOpenRoofSound(Roof roof)
        {
            SoundMan.PlaySound(LotaSound.BuildingOpen);
            await GameControl.WaitAsync(50);
        }

        public virtual async Task GuardAttackPlayer(Guard guard)
        {
            await TextArea.PrintLine();

            await TextArea.Print("Attacked by " + guard.Name + "! -- ", XleColor.White);

            if (Random.NextDouble() > ChanceToHitPlayer(guard))
            {
                await TextArea.Print("Missed", XleColor.Cyan);
                SoundMan.PlaySound(LotaSound.EnemyMiss);
            }
            else
            {
                int dam = RollDamageToPlayer(guard);

                await TextArea.Print("Blow ", XleColor.Yellow);
                await TextArea.Print(dam.ToString(), XleColor.White);
                await TextArea.Print(" H.P.", XleColor.White);

                SoundMan.PlaySound(LotaSound.EnemyHit);

                Player.HP -= dam;
            }

            await TextArea.PrintLine();

            await GameControl.WaitAsync(100 * Player.Gamespeed);
        }

        protected override async Task AfterStepImpl(bool didEvent)
        {
            Point pt = new Point(Player.X, Player.Y);
            var roofs = TheMap.Roofs;

            for (int i = 0; i < roofs.Count; i++)
            {
                if (roofs[i].Open == false && roofs[i].CharIn(pt))
                {
                    roofs[i].Open = true;
                    await PlayOpenRoofSound(roofs[i]);
                }
                else if (roofs[i].Open == true && IsAngry == false
                    && roofs[i].CharIn(pt) == false)
                {
                    roofs[i].Open = false;
                    await PlayCloseRoofSound(roofs[i]);
                }
            }

            if (Player.X < 0 || Player.X >= TheMap.Width - 1 ||
                Player.Y < 0 || Player.Y >= TheMap.Height - 1)
            {
                if (IsAngry && this.GetType().Equals(typeof(Town)))
                {
                    Player.LastAttackedMapID = TheMap.MapID;
                }

                await LeaveMap();
            }
        }
    }
}
