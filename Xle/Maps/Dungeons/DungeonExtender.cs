using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xle.Maps.XleMapTypes;
using Xle.Services.Rendering;
using Xle.Services.Rendering.Maps;

namespace Xle.Maps.Dungeons
{
    public class DungeonExtender : Map3DExtender
    {
        private static Dictionary<int, string> trapNames = new Dictionary<int, string>()
        {
            { 0x11, "ceiling hole" },
            { 0x12, "floor hole" },
            { 0x13, "poison gas vent" },
            { 0x14, "slime splotch" },
            { 0x15, "trip wire" },
            { 0x16, "gas vent" }
        };
        private int hitAttemptCount = 0;

        public DungeonExtender()
        {
            Combat = new DungeonCombat();
        }

        public new Dungeon TheMap
        {
            get { return (Dungeon)base.TheMap; }
            set { base.TheMap = value; }
        }

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
            return trapNames[val];
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

        public override void CheckSounds(GameTime time)
        {
            // Used by LOTA to play the dungeon drip sounds.
        }

        public virtual DungeonMonster GetMonsterToSpawn()
        {
            return null;
        }

        public virtual bool RollToHitPlayer(DungeonMonster monster)
        {
            hitAttemptCount++;
            return hitAttemptCount % 2 == 1;
        }

        public virtual int RollDamageToPlayer(DungeonMonster monster)
        {
            return 4;
        }

        public virtual bool SpawnMonsters()
        {
            return true;
        }
        public virtual async Task UpdateMonsters()
        {
            foreach (var monster in Combat.Monsters.Where(x => x.DungeonLevel == Player.DungeonLevel))
            {
                var delta = new Point(
                    Player.X - monster.Location.X,
                    Player.Y - monster.Location.Y);

                if (Math.Abs(delta.X) + Math.Abs(delta.Y) == 1)
                {
                    await MonsterAttackPlayer(monster);
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

        private async Task DungeonLevelText()
        {
            CurrentLevel = Player.DungeonLevel;

            if (TheMap[Player.X, Player.Y] == 0x21) TheMap[Player.X, Player.Y] = 0x11;
            if (TheMap[Player.X, Player.Y] == 0x22) TheMap[Player.X, Player.Y] = 0x12;

            await TextArea.PrintLine("\n\nYou are now at level " + (Player.DungeonLevel + 1).ToString() + ".", XleColor.White);
        }

        public int CurrentLevel
        {
            get { return TheMap.CurrentLevel; }
            set { TheMap.CurrentLevel = value; }
        }

        private async Task OnPlayerAvoidTrap(int x, int y)
        {
            // don't print a message for ceiling holes
            if (TheMap[x, y] == 0x21) return;

            //string name = TrapName(this[x, y]);

            //TextArea.PrintLine();
            //TextArea.PrintLine("You avoid the " + name + ".");
            //XleCore.wait(150);
        }

        private async Task OnPlayerTriggerTrap(int x, int y)
        {
            // don't trigger ceiling holes
            if (TheMap[x, y] == 0x21) return;

            TheMap[x, y] -= 0x10;
            int damage = 31;
            await TextArea.PrintLine();

            if (TheMap[x, y] == 0x12)
            {
                await TextArea.PrintLine("You fall through a hidden hole.", XleColor.White);
            }
            else
            {
                await TextArea.PrintLine($"You're ambushed by a {TrapName(TheMap[x, y])}.", XleColor.White);
                await GameControl.WaitAsync(100);
            }

            await TextArea.PrintLine("   H.P. - " + damage.ToString(), XleColor.White);
            Player.HP -= damage;

            SoundMan.PlaySound(LotaSound.EnemyHit);
            await GameControl.WaitAsync(500);

            if (TheMap[x, y] == 0x12)
            {
                Player.DungeonLevel++;
                await DungeonLevelText();
            }
        }

        public override async Task AfterPlayerStep()
        {
            int val = TheMap[Player.X, Player.Y];

            CurrentLevel = Player.DungeonLevel;

            if (val >= 0x21 && val <= 0x2a)
            {
                await OnPlayerTriggerTrap(Player.X, Player.Y);
            }
            else if (val >= 0x11 && val <= 0x1a)
            {
                await OnPlayerAvoidTrap(Player.X, Player.Y);
            }
        }

        private async Task MonsterAttackPlayer(DungeonMonster monster)
        {
            await TextArea.PrintLine();

            var delta = new Point(
                monster.Location.X - Player.X,
                monster.Location.Y - Player.Y);

            var forward = Player.FaceDirection.StepDirection();
            var right = Player.FaceDirection.RightDirection();
            var left = Player.FaceDirection.LeftDirection();
            bool allowEffect = false;

            if (delta == forward)
            {
                await TextArea.Print("Attacked by ");
                await TextArea.Print(monster.Name, XleColor.Yellow);
                await TextArea.PrintLine("!");

                allowEffect = true;
            }
            else if (delta == right)
            {
                await TextArea.Print("Attacked from the ");
                await TextArea.Print("right", XleColor.Yellow);
                await TextArea.PrintLine(".");
            }
            else if (delta == left)
            {
                await TextArea.Print("Attacked from the ");
                await TextArea.Print("left", XleColor.Yellow);
                await TextArea.PrintLine(".");
            }
            else
            {
                await TextArea.Print("Attacked from ");
                await TextArea.Print("behind", XleColor.Yellow);
                await TextArea.PrintLine(".");
            }

            if (RollToHitPlayer(monster))
            {
                int damage = RollDamageToPlayer(monster);

                SoundMan.PlaySound(LotaSound.EnemyHit);
                await TextArea.Print("Hit by blow of ");
                await TextArea.Print(damage.ToString(), XleColor.Yellow);
                await TextArea.PrintLine("!");

                Player.HP -= damage;
            }
            else
            {
                SoundMan.PlaySound(LotaSound.EnemyMiss);
                await TextArea.PrintLine("Attack missed.", XleColor.Green);
            }

            await GameControl.WaitAsync(250);
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

        public override async Task AfterExecuteCommand(Keys cmd)
        {
            await base.AfterExecuteCommand(cmd);

            await GameControl.WaitAsync(100);
            await UpdateMonsters();
        }

        public void ExecuteKillFlash()
        {
            SoundMan.PlaySoundSync(LotaSound.VeryBad);

            Combat.Monsters.RemoveAll(monst => monst.KillFlashImmune == false);
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
    }
}
