using AgateLib;
using AgateLib.Platform;

using ERY.Xle.Data;
using ERY.Xle.Maps;
using ERY.Xle.Maps.Dungeons;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ERY.Xle.Services;
using ERY.Xle.Services.Commands;

namespace ERY.Xle.LotA.MapExtenders.Dungeons
{
    public abstract class LotaDungeon : DungeonExtender
    {
        public LotaDungeon()
        {
            FillDrips();

            nextSound = Timing.TotalSeconds + 3;
        }

        public XleOptions Options { get; set; }

        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

        public override IEnumerable<MagicSpell> ValidMagic
        {
            get
            {
                // everything but seek spell
                return from m in Data.MagicSpells
                       where m.Key != 6
                       select m.Value;
            }
        }
        public override bool PrintLevelDuringXamine
        {
            get { return Options.EnhancedGameplay; }
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LotaProgram.CommonLotaCommands);

            commands.Items.Add(CommandFactory.Fight("LotaDungeonFight"));
            commands.Items.Add(CommandFactory.Xamine("DungeonXamine"));
            commands.Items.Add(CommandFactory.Climb("DungeonClimb"));
            commands.Items.Add(CommandFactory.End());
            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Open("DungeonOpen"));
            commands.Items.Add(CommandFactory.Use("LotaUse"));
        }

        protected abstract int StrengthBoost { get; }
        protected abstract bool IsCompleted { get; set; }

        public override void OnLoad()
        {
            base.OnLoad();

            Story.BeenInDungeon = true;
        }

        protected void GivePermanentStrengthBoost()
        {
            Player.Attribute[Attributes.strength] += StrengthBoost;

            TextArea.PrintLine("Strength + " + StrengthBoost);
            SoundMan.PlaySoundSync(LotaSound.VeryGood);
        }

        protected override bool ShowDirections()
        {
            // check for compass.
            return Player.Items[LotaItem.Compass] > 0;
        }

        double nextSound;
        LotaSound[] drips;

        private void FillDrips()
        {
            drips = new LotaSound[2];
            drips[0] = LotaSound.Drip0;
            drips[1] = LotaSound.Drip1;
        }

        public override void CheckSounds()
        {
            if (Timing.TotalSeconds > nextSound)
            {
                ResetDripTime();

                SoundMan.PlaySound(drips[Random.Next(drips.Length)]);
            }
        }


        private void ResetDripTime()
        {
            double time = Random.NextDouble() * 10 + 2;

            nextSound = Timing.TotalSeconds + time;
        }


        public override DungeonMonster GetMonsterToSpawn()
        {
            if (Random.NextDouble() > 0.07)
                return null;

            int monsterID = Random.Next(6);

            if (Player.DungeonLevel >= 4)
                monsterID += 6;

            DungeonMonster monst = new DungeonMonster(
                Data.DungeonMonsters[monsterID]);

            monst.HP = (int)
                ((monsterID + 15 + 15 * Random.NextDouble()) * 2.4 * TheMap.MonsterHealthScale);

            return monst;
        }

        public override bool RollToHitPlayer(DungeonMonster monster)
        {
            if (Random.NextDouble() * 70 > Player.Attribute[Attributes.dexterity])
            {
                return true;
            }
            else
                return false;
        }

        public override int RollDamageToPlayer(DungeonMonster monster)
        {
            var armor = Player.CurrentArmor;
            double vc = armor.ID + armor.Quality / 3.5;

            double damage = 10 * TheMap.MonsterDamageScale / (vc + 3) * (Player.DungeonLevel + 7);

            return (int)((Random.NextDouble() + 0.5) * damage);
        }

        public override bool RollSpellFizzle(MagicSpell magic)
        {
            return Random.NextDouble() * 45 > Player.Attribute[Attributes.intelligence] || Random.NextDouble() < 0.05;
        }
        public override int RollSpellDamage(MagicSpell magic, int distance)
        {
            var dam = (1.0 / distance + .3) * 45 * (1 + Random.NextDouble()) * ((magic.ID == 2) ? 2 : 1);

            return (int)dam;
        }

        public override void CastSpell(MagicSpell magic)
        {
            TextArea.PrintLine("Cast " + magic.Name + ".", XleColor.White);

            if (magic.ID == 3)
                CastBefuddle(magic);
            if (magic.ID == 4)
                CastPsychoStrength(magic);
            if (magic.ID == 5)
                CastKillFlash(magic);
        }

        private void CastKillFlash(MagicSpell magic)
        {
            ExecuteKillFlash();
        }

        private void CastPsychoStrength(MagicSpell magic)
        {
            throw new NotImplementedException();
        }

        private void CastBefuddle(MagicSpell magic)
        {
            if (Player.HP >= 250 && Random.NextDouble() < 0.07)
            {
                //Backfire!!!
                Story.BackfiredBefuddleTurns = Random.Next(5, 10);
            }
            else
            {
                if (Story.BefuddleTurns > 0)
                    Story.BefuddleTurns /= 2;

                Story.BefuddleTurns += Random.Next(25, 35);

                var monst = MonsterInFrontOfPlayer(Player);

                if (monst != null)
                {
                    TextArea.PrintLine("The " + monst.Name + " looks confused.", XleColor.White);
                }
            }
        }

        public override void UpdateMonsters()
        {
            if (Story.BefuddleTurns > 0)
            {
                Story.BefuddleTurns--;
            }
            else
                base.UpdateMonsters();
        }
    }
}
