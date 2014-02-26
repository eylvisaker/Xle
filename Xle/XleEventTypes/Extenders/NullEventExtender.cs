using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public class NullEventExtender : IEventExtender
	{
		public XleEvent TheEvent { get;set;}

		public virtual void BeforeStepOn(GameState state)
		{

		}
		public virtual void Speak(GameState state, ref bool handled)
		{
		}

		public virtual void Rob(GameState state, ref bool handled)
		{
		}

		public virtual void Open(GameState state, ref bool handled)
		{
		}

		public virtual void Take(GameState state, ref bool handled)
		{
		}

		public virtual void StepOn(GameState state, ref bool handled)
		{
		}

		public virtual void TryToStepOn(GameState state, int dx, int dy, ref bool allowStep)
		{
		}

		public virtual void Use(GameState state, int item, ref bool handled)
		{
		}

		public virtual void Xamine(GameState state, ref bool handled)
		{
		}

		public virtual void OnLoad(GameState state)
		{ }
	}
}
