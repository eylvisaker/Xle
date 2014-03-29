﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class EventExtender
	{
		public XleEvent TheEvent { get;set;}

		/// <summary>
		/// Function called when player speaks in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Speak(GameState state)
		{
			bool handled = false;
			Speak(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when player executes Rob in a square inside or next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Rob(GameState state)
		{
			bool handled = false;
			Rob(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player executes the Open command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Open(GameState state)
		{
			bool handled = false;
			Open(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player executes the Take command inside
		/// or next to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Take(GameState state)
		{
			bool handled = false;
			Take(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player walks inside
		/// the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool StepOn(GameState state)
		{
			bool handled = false;
			StepOn(state, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player tries to walk inside
		/// the XleEvent.
		/// 
		/// This is before the position is updated.  Returns false to 
		/// block the player from stepping there, and true if the
		/// player can walk there.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <returns></returns>
		public virtual void TryToStepOn(GameState state, int dx, int dy, out bool allowStep)
		{
			allowStep = true;
		}
		/// <summary>
		/// Function called when the player uses an item
		/// or next to the XleEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual bool Use(GameState state, int item)
		{
			bool handled = false;
			Use(state, item, ref handled);

			return handled;
		}
		/// <summary>
		/// Function called when the player eXamines next
		/// to the LotaEvent.
		/// 
		/// Returns true if handled by the event.
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public virtual bool Xamine(GameState state)
		{
			bool handled = false;
			Xamine(state, ref handled);

			return handled;
		}

		public virtual void BeforeStepOn(GameState state)
		{

		}
		[Obsolete]
		public virtual void Speak(GameState state, ref bool handled)
		{
		}
		[Obsolete]
		public virtual void Rob(GameState state, ref bool handled)
		{
		}
		[Obsolete]
		public virtual void Open(GameState state, ref bool handled)
		{
		}
[Obsolete]
		public virtual void Take(GameState state, ref bool handled)
		{
		}
		[Obsolete]
		public virtual void StepOn(GameState state, ref bool handled)
		{
		}
		[Obsolete]
		public virtual void Use(GameState state, int item, ref bool handled)
		{
		}
		[Obsolete]
		public virtual void Xamine(GameState state, ref bool handled)
		{
		}

		public virtual void OnLoad(GameState state)
		{ }
	}
}
