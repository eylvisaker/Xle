using System;
using System.Linq;

using AgateLib.Geometry;
using AgateLib.InputLib;

using ERY.Xle.Data;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;
using ERY.Xle.Services.ScreenModel;

namespace ERY.Xle.Maps.Dungeons
{
    public class DungeonExtender : Map3DExtender
    {
        public DungeonExtender()
        {
            Combat = new DungeonCombat();
        }

        public IStatsDisplay StatsDisplay { get; set; }

        public new Dungeon TheMap { get { return (Dungeon)base.TheMap; } set { base.TheMap = value; } }

        public DungeonCombat Combat { get; set; }

        public override XleMapRenderer CreateMapRenderer(IMapRendererFactory factory)
        {
            return factory.DungeonRenderer(this);
        }

        protected override void PlayPlayerMoveSound()
        {
            SoundMan.PlaySound(LotaSound.WalkDungeon);
        }

        public override Map3DSurfaces Surfaces()
        {
            return null;
        }

        public virtual void OnPlayerExitDungeon()
        {
        }

        public virtual void OnBeforeGiveItem(ref int treasure, ref bool handled, ref bool clearBox)
        {
        }

        public override void OnLoad()
        {
            base.OnLoad();

            CurrentLevel = Player.DungeonLevel;
        }

        public virtual string TrapName(int val)
        {
            switch (val)
            {
                case 0x11: return "ceiling hole";
                case 0x12: return "floor hole";
                case 0x13: return "poison gas vent";
                case 0x14: return "slime splotch";
                case 0x15: return "trip wire";
                case 0x16: return "gas vent";
                default: throw new ArgumentException();
            }
        }


        public override void SetColorScheme(ColorScheme scheme)
        {
            scheme.TextColor = XleColor.Cyan;

            scheme.FrameColor = XleColor.DarkGray;
            scheme.FrameHighlightColor = XleColor.Green;

            scheme.MapAreaWidth = 23;
        }


        public virtual int GetTreasure(int dungeonLevel, int chestID)
        {
            return 0;
        }

        public override void CheckSounds()
        {
        }

        public virtual DungeonMonster GetMonsterToSpawn()
        {
            return null;
        }

        int count = 0;
        public virtual bool RollToHitPlayer(DungeonMonster monster)
        {
            count++;
            return count % 2 == 1;
        }

        public virtual int RollDamageToPlayer(DungeonMonster monster)
        {
            return 4;
        }

        public virtual bool SpawnMonsters()
        {
            return true;
        }
        public virtual void UpdateMonsters()
        {
            foreach (var monster in Combat.Monsters.Where(x => x.DungeonLevel == Player.DungeonLevel))
            {
                var delta = new Point(
                    Player.X - monster.Location.X,
                    Player.Y - monster.Location.Y);

                if (Math.Abs(delta.X) + Math.Abs(delta.Y) == 1)
                {
                    MonsterAttackPlayer(monster);
                }
                else
                {
                    if (Math.Abs(delta.X) > Math.Abs(delta.Y))
                    {
                        if (false == TryMonsterStep(monster, new Point(delta.X, 0)))
                        {
                            TryMonsterStep(monster, new Point(0, delta.Y));
                        }
                    }
                    else
                    {
                        if (false == TryMonsterStep(monster, new Point(0, delta.Y)))
                            TryMonsterStep(monster, new Point(delta.X, 0));
                    }
                }
            }

            if (SpawnMonsters() &&
                Combat.Monsters.Count(monst => monst.DungeonLevel == Player.DungeonLevel) < TheMap.MaxMonsters)
            {
                SpawnMonster();
            }
        }

        public virtual bool PrintLevelDuringXamine
        {
            get { return true; }
        }

        private void DungeonLevelText()
        {
            CurrentLevel = Player.DungeonLevel;

            if (TheMap[Player.X, Player.Y] == 0x21) TheMap[Player.X, Player.Y] = 0x11;
            if (TheMap[Player.X, Player.Y] == 0x22) TheMap[Player.X, Player.Y] = 0x12;

            TextArea.PrintLine("\n\nYou are now at level " + (Player.DungeonLevel + 1).ToString() + ".", XleColor.White);
        }

        public int CurrentLevel
        {
            get { return TheMap.CurrentLevel; }
            set { TheMap.CurrentLevel = value; }
        }

        private void OnPlayerAvoidTrap(int x, int y)
        {
            // don't print a message for ceiling holes
            if (TheMap[x, y] == 0x21) return;

            //string name = TrapName(this[x, y]);

            //TextArea.PrintLine();
            //TextArea.PrintLine("You avoid the " + name + ".");
            //XleCore.wait(150);
        }
        private void OnPlayerTriggerTrap(int x, int y)
        {
            // don't trigger ceiling holes
            if (TheMap[x, y] == 0x21) return;

            TheMap[x, y] -= 0x10;
            int damage = 31;
            TextArea.PrintLine();

            if (TheMap[x, y] == 0x12)
            {
                TextArea.PrintLine("You fall through a hidden hole.", XleColor.White);
            }
            else
            {
                TextArea.PrintLine("You're ambushed by a " +
                    TrapName(TheMap[x, y]) + ".", XleColor.White);
                GameControl.Wait(100);
            }

            TextArea.PrintLine("   H.P. - " + damage.ToString(), XleColor.White);
            Player.HP -= damage;

            SoundMan.PlaySound(LotaSound.EnemyHit);
            GameControl.Wait(500);

            if (TheMap[x, y] == 0x12)
            {
                Player.DungeonLevel++;
                DungeonLevelText();
            }
        }

        public override void AfterPlayerStep()
        {
            int val = TheMap[Player.X, Player.Y];

            CurrentLevel = Player.DungeonLevel;

            if (val >= 0x21 && val <= 0x2a)
            {
                OnPlayerTriggerTrap(Player.X, Player.Y);
            }
            else if (val >= 0x11 && val <= 0x1a)
            {
                OnPlayerAvoidTrap(Player.X, Player.Y);
            }

        }

        private void MonsterAttackPlayer(DungeonMonster monster)
        {
            TextArea.PrintLine();

            var delta = new Point(
                monster.Location.X - Player.X,
                monster.Location.Y - Player.Y);

            var forward = Player.FaceDirection.StepDirection();
            var right = Player.FaceDirection.RightDirection();
            var left = Player.FaceDirection.LeftDirection();
            bool allowEffect = false;

            if (delta == forward)
            {
                TextArea.Print("Attacked by ");
                TextArea.Print(monster.Name, XleColor.Yellow);
                TextArea.PrintLine("!");

                allowEffect = true;
            }
            else if (delta == right)
            {
                TextArea.Print("Attacked from the ");
                TextArea.Print("right", XleColor.Yellow);
                TextArea.PrintLine(".");
            }
            else if (delta == left)
            {
                TextArea.Print("Attacked from the ");
                TextArea.Print("left", XleColor.Yellow);
                TextArea.PrintLine(".");
            }
            else
            {
                TextArea.Print("Attacked from ");
                TextArea.Print("behind", XleColor.Yellow);
                TextArea.PrintLine(".");
            }

            if (RollToHitPlayer(monster))
            {
                int damage = RollDamageToPlayer(monster);

                SoundMan.PlaySound(LotaSound.EnemyHit);
                TextArea.Print("Hit by blow of ");
                TextArea.Print(damage.ToString(), XleColor.Yellow);
                TextArea.PrintLine("!");

                Player.HP -= damage;
            }
            else
            {
                SoundMan.PlaySound(LotaSound.EnemyMiss);
                TextArea.PrintLine("Attack missed.", XleColor.Green);
            }

            GameControl.Wait(250);
        }

        private bool TryMonsterStep(DungeonMonster monster, Point delta)
        {
            if (delta.X != 0 && delta.Y != 0) throw new ArgumentOutOfRangeException();
            if (delta.X == 0 && delta.Y == 0) return false;

            if (delta.X != 0) delta.X /= Math.Abs(delta.X);
            if (delta.Y != 0) delta.Y /= Math.Abs(delta.Y);

            if (CanMonsterStepInto(monster, new Point(monster.Location.X + delta.X, monster.Location.Y + delta.Y)))
            {
                monster.Location = new Point(
                    monster.Location.X + delta.X,
                    monster.Location.Y + delta.Y);

                return true;
            }

            return false;
        }

        private bool CanMonsterStepInto(DungeonMonster monster, Point newPt)
        {
            if (IsMapSpaceBlocked(newPt.X, newPt.Y))
                return false;

            if (IsSpaceOccupiedByMonster(newPt.X, newPt.Y))
                return false;

            return true;
        }

        protected bool IsSpaceOccupiedByMonster(int xx, int yy)
        {
            return MonsterAt(Player.DungeonLevel, new Point(xx, yy)) != null;
        }

        public DungeonMonster MonsterAt(int dungeonLevel, Point loc)
        {
            return Combat.MonsterAt(dungeonLevel, loc);
        }
        private void SpawnMonster()
        {
            DungeonMonster monster = GetMonsterToSpawn();

            if (monster == null)
                return;

            do
            {
                monster.Location = new Point(
                    Random.Next(1, 15),
                    Random.Next(1, 15));

            } while (CanPlayerStepIntoImpl(monster.Location.X, monster.Location.Y) == false || monster.Location == Player.Location);

            monster.DungeonLevel = Player.DungeonLevel;

            Combat.Monsters.Add(monster);
        }

        public override void AfterExecuteCommand(KeyCode cmd)
        {
            base.AfterExecuteCommand(cmd);

            GameControl.Wait(100);
            UpdateMonsters();
        }

        public void ExecuteKillFlash()
        {
            SoundMan.PlaySoundSync(LotaSound.VeryBad);

            Combat.Monsters.RemoveAll(monst => monst.KillFlashImmune == false);
        }

        private void UseAttackMagic(MagicSpell magic)
        {
            int distance = 0;
            TextArea.PrintLine();
            TextArea.PrintLine("Shoot " + magic.Name + ".", XleColor.White);

            DungeonMonster monst = MonsterInFrontOfPlayer(Player, ref distance);
            var magicSound = magic.ID == 1 ? LotaSound.MagicFlame : LotaSound.MagicBolt;
            var hitSound = magic.ID == 1 ? LotaSound.MagicFlameHit : LotaSound.MagicBoltHit;

            if (monst == null)
            {
                SoundMan.PlayMagicSound(magicSound, hitSound, distance);
                TextArea.PrintLine("There is no effect.", XleColor.White);
            }
            else
            {
                if (RollSpellFizzle(magic))
                {
                    SoundMan.PlayMagicSound(magicSound, LotaSound.MagicFizzle, distance);
                    TextArea.PrintLine("Attack fizzles.", XleColor.White);
                    GameControl.Wait(500);
                }
                else
                {
                    SoundMan.PlayMagicSound(magicSound, hitSound, distance);
                    int damage = RollSpellDamage(magic, distance);

                    HitMonster(monst, damage, XleColor.White);
                }
            }
        }

        protected override void PlayerMagicImpl(MagicSpell magic)
        {
            switch (magic.ID)
            {
                case 1:
                case 2:
                    UseAttackMagic(magic);
                    break;

                default:
                    CastSpell(magic);
                    break;
            }
        }

        protected DungeonMonster MonsterInFrontOfPlayer(Player player)
        {
            int distance = 0;
            return MonsterInFrontOfPlayer(player, ref distance);
        }
        public DungeonMonster MonsterInFrontOfPlayer(Player player, ref int distance)
        {
            Point fightDir = player.FaceDirection.StepDirection();
            DungeonMonster monst = null;

            for (int i = 1; i <= 5; i++)
            {
                Point loc = new Point(player.X + fightDir.X * i, player.Y + fightDir.Y * i);

                distance = i;
                monst = MonsterAt(player.DungeonLevel, loc);

                if (monst != null)
                    break;
                if (CanPlayerStepIntoImpl(loc.X, loc.Y) == false)
                    break;
            }

            return monst;
        }

        public override bool CanPlayerStepIntoImpl(int xx, int yy)
        {
            if (IsSpaceOccupiedByMonster(xx, yy))
                return false;

            return base.CanPlayerStepIntoImpl(xx, yy);
        }

        private void HitMonster(DungeonMonster monst, int damage, Color clr)
        {
            TextArea.Print("Enemy hit by blow of ", clr);
            TextArea.Print(damage.ToString(), XleColor.White);
            TextArea.PrintLine("!");

            monst.HP -= damage;
            GameControl.Wait(1000);

            if (monst.HP <= 0)
            {
                Combat.Monsters.Remove(monst);
                TextArea.PrintLine(monst.Name + " dies!!");

                SoundMan.PlaySound(LotaSound.EnemyDie);

                GameControl.Wait(500);
            }
        }


    }
}
