using AgateLib.Geometry;

using ERY.Xle.Data;
using ERY.Xle.Maps.Renderers;
using ERY.Xle.Rendering;
using ERY.Xle.Services;
using ERY.Xle.Services.Implementation;
using ERY.Xle.Services.Implementation.Commands;
using ERY.Xle.XleEventTypes;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.Maps.Extenders
{
	public class MapExtender
	{
		XleMap mTheMap;

		public XleMap TheMap
		{
			get { return mTheMap; }
			set
			{
				mTheMap = value;
				MapRenderer = CreateMapRenderer();
				MapRenderer.TheMap = TheMap;
				MapRenderer.Extender = this;

				InitializeMapRenderer();
			}
		}

		protected virtual void InitializeMapRenderer()
		{
		}

		public XleMapRenderer MapRenderer { get; set; }
        public ICommandFactory CommandFactory { get; set; }
        public IEventExtenderFactory EventFactory { get; set; }

		public bool IsAngry
		{
			get { return TheMap.Guards.IsAngry; }
			set
			{
				TheMap.Guards.IsAngry = value;

				OnSetAngry(value);
			}
		}

		protected virtual void OnSetAngry(bool value)
		{
		}

		protected virtual XleMapRenderer CreateMapRenderer()
		{
			return new XleMapRenderer();
		}

		public virtual int GetOutsideTile(Point playerPoint, int x, int y)
		{
			return TheMap.OutsideTile;
		}

		public virtual void OnLoad(GameState state)
		{
        }

		public virtual void OnAfterEntry(GameState state)
		{ }

		public virtual void AfterPlayerStep(GameState state)
		{
			bool didEvent = false;

			foreach (var evt in TheMap.EventsAt(state.Player.X, state.Player.Y, 0))
			{
				evt.StepOn(state);
				didEvent = true;
			}

			AfterStepImpl(state, didEvent);
		}

		public virtual void SetColorScheme(ColorScheme scheme)
		{
			throw new NotImplementedException();
		}


		public virtual EventExtender CreateEventExtender(XleEvent evt, Type defaultExtender)
		{
			return (EventExtender)Activator.CreateInstance(defaultExtender);
		}


		public virtual int StepSize
		{
			get { return 1; }
		}


		public virtual void PlayerUse(GameState state, int item, ref bool handled)
		{
			handled = CommandNotImplemented();
		}


		public virtual void OnBeforeEntry(GameState state, ref int targetEntryPoint)
		{
		}

		public virtual void AfterExecuteCommand(GameState state, AgateLib.InputLib.KeyCode cmd)
		{
		}


		public virtual void SetCommands(ICommandList commands)
		{

		}


		public virtual double ChanceToHitPlayer(Player player, Guard guard)
		{
			return (player.Attribute[Attributes.dexterity] / 80.0);
		}


		public virtual int RollDamageToPlayer(Player player, Guard guard)
		{
			int armorType = player.CurrentArmor.ID;

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
		public virtual bool CanPlayerStepInto(GameState state, Point pt)
		{
			return CanPlayerStepIntoImpl(state.Player, pt.X, pt.Y);
		}
		public virtual bool CanPlayerStepIntoImpl(Player player, int xx, int yy)
		{
			return true;
		}

		[Obsolete("Use LeaveMap(GameState) overload instead.")]
		public virtual void LeaveMap(Player player)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Leave " + TheMap.MapName);
			XleCore.TextArea.PrintLine();

			XleCore.Wait(XleCore.GameState.GameSpeed.LeaveMapTime);

			player.ReturnToPreviousMap();

			XleCore.TextArea.PrintLine();
		}

		protected virtual void LeaveMap(GameState state)
		{
			XleCore.TextArea.PrintLine("Leave " + TheMap.MapName);
			XleCore.TextArea.PrintLine();

			XleCore.Wait(state.GameSpeed.LeaveMapTime);

			state.Player.ReturnToPreviousMap();
		}

		public virtual bool PlayerFight(GameState state)
		{
			return CommandNotImplemented();
		}

		public virtual bool PlayerRob(GameState state)
		{
			return CommandNotImplemented();
		}

		public virtual bool PlayerSpeak(GameState state)
		{
			foreach (var evt in TheMap.EnabledEventsAt(state.Player, 1))
			{
				bool handled = evt.Speak(state);

				if (handled)
					return handled;
			}

			return PlayerSpeakImpl(state);
		}

		protected virtual bool PlayerSpeakImpl(GameState state)
		{
			return false;
		}
		/// <summary>
		/// Function called when the player executes the Climb command.
		/// Returns true if the command was handled by this function, false
		/// if the caller should display a "Nothing to Climb" type message.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool PlayerClimb(GameState state)
		{
			return CommandNotImplemented();
		}
		/// <summary>
		/// Function called when the player executes the Xamine command.
		/// Returns true if the command was handled by this function, false
		/// if the caller should display a "Nothing to Xamine" type message.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool PlayerXamine(GameState state)
		{

			return CommandNotImplemented();
		}

		public virtual bool PlayerLeave(GameState state)
		{
			return CommandNotImplemented();
		}

		protected static bool CommandNotImplemented()
		{

			XleCore.TextArea.PrintLine("This command is not implemented.", Color.Red);
			XleCore.TextArea.PrintLine();

			SoundMan.PlaySoundSync(LotaSound.Medium);

			return false;
		}

		public virtual bool PlayerTake(GameState state)
		{
			foreach (var evt in TheMap.EnabledEventsAt(state.Player, 1))
			{
				if (evt.Take(state))
					return true;
			}

			return false;
		}

		public virtual bool PlayerOpen(GameState state)
		{
			foreach (var evt in TheMap.EnabledEventsAt(state.Player, 1))
			{
				if (evt.Open(state))
					return true;
			}

			return false;
		}
		/// <summary>
		/// Returns true if there was an effect of using the item.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool PlayerUse(GameState state, int item)
		{
			bool handled = false;

			foreach (var evt in TheMap.EventsAt(state.Player, 1))
			{
				handled = evt.Use(state, item);

				if (handled)
					return handled;
			}

			PlayerUse(state, item, ref handled);

			return handled;
		}

		public virtual bool PlayerDisembark(GameState state)
		{
			return false;
		}

		public virtual void PlayerMagic(GameState state)
		{
			var magics = ValidMagic.Where(x => state.Player.Items[x.ItemID] > 0).ToList();

			MagicSpell magic;

			if (UseFancyMagicPrompt)
				magic = MagicPrompt(state, magics.ToArray());
			else
				magic = MagicMenu(magics.ToArray());

			if (magic == null)
				return;

			if (state.Player.Items[magic.ItemID] <= 0)
			{
				XleCore.TextArea.PrintLine();
				XleCore.TextArea.PrintLine("You have no " + magic.PluralName + ".", XleColor.White);
				return;
			}

			state.Player.Items[magic.ItemID]--;

			PlayerMagicImpl(state, magic);
		}

		protected virtual void PlayerMagicImpl(GameState state, MagicSpell magic)
		{
		}


		protected virtual MagicSpell MagicPrompt(GameState state, MagicSpell[] magics)
		{
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine();
			XleCore.TextArea.PrintLine("Use which magic?", XleColor.Purple);
			XleCore.TextArea.PrintLine();

			bool hasFlames = magics.Contains(XleCore.Data.MagicSpells[1]);
			bool hasBolts = magics.Contains(XleCore.Data.MagicSpells[2]);

			int defaultValue = 0;
			int otherStart = 2 - (hasBolts ? 0 : 1) - (hasFlames ? 0 : 1);
			bool anyOthers = otherStart < magics.Length;

			if (hasFlames == false)
			{
				defaultValue = 1;

				if (hasBolts == false)
					defaultValue = 2;
			}

			var menu = new MenuItemList("Flame", "Bolt", anyOthers ? "Other" : "Nothing");

			int choice = XleCore.QuickMenu(menu, 2, defaultValue,
				XleColor.Purple, XleColor.White);

			if (choice == 0)
				return XleCore.Data.MagicSpells[1];
			else if (choice == 1)
				return XleCore.Data.MagicSpells[2];
			else
			{
				if (anyOthers == false)
					return null;

				XleCore.TextArea.PrintLine(" - select above", XleColor.White);
				XleCore.TextArea.PrintLine();

				return MagicMenu(magics.Skip(otherStart).ToList());
			}
		}

		private static MagicSpell MagicMenu(IList<MagicSpell> magics)
		{
			MenuItemList menu = new MenuItemList("Nothing");

			for (int i = 0; i < magics.Count; i++)
			{
				menu.Add(magics[i].Name);
			}

			int choice = XleCore.SubMenu("Pick magic", 0, menu);

			if (choice == 0)
			{
				XleCore.TextArea.PrintLine("Select no magic.", XleColor.White);
				return null;
			}

			return magics[choice - 1];
		}

		public virtual bool UseFancyMagicPrompt { get { return true; } }

		/// <summary>
		/// Executes the movement of the player in a certain direction.
		/// Assumes validation has already been performed. Call CanPlayerStep
		/// first to check to see if the movement is valid.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="stepDirection"></param>
		public virtual void MovePlayer(GameState state, Point stepDirection)
		{
			Point newPoint = new Point(state.Player.X + stepDirection.X, state.Player.Y + stepDirection.Y);

			BeforeStepOn(state, newPoint.X, newPoint.Y);

			state.Player.Location = newPoint;

			AfterPlayerStep(state);
		}

		public void BeforeStepOn(GameState state, int x, int y)
		{
			foreach (var evt in TheMap.EventsAt(x, y, 0))
			{
				evt.BeforeStepOn(state);
			}
		}

		/// <summary>
		/// Called after the player steps.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="didEvent">True if there was an event that occured at this location</param>
		protected virtual void AfterStepImpl(GameState state, bool didEvent)
		{

		}

		public bool CanPlayerStep(GameState state, Point stepDirection)
		{
			return CanPlayerStep(state, stepDirection.X, stepDirection.Y);
		}

		protected virtual bool CanPlayerStep(GameState state, int dx, int dy)
		{
			var player = state.Player;
			XleEvent evt = TheMap.GetEvent(player.X + dx, player.Y + dy, 0);

			if (evt != null)
			{
				bool allowStep;

				evt.TryToStepOn(state, dx, dy, out allowStep);

				if (allowStep == false)
					return false;
			}

			return CanPlayerStepIntoImpl(player, player.X + dx, player.Y + dy);
		}

		public virtual void PlayerCursorMovement(GameState state, Direction dir)
		{

		}

		public virtual void CheckSounds(GameState state)
		{
		}
	}
}
