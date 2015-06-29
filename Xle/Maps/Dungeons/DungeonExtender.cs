using System;
using System.Linq;

using AgateLib.Geometry;
using AgateLib.InputLib;

using ERY.Xle.Data;
using ERY.Xle.Maps.XleMapTypes;
using ERY.Xle.Services.Rendering;
using ERY.Xle.Services.Rendering.Maps;

namespace ERY.Xle.Maps.Dungeons
{
    public class DungeonExtender : Map3DExtender
    {
        public DungeonExtender()
        {
            Combat = new DungeonCombat();
        }

        public IXleRenderer Renderer { get; set; }

        public new Dungeon TheMap { get { return (Dungeon)base.TheMap; } }

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
        public virtual void OnBeforeOpenBox(ref bool handled)
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

        public virtual bool RollToHitMonster(DungeonMonster monster)
        {
            return true;
        }

        public virtual int RollDamageToMonster(DungeonMonster monster)
        {
            return 9999;
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

        public virtual void PrintExamineMonsterMessage(DungeonMonster foundMonster, ref bool handled)
        {
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
        private void OnPlayerTriggerTrap(Player player, int x, int y)
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
            player.HP -= damage;

            SoundMan.PlaySound(LotaSound.EnemyHit);
            GameControl.Wait(500);

            if (TheMap[x, y] == 0x12)
            {
                player.DungeonLevel++;
                DungeonLevelText();
            }
        }

        public override void AfterPlayerStep()
        {
            int val = TheMap[Player.X, Player.Y];

            CurrentLevel = Player.DungeonLevel;

            if (val >= 0x21 && val <= 0x2a)
            {
                OnPlayerTriggerTrap(Player, Player.X, Player.Y);
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

        private DungeonMonster MonsterAt(int dungeonLevel, Point loc)
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

        public override bool PlayerOpen()
        {
            int val = TheMap[Player.X, Player.Y];
            bool clearBox = true;

            if (val == 0x1e)
            {
                OpenBox(ref clearBox);

                if (clearBox)
                    TheMap[Player.X, Player.Y] = 0x10;
            }
            else if (val >= 0x30 && val <= 0x3f)
            {
                OpenChest(val, ref clearBox);

                if (clearBox)
                    TheMap[Player.X, Player.Y] = 0x10;
            }
            else
            {
                TextArea.PrintLine();
                TextArea.PrintLine();
                TextArea.PrintLine("Nothing to open.");
                GameControl.Wait(1000);
            }


            return true;
        }

        public virtual void OpenBox(ref bool clearBox)
        {
            int amount = Random.Next(60, 200);

            TextArea.PrintLine(" Box");
            TextArea.PrintLine();
            SoundMan.PlaySound(LotaSound.OpenChest);
            GameControl.Wait(500);

            if (amount + Player.HP > Player.MaxHP)
            {
                amount = Player.MaxHP - Player.HP;
                if (amount < 0)
                    amount = 0;
            }

            bool handled = false;

            OnBeforeOpenBox(ref handled);

            if (handled == false)
            {
                if (amount == 0)
                    TextArea.PrintLine("You find nothing.", Color.Yellow);
                else
                {
                    TextArea.PrintLine("Hit points:  + " + amount.ToString(), XleColor.Yellow);
                    Player.HP += amount;
                    SoundMan.PlaySound(LotaSound.Good);
                    Renderer.FlashHPWhileSound(XleColor.Yellow);
                }
            }

            SoundMan.FinishSounds();
        }
        public virtual void OpenChest(int val, ref bool clearBox)
        {
            val -= 0x30;

            TextArea.PrintLine(" Chest");
            TextArea.PrintLine();

            SoundMan.PlaySound(LotaSound.OpenChest);
            GameControl.Wait(GameState.GameSpeed.DungeonOpenChestSoundTime);

            // TODO: give weapons
            // TODO: bobby trap chests.

            if (val == 0)
            {
                int amount = Random.Next(90, 300);

                TextArea.PrintLine("You find " + amount.ToString() + " gold.", XleColor.Yellow);

                Player.Gold += amount;

                Renderer.FlashHPWhileSound(XleColor.Yellow);
            }
            else
            {
                int treasure = GetTreasure(CurrentLevel + 1, val);

                bool handled = false;

                OnBeforeGiveItem(ref treasure, ref handled, ref clearBox);

                if (handled == false)
                {
                    if (treasure > 0)
                    {
                        string text = "You find a " + Data.ItemList[treasure].LongName + "!!";
                        TextArea.Clear();
                        TextArea.PrintLine(text);

                        Player.Items[treasure] += 1;

                        SoundMan.PlaySound(LotaSound.VeryGood);

                        TextArea.FlashLinesWhile(() => SoundMan.IsPlaying(LotaSound.VeryGood),
                            XleColor.White, XleColor.Yellow, 100);
                    }
                    else
                    {
                        TextArea.PrintLine("You find nothing.");
                    }
                }
            }
        }

        public override bool PlayerSpeak()
        {
            return false;
        }

        public override void AfterExecuteCommand(KeyCode cmd)
        {
            base.AfterExecuteCommand(cmd);

            GameControl.Wait(100);
            UpdateMonsters();
        }

        public override bool PlayerXamine()
        {
            SoundMan.PlaySound(LotaSound.Xamine);
            GameControl.Wait(500);

            Point faceDir = new Point();

            switch (Player.FaceDirection)
            {
                case Direction.East: faceDir = new Point(1, 0); break;
                case Direction.West: faceDir = new Point(-1, 0); break;
                case Direction.North: faceDir = new Point(0, -1); break;
                case Direction.South: faceDir = new Point(0, 1); break;
                default: break;
            }

            TextArea.PrintLine("\n");

            bool revealHidden = false;
            DungeonMonster foundMonster = null;

            for (int i = 0; i < 5; i++)
            {
                Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);

                foundMonster = MonsterAt(Player.DungeonLevel, loc);

                if (foundMonster != null)
                    break;
                if (TheMap[loc.X, loc.Y] < 0x10)
                    break;
                if (TheMap[loc.X, loc.Y] >= 0x21 && TheMap[loc.X, loc.Y] < 0x2a)
                {
                    TheMap[loc.X, loc.Y] -= 0x10;
                    revealHidden = true;
                }
            }

            if (revealHidden)
            {
                TextArea.PrintLine("Hidden objects detected!!!", XleColor.White);
                SoundMan.PlaySound(LotaSound.XamineDetected);
            }

            string extraText = string.Empty;
            int distance = 0;

            if (foundMonster != null)
            {
                bool handled = false;

                PrintExamineMonsterMessage(foundMonster, ref handled);

                if (false == handled)
                {
                    string name = " " + foundMonster.Name;
                    if ("aeiou".Contains(foundMonster.Name[0]))
                        name = "n" + name;

                    TextArea.PrintLine("A" + name + " is stalking you!", XleColor.White);
                }
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    Point loc = new Point(Player.X + faceDir.X * i, Player.Y + faceDir.Y * i);
                    int val = TheMap[loc.X, loc.Y];

                    if (val < 0x10) break;

                    if (extraText == string.Empty)
                    {
                        distance = i;

                        if (val > 0x10 && val < 0x1a)
                        {
                            extraText = TrapName(val);
                        }
                        if (val >= 0x30 && val <= 0x3f)
                        {
                            extraText = "treasure chest";
                        }
                        if (val == 0x1e)
                        {
                            extraText = "box";
                        }
                    }
                }

                if (extraText != string.Empty)
                {
                    if (distance > 0)
                    {
                        TextArea.PrintLine("A " + extraText + " is in sight.");
                    }
                    else
                    {
                        TextArea.PrintLine("You are standing next ");
                        TextArea.PrintLine("to a " + extraText + ".");
                    }
                }
                else
                {
                    if (PrintLevelDuringXamine)
                    {
                        TextArea.PrintLine("Level " + (Player.DungeonLevel + 1).ToString() + ".");
                    }

                    TextArea.PrintLine("Nothing unusual in sight.");
                }
            }

            return true;
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
        protected DungeonMonster MonsterInFrontOfPlayer(Player player, ref int distance)
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


        public override bool PlayerFight()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            int distance = 0;
            int maxDistance = 1;
            if (Player.CurrentWeapon.Info(Data).Ranged)
                maxDistance = 5;

            DungeonMonster monst = MonsterInFrontOfPlayer(Player, ref distance);

            if (monst == null)
            {
                TextArea.PrintLine("Nothing to fight.");
                return true;
            }
            else if (distance > maxDistance)
            {
                TextArea.PrintLine("The " + monst.Name + " is out-of-range");
                TextArea.PrintLine("of your " + Player.CurrentWeapon.BaseName(Data) + ".");
                return true;
            }

            bool hit = RollToHitMonster(monst);

            TextArea.Print("Hit ");
            TextArea.Print(monst.Name, XleColor.White);
            TextArea.PrintLine(" with " + Player.CurrentWeapon.BaseName(Data));

            if (hit)
            {
                int damage = RollDamageToMonster(monst);

                SoundMan.PlaySound(LotaSound.PlayerHit);

                HitMonster(monst, damage, XleColor.Cyan);
            }
            else
            {
                SoundMan.PlaySound(LotaSound.PlayerMiss);
                TextArea.PrintLine("Your attack misses.");
                GameControl.Wait(500);
            }

            return true;
        }

    }
}
