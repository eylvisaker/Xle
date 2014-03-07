using AgateLib.Geometry;
using ERY.Xle.Commands;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class NullMapExtender : IMapExtender
	{
		public XleMap TheMap { get; set; }

		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return -1;
		}

		public virtual void OnLoad(GameState state)
		{ }


		public virtual void PlayerStep(GameState state)
		{ }

		public virtual void SetColorScheme(ColorScheme scheme)
		{
			throw new NotImplementedException();
		}


		public virtual IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return (IEventExtender)Activator.CreateInstance(defaultExtender);
		}


		public virtual int StepSize
		{
			get { return 1; }
		}


		public virtual void PlayerUse(GameState state, int item, ref bool handled)
		{
		}


		public virtual void BeforeEntry(GameState state, ref int targetEntryPoint)
		{
		}


		public virtual void OnAfterEntry(GameState state)
		{
		}


		public virtual void AfterExecuteCommand(GameState state, AgateLib.InputLib.KeyCode cmd)
		{
		}


		public virtual void SetCommands(CommandList commands)
		{

		}


		public virtual double ChanceToHitPlayer(Player player, Guard guard)
		{
			return (player.Attribute[Attributes.dexterity] / 80.0);
		}


		public virtual int RollDamageToPlayer(Player player, Guard guard)
		{
			int armorType = player.CurrentArmorType;

			double damage = guard.Attack / 99.0 *
							   (120 + XleCore.random.NextDouble() * 250) /
							   Math.Pow(armorType + 3, 0.8) /
								   Math.Pow(player.Attribute[Attributes.endurance], 0.8) + 3;

			return (int)Math.Round(damage);
		}
	}
}
