using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle
{
	public class GameState
	{
		public GameState()
		{ }

		public Player Player { get; set; }
		public XleMap Map { get; set; }

		public GameSpeed GameSpeed { get; set; }

		public Commands commands;
	}
}
