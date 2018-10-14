using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AgateLib.Mathematics.Geometry;
using AgateLib.InputLib;

using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands.Implementation;
using ERY.Xle.Services.XleSystem;

namespace ERY.Xle.Maps.Towns
{
    [ServiceName("FightAgainstGuard")]
    public class FightAgainstGuard : Fight
    {
        public IXleInput Input { get; set; }

        Town TheMap { get { return (Town)GameState.Map; } }
        TownExtender Town { get { return (TownExtender)GameState.MapExtender; } }
        GuardList guards { get { return GameState.Map.Guards; } }

        bool IsAngry
        {
            get { return Town.IsAngry; }
            set { Town.IsAngry = value; }
        }

        public override void Execute()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            TextArea.PrintLine("Fight with " + Player.CurrentWeapon.BaseName(Data));
            TextArea.Print("Enter direction: ");

            Keys key = Input.WaitForKey(Keys.Up, Keys.Down, Keys.Left, Keys.Right);

            Direction fightDir;

            switch (key)
            {
                case Keys.Right: fightDir = Direction.East; break;
                case Keys.Up: fightDir = Direction.North; break;
                case Keys.Left: fightDir = Direction.West; break;
                case Keys.Down: fightDir = Direction.South; break;
                default:
                    throw new InvalidOperationException();
            }

            TextArea.PrintLine(fightDir.ToString());

            FightInDirection(fightDir);

        }

        protected virtual void FightInDirection(Direction fightDir)
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

    }
}
