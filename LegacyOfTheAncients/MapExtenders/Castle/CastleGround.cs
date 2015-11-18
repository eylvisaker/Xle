using ERY.Xle.LotA.MapExtenders.Castle.Commands;
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
