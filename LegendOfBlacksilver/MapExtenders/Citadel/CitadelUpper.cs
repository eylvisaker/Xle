using ERY.Xle.LoB.MapExtenders.Citadel.EventExtenders;
using ERY.Xle.Maps.Castles;
using ERY.Xle.Services;
using ERY.Xle.Services.Commands;
using ERY.Xle.XleEventTypes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgateLib.Mathematics.Geometry;
using ERY.Xle.Maps;

namespace ERY.Xle.LoB.MapExtenders.Citadel
{
	public class CitadelUpper : CastleExtender
	{
		CastleDamageCalculator cdc;

		public CitadelUpper(Random random)
		{
			cdc = new CastleDamageCalculator(random) { v5 = 1.6, v6 = 5.5, v7 = 2.3 };
		}
		public override void SetColorScheme(ColorScheme scheme)
		{
			scheme.TextColor = XleColor.White;

			scheme.FrameColor = XleColor.Orange;
			scheme.FrameHighlightColor = XleColor.Yellow;
		}

		public override void SetCommands(ICommandList commands)
		{
			commands.Items.AddRange(LobProgram.CommonLobCommands);

			var fight = (LobCastleFight)CommandFactory.Fight("LobCastleFight");
			fight.DamageCalculator = cdc;

			commands.Items.Add(fight);

			commands.Items.Add(CommandFactory.Open());
			commands.Items.Add(CommandFactory.Magic("LobCastleMagic"));
			commands.Items.Add(CommandFactory.Take());
			commands.Items.Add(CommandFactory.Use("LobUse"));
			commands.Items.Add(CommandFactory.Speak());
			commands.Items.Add(CommandFactory.Xamine());
		}

		public override double ChanceToHitGuard(Guard guard, int distance)
		{
			return cdc.ChanceToHitGuard(Player, distance);
		}
		public override double ChanceToHitPlayer(Guard guard)
		{
			return cdc.ChanceToHitPlayer(Player);
		}
		public override int RollDamageToGuard(Guard guard)
		{
			return cdc.RollDamageToGuard(Player);
		}
		public override int RollDamageToPlayer(Guard guard)
		{
			return cdc.RollDamageToPlayer(Player);
		}

		public override int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return TheMap.OutsideTile;
		}

	}
}
