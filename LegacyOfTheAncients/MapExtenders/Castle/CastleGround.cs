using Xle.Ancients.MapExtenders.Castle.Commands;
using Xle.Maps;
using Xle.Maps.Castles;
using Xle.Services.Commands;
using Microsoft.Xna.Framework;
using System;

namespace Xle.Ancients.MapExtenders.Castle
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
        }

        public override void SetCommands(ICommandList commands)
        {
            commands.Items.AddRange(LotaProgram.CommonLotaCommands);

            var fight = (CastleFight)CommandFactory.Fight("CastleFight");
            fight.WhichCastle = WhichCastle;
            fight.CastleLevel = CastleLevel;

            commands.Items.Add(fight);
            commands.Items.Add(CommandFactory.Magic("CastleMagic"));
            commands.Items.Add(CommandFactory.Open());
            commands.Items.Add(CommandFactory.Take());
            commands.Items.Add(CommandFactory.Speak("CastleSpeak"));
            commands.Items.Add(CommandFactory.Xamine());
            commands.Items.Add(CommandFactory.Use("CastleUse"));
        }

        public override int GetOutsideTile(Point playerPoint, int x, int y)
        {
            if (y >= TheMap.Height)
                return 16;
            else
                return base.GetOutsideTile(playerPoint, x, y);
        }

        protected override void OnSetAngry(bool value)
        {
            Story.Invisible = false;
            Player.RenderColor = XleColor.White;
        }

        protected int WhichCastle = 1;
        protected int CastleLevel = 1;
        protected double GuardAttack = 1;

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
