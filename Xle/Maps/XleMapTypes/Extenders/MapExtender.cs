using AgateLib.Geometry;
using ERY.Xle.Commands;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.XleMapTypes.Extenders
{
	public class MapExtender 
	{
		public XleMap TheMap { get; set; }

		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return -1;
		}

		public virtual void OnLoad(GameState state)
		{ }


		public virtual void AfterPlayerStep(GameState state)
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


		public virtual void AfterEntry(GameState state)
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


		/// <summary>
		/// Returns the list of magic spells that can be used on this map.
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public virtual IEnumerable<MagicSpell> ValidMagic
		{
			get { yield break; }
		}

		public virtual void CastSpell(GameState state, MagicSpell magic)
		{
		}

		public virtual bool RollSpellFizzle(GameState state, MagicSpell magic)
		{
			return XleCore.random.Next(10) < 5;
		}

		public virtual int RollSpellDamage(GameState state, MagicSpell magic, int distance)
		{
			return (int)((magic.ID + 0.5) * 15 * (XleCore.random.NextDouble() + 1));
		}
	}
}
