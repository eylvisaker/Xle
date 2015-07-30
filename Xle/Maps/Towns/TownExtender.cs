using System;

using AgateLib.Geometry;
using AgateLib.InputLib;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Towns
{
    public class TownExtender : Map2DExtender
    {
        public new Town TheMap { get { return (Town)base.TheMap; } }

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

        public override void AfterExecuteCommand(KeyCode cmd)
        {
            base.AfterExecuteCommand(cmd);

            UpdateGuards();
        }

        public override void OnAfterEntry()
        {
            if (TheMap.MapID == Player.LastAttackedMapID)
            {
                IsAngry = true;

                TextArea.Clear(true);
                TextArea.PrintLine("\nWe remember you - slime!");

                GameControl.Wait(2000);
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

        public override bool UseFancyMagicPrompt
        {
            get { return true; }
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

        public void UpdateGuards()
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

                    GuardAttackPlayer(guard);
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
        void AttackGuard(int grd, int distance)
        {
            AttackGuard(TheMap.Guards[grd], distance);
        }
        void AttackGuard(Guard guard, int distance)
        {
            if (guard.OnPlayerAttack != null)
            {
                bool cancel = guard.OnPlayerAttack(GameState, guard);
                if (cancel)
                    return;
            }

            double hitChance = ChanceToHitGuard(guard, distance);


            if (Random.NextDouble() > hitChance)
            {
                TextArea.PrintLine("Attack on " + guard.Name + " missed", XleColor.Purple);
                SoundMan.PlaySound(LotaSound.PlayerMiss);
            }
            else
            {
                int dam = RollDamageToGuard(guard);

                IsAngry = true;
                Player.LastAttackedMapID = TheMap.MapID;

                TextArea.Print(guard.Name + " struck  ", XleColor.Yellow);
                TextArea.Print(dam.ToString(), XleColor.White);
                TextArea.Print("  H.P. Blow", XleColor.White);
                TextArea.PrintLine();

                guard.HP -= dam;

                SoundMan.PlaySound(LotaSound.PlayerHit);

                if (guard.HP <= 0)
                {
                    TextArea.PrintLine(guard.Name + " killed");

                    TheMap.Guards.Remove(guard);

                    GameControl.Wait(100);

                    SoundMan.StopSound(LotaSound.PlayerHit);
                    SoundMan.PlaySound(LotaSound.EnemyDie);

                    if (guard.OnGuardDead != null)
                        guard.OnGuardDead(GameState, guard);
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
            GameControl.Wait(50);
        }
        public virtual void PlayOpenRoofSound(Roof roof)
        {
            SoundMan.PlaySound(LotaSound.BuildingOpen);
            GameControl.Wait(50);
        }

        public virtual void SpeakToGuard()
        {
            TextArea.PrintLine("\n\nThe guard salutes.");
        }

        public virtual void GuardAttackPlayer(Guard guard)
        {
            TextArea.PrintLine();

            TextArea.Print("Attacked by " + guard.Name + "! -- ", XleColor.White);

            if (Random.NextDouble() > ChanceToHitPlayer(guard))
            {
                TextArea.Print("Missed", XleColor.Cyan);
                SoundMan.PlaySound(LotaSound.EnemyMiss);
            }
            else
            {
                int dam = RollDamageToPlayer(guard);

                TextArea.Print("Blow ", XleColor.Yellow);
                TextArea.Print(dam.ToString(), XleColor.White);
                TextArea.Print(" H.P.", XleColor.White);

                SoundMan.PlaySound(LotaSound.EnemyHit);

                Player.HP -= dam;
            }

            TextArea.PrintLine();

            GameControl.Wait(100 * Player.Gamespeed);
        }

        protected override void PlayerFight(Direction fightDir)
        {
            bool attacked = false;
            int maxXdist = 1;
            int maxYdist = 1;
            int tile = 0, tile1;
            int hit = 0;

            if (Player.CurrentWeapon.Info(Data).Ranged)
            {
                maxXdist = 12;
                maxYdist = 8;
            }

            Player.FaceDirection = fightDir;

            Point attackPt = Point.Empty, attackPt2 = Point.Empty;

            attackPt.X = Player.X;
            attackPt.Y = Player.Y;
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
                            AttackGuard(k, Math.Max(i, j));
                            attacked = true;

                            GameControl.Wait(200);
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

                        int dam = Random.Next(10) + 30;

                        TextArea.PrintLine();
                        TextArea.PrintLine("Merchant killed by blow of " + dam.ToString());

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
                        TextArea.PrintLine("The prison bars hold.");

                        SoundMan.PlaySound(LotaSound.Bump);

                        attacked = true;
                    }
                }

            }

            if (attacked == false)
            {
                TextArea.PrintLine("Nothing hit");
            }

            GameControl.Wait(200 + 50 * Player.Gamespeed, keyBreak: true);
        }

        protected override void AfterStepImpl(bool didEvent)
        {
            Point pt = new Point(Player.X, Player.Y);
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

            if (Player.X < 0 || Player.X >= TheMap.Width - 1 ||
                Player.Y < 0 || Player.Y >= TheMap.Height - 1)
            {
                if (IsAngry && this.GetType().Equals(typeof(Town)))
                {
                    Player.LastAttackedMapID = TheMap.MapID;
                }

                LeaveMap();
            }
        }

        protected override bool PlayerSpeakImpl()
        {
            var guards = TheMap.Guards;

            for (int j = -1; j < 3; j++)
            {
                for (int i = -1; i < 3; i++)
                {
                    foreach (var guard in guards)
                    {
                        if ((guard.X == Player.X + i || guard.X + 1 == Player.X + i) &&
                            (guard.Y == Player.Y + j || guard.Y + 1 == Player.Y + j))
                        {
                            SpeakToGuard();
                            return true;

                        }
                    }
                }
            }

            return false;
        }

    }
}
