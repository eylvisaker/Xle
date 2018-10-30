using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Xle.Data;
using Xle.Maps;
using Xle.Maps.Dungeons;
using Xle.Services.Commands;

namespace Xle.Ancients.MapExtenders.Dungeons
{
    public abstract class LotaDungeon : DungeonExtender
    {
        public LotaDungeon()
        {
            FillDrips();
        }

        public XleOptions Options { get; set; }

        public LotaStory Story
        {
            get { return GameState.Story(); }
        }


        public override void SetCommands(ICommandList commands)
        {
            commands.Items.Add(CommandFactory.Armor());
            commands.Items.Add(CommandFactory.Gamespeed());
            commands.Items.Add(CommandFactory.Hold());
            commands.Items.Add(CommandFactory.Inventory());
            commands.Items.Add(CommandFactory.Pass());
            commands.Items.Add(CommandFactory.Weapon());

            commands.Items.Add(CommandFactory.Climb("DungeonClimb"));
            commands.Items.Add(CommandFactory.End());
            commands.Items.Add(CommandFactory.Fight("LotaDungeonFight"));
            commands.Items.Add(CommandFactory.Magic("LotaDungeonMagic"));
            commands.Items.Add(CommandFactory.Open("DungeonOpen"));
            commands.Items.Add(CommandFactory.Use("LotaUse"));
            commands.Items.Add(CommandFactory.Xamine("LotaDungeonXamine"));
        }

        protected abstract int StrengthBoost { get; }
        protected abstract bool IsCompleted { get; set; }

        public override void OnLoad()
        {
            base.OnLoad();

            Story.BeenInDungeon = true;
        }

        protected async Task GivePermanentStrengthBoost()
        {
            Player.Attribute[Attributes.strength] += StrengthBoost;

            await TextArea.PrintLine("Strength + " + StrengthBoost);
            await GameControl.PlaySoundWait(LotaSound.VeryGood);
        }

        protected override bool ShowDirections()
        {
            // check for compass.
            return Player.Items[LotaItem.Compass] > 0;
        }

        /// <summary>
        /// Amount of time in seconds until next atmosphere sound.
        /// </summary>
        private double timeToNextSound = 3;

        private LotaSound[] drips;

        private void FillDrips()
        {
            drips = new LotaSound[2];
            drips[0] = LotaSound.Drip0;
            drips[1] = LotaSound.Drip1;
        }

        public override void CheckSounds(GameTime time)
        {
            timeToNextSound -= time.ElapsedGameTime.TotalSeconds;

            if (timeToNextSound <= 0)
            {
                ResetDripTime();

                SoundMan.PlaySound(drips[Random.Next(drips.Length)]);
            }
        }


        private void ResetDripTime()
        {
            timeToNextSound += Random.NextDouble() * 10 + 2;
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

        public override async Task CastSpell(MagicSpell magic)
        {
            await TextArea.PrintLine("Cast " + magic.Name + ".", XleColor.White);

            if (magic.ID == 3)
                await CastBefuddle(magic);
            if (magic.ID == 4)
                await CastPsychoStrength(magic);
            if (magic.ID == 5)
                await CastKillFlash(magic);
        }

        private async Task CastKillFlash(MagicSpell magic)
        {
            await ExecuteKillFlash();
        }

        private async Task CastPsychoStrength(MagicSpell magic)
        {
            throw new NotImplementedException();
        }

        private async Task CastBefuddle(MagicSpell magic)
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
                    await TextArea.PrintLine("The " + monst.Name + " looks confused.", XleColor.White);
                }
            }
        }

        public override async Task UpdateMonsters()
        {
            if (Story.BefuddleTurns > 0)
            {
                Story.BefuddleTurns--;
            }
            else
                await base.UpdateMonsters();
        }
    }
}
