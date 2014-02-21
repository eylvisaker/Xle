using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleEventTypes.Extenders
{
	public interface IEventExtender
	{
		XleEvent TheEvent { get; set; }

		void Speak(GameState gameState, ref bool handled);
		void Rob(GameState state, ref bool handled);
		void Open(GameState state, ref bool handled);
		void Take(GameState state, ref bool handled);
		void StepOn(GameState state, ref bool handled);
		void TryToStepOn(GameState state, int dx, int dy, ref bool allowStep);
		void Use(GameState state, int item, ref bool handled);
		void Xamine(GameState state, ref bool handled);
	}
}
