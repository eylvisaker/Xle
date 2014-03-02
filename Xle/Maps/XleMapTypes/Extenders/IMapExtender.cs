﻿using AgateLib.Geometry;
using ERY.Xle.Commands;
using ERY.Xle.XleEventTypes.Extenders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERY.Xle.XleMapTypes.Extenders
{
	public interface IMapExtender
	{
		XleMap TheMap { get; set; }

		int GetOutsideTile(Point playerPoint, int x, int y);

		IEventExtender CreateEventExtender(XleEvent evt, Type defaultExtender);

		void SetColorScheme(ColorScheme scheme);

		int StepSize { get; }

		void PlayerUse(GameState state, int item, ref bool handled);

		void OnLoad(GameState state);

		void BeforeEntry(GameState state, ref int targetEntryPoint);

		void PlayerStep(GameState state);

		void OnAfterEntry(GameState state);

		void AfterExecuteCommand(GameState state, AgateLib.InputLib.KeyCode cmd);

		void SetCommands(CommandList commands);
	}
}
