using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.Maps.Castles;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.XleEventTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;

using AgateLib.Geometry;

namespace ERY.Xle.LotA.MapExtenders.Castle
{
    public class CastleGround : CastleExtender
    {
        public CastleGround()
        {

        }

        public LotaMuseumCoinSale MuseumCoinSale { get; set; }

        public LotaStory Story
        {
            get { return GameState.Story(); }
        }

        public override void OnLoad()
        {
            base.OnLoad();
            MuseumCoinSale.ResetMuseumCoinOffers();
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LotaProgram.CommonLotaCommands);

            commands.Items.Add(CommandFactory.Magic());
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Speak());
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            if (y >= TheMap.Height)
                return 16;
            else
                return base.GetOutsideTile(playerPoint, x, y);
        }

        public override void PlayerUse(int item, ref bool handled)
        {
            switch (item)
            {
                case (int)LotaItem.MagicSeed:
                    handled = UseMagicSeeds();
                    break;
            }
        }
        private bool UseMagicSeeds()
        {
            GameControl.Wait(150);

            Story.Invisible = true;
            TextArea.PrintLine("You're invisible.");
            Player.RenderColor = XleColor.DarkGray;

            TheMap.Guards.IsAngry = false;

            GameControl.Wait(500);

            Player.Items[LotaItem.MagicSeed]--;

            return true;
        }

        public override void SpeakToGuard()
        {
            TextArea.PrintLine();
            TextArea.PrintLine();

            if (Story.Invisible)
            {
                TextArea.PrintLine("The guard looks startled.");
            }
            else
            {
                TextArea.PrintLine("The guard ignores you.");
            }
        }

        protected override void OnSetAngry(bool value)
        {
            Story.Invisible = false;
            Player.RenderColor = XleColor.White;
        }

        protected int WhichCastle = 1;
        protected double CastleLevel = 1;
        protected double GuardAttack = 1;

        public override double ChanceToHitGuard(Guard guard, int distance)
        {
            int weaponType = Player.CurrentWeapon.ID;
            double GuardDefense = 1;

            if (WhichCastle == 2)
                GuardDefense = Player.Attribute[Attributes.dexterity] / 26.0;

            return (Player.Attribute[Attributes.dexterity] + 13)
                * (99 + weaponType * 11) / 7500.0 / GuardDefense;
        }


        public override int RollDamageToGuard(Guard guard)
        {
            int weaponType = Player.CurrentWeapon.ID;

            double damage = Player.Attribute[Attributes.strength] *
                       (weaponType / 2 + 1) / 7;

            damage *= 1 + 2 * Random.NextDouble();

            return (int)Math.Round(damage);
        }


        public override double ChanceToHitPlayer(Guard guard)
        {
            return 1 - (Player.Attribute[Attributes.dexterity] / 99.0);
        }


        public override int RollDamageToPlayer(Guard guard)
        {
            int armorType = Player.CurrentArmor.ID;

            double damage =
                Math.Pow(CastleLevel, 1.8) * GuardAttack * (300 + Random.NextDouble() * 600) /
                (armorType + 2) / Math.Pow(Player.Attribute[Attributes.endurance], 0.9) + 2;

            return (int)Math.Round(damage);
        }
    }
}
