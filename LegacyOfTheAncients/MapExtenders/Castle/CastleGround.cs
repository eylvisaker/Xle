using ERY.Xle.LotA.MapExtenders.Castle.Events;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.Maps.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERY.Xle.Maps;
using ERY.Xle.Rendering;
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

        public override void OnLoad(GameState state)
        {
            base.OnLoad(state);
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

        public override void PlayerUse(GameState state, int item, ref bool handled)
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

        public override void SpeakToGuard(GameState state)
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

        public override double ChanceToHitGuard(Player player, Guard guard, int distance)
        {
            int weaponType = player.CurrentWeapon.ID;
            double GuardDefense = 1;

            if (WhichCastle == 2)
                GuardDefense = player.Attribute[Attributes.dexterity] / 26.0;

            return (player.Attribute[Attributes.dexterity] + 13)
                * (99 + weaponType * 11) / 7500.0 / GuardDefense;
        }


        public override int RollDamageToGuard(Player player, Guard guard)
        {
            int weaponType = player.CurrentWeapon.ID;

            double damage = player.Attribute[Attributes.strength] *
                       (weaponType / 2 + 1) / 7;

            damage *= 1 + 2 * Random.NextDouble();

            return (int)Math.Round(damage);
        }


        public override double ChanceToHitPlayer(Player player, Guard guard)
        {
            return 1 - (player.Attribute[Attributes.dexterity] / 99.0);
        }


        public override int RollDamageToPlayer(Player player, Guard guard)
        {
            int armorType = player.CurrentArmor.ID;

            double damage =
                Math.Pow(CastleLevel, 1.8) * GuardAttack * (300 + Random.NextDouble() * 600) /
                (armorType + 2) / Math.Pow(player.Attribute[Attributes.endurance], 0.9) + 2;

            return (int)Math.Round(damage);
        }
    }
}
